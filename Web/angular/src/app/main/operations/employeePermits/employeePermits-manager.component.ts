import { finalize } from 'rxjs/operators';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { EmployeePermitsServiceProxy, EmployeePermitDto, CreateOrEditEmployeePermitDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditEmployeePermitModalComponent } from './create-or-edit-employeePermit-modal.component';
import { ViewEmployeePermitModalComponent } from './view-employeePermit-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';

@Component({
    templateUrl: './employeePermits-manager.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
    styles:[`.approve{background-color:#3b6d3b !important;color:white !important}`]
})
export class EmployeePermitsManagerComponent extends AppComponentBase {

    @ViewChild('createOrEditEmployeePermitModal', { static: true }) createOrEditEmployeePermitModal: CreateOrEditEmployeePermitModalComponent;
    @ViewChild('viewEmployeePermitModalComponent', { static: true }) viewEmployeePermitModal: ViewEmployeePermitModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    maxPermitDateFilter : moment.Moment;
		minPermitDateFilter : moment.Moment;
    statusFilter = -1;
        userNameFilter = '';
        permitDescriptionArFilter = '';




    constructor(
        injector: Injector,
        private _employeePermitsServiceProxy: EmployeePermitsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    getEmployeePermits(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._employeePermitsServiceProxy.getAllForManager(
            this.filterText,
            this.maxPermitDateFilter,
            this.minPermitDateFilter,
            this.statusFilter,
            this.userNameFilter,
            this.permitDescriptionArFilter,
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

    createEmployeePermit(): void {
        this.createOrEditEmployeePermitModal.show();
    }

    deleteEmployeePermit(employeePermit: EmployeePermitDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._employeePermitsServiceProxy.delete(employeePermit.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._employeePermitsServiceProxy.getEmployeePermitsToExcel(
        this.filterText,
            this.maxPermitDateFilter,
            this.minPermitDateFilter,
            this.statusFilter,
            this.userNameFilter,
            this.permitDescriptionArFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }


    approvePermit(employeePermit: EmployeePermitDto) : void{
        this._employeePermitsServiceProxy.getEmployeePermitForEdit(employeePermit.id).subscribe(result => {
            result.employeePermit.status = true;
            this._employeePermitsServiceProxy.createOrEdit(result.employeePermit)
            .pipe(finalize(() => { }))
            .subscribe(() => {
                employeePermit.status = true;
               this.notify.info(this.l('SavedSuccessfully'));
            });

        });
    }

    rejectPermit(employeePermit: EmployeePermitDto) : void{

        this._employeePermitsServiceProxy.getEmployeePermitForEdit(employeePermit.id).subscribe(result => {
            result.employeePermit.status = false;
            this._employeePermitsServiceProxy.createOrEdit(result.employeePermit)
            .pipe(finalize(() => { }))
            .subscribe(() => {
                employeePermit.status = false;
               this.notify.info(this.l('SavedSuccessfully'));
            });

        });

    }
}
