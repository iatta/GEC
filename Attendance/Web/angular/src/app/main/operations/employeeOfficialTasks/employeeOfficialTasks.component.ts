import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { EmployeeOfficialTasksServiceProxy, EmployeeOfficialTaskDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditEmployeeOfficialTaskModalComponent } from './create-or-edit-employeeOfficialTask-modal.component';
import { ViewEmployeeOfficialTaskModalComponent } from './view-employeeOfficialTask-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';

@Component({
    templateUrl: './employeeOfficialTasks.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class EmployeeOfficialTasksComponent extends AppComponentBase {

    @ViewChild('createOrEditEmployeeOfficialTaskModal', { static: true }) createOrEditEmployeeOfficialTaskModal: CreateOrEditEmployeeOfficialTaskModalComponent;
    @ViewChild('viewEmployeeOfficialTaskModalComponent', { static: true }) viewEmployeeOfficialTaskModal: ViewEmployeeOfficialTaskModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    maxFromDateFilter : moment.Moment;
		minFromDateFilter : moment.Moment;
    maxToDateFilter : moment.Moment;
		minToDateFilter : moment.Moment;
    remarksFilter = '';
    descriptionArFilter = '';
    descriptionEnFilter = '';
        officialTaskTypeNameArFilter = '';




    constructor(
        injector: Injector,
        private _employeeOfficialTasksServiceProxy: EmployeeOfficialTasksServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private router: Router,
    ) {
        super(injector);
    }

    getEmployeeOfficialTasks(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._employeeOfficialTasksServiceProxy.getAll(
            this.filterText,
            this.maxFromDateFilter,
            this.minFromDateFilter,
            this.maxToDateFilter,
            this.minToDateFilter,
            this.remarksFilter,
            this.descriptionArFilter,
            this.descriptionEnFilter,
            this.officialTaskTypeNameArFilter,
            this.primengTableHelper.getSorting(this.dataTable),
            this.primengTableHelper.getSkipCount(this.paginator, event),
            this.primengTableHelper.getMaxResultCount(this.paginator, event)
        ).subscribe(result => {
            this.primengTableHelper.totalRecordsCount = result.totalCount;
            this.primengTableHelper.records = result.items;
            this.primengTableHelper.hideLoadingIndicator();
        });
    }



    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    createEmployeeOfficialTask(): void {

        this.router.navigate(['app/main/operations/manageOfficialTask']);
    }

    deleteEmployeeOfficialTask(employeeOfficialTask: EmployeeOfficialTaskDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._employeeOfficialTasksServiceProxy.delete(employeeOfficialTask.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._employeeOfficialTasksServiceProxy.getEmployeeOfficialTasksToExcel(
        this.filterText,
            this.maxFromDateFilter,
            this.minFromDateFilter,
            this.maxToDateFilter,
            this.minToDateFilter,
            this.remarksFilter,
            this.descriptionArFilter,
            this.descriptionEnFilter,
            this.officialTaskTypeNameArFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
}
