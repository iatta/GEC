import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditEmployeeOfficialTaskDetailModalComponent } from './create-or-edit-employeeOfficialTaskDetail-modal.component';
import { ViewEmployeeOfficialTaskDetailModalComponent } from './view-employeeOfficialTaskDetail-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';

@Component({
    templateUrl: './employeeOfficialTaskDetails.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class EmployeeOfficialTaskDetailsComponent extends AppComponentBase {

    @ViewChild('createOrEditEmployeeOfficialTaskDetailModal', { static: true }) createOrEditEmployeeOfficialTaskDetailModal: CreateOrEditEmployeeOfficialTaskDetailModalComponent;
    @ViewChild('viewEmployeeOfficialTaskDetailModalComponent', { static: true }) viewEmployeeOfficialTaskDetailModal: ViewEmployeeOfficialTaskDetailModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
        employeeOfficialTaskDescriptionArFilter = '';
        userNameFilter = '';




    constructor(
        injector: Injector,

        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    getEmployeeOfficialTaskDetails(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();


    }

    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    createEmployeeOfficialTaskDetail(): void {
        this.createOrEditEmployeeOfficialTaskDetailModal.show();
    }

    deleteEmployeeOfficialTaskDetail(): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {

                }
            }
        );
    }

    exportToExcel(): void {

    }
}
