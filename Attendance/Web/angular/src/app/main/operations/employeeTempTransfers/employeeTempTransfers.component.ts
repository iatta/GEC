import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { EmployeeTempTransfersServiceProxy, EmployeeTempTransferDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditEmployeeTempTransferModalComponent } from './create-or-edit-employeeTempTransfer-modal.component';
import { ViewEmployeeTempTransferModalComponent } from './view-employeeTempTransfer-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';

@Component({
    templateUrl: './employeeTempTransfers.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class EmployeeTempTransfersComponent extends AppComponentBase {

    @ViewChild('createOrEditEmployeeTempTransferModal', { static: true }) createOrEditEmployeeTempTransferModal: CreateOrEditEmployeeTempTransferModalComponent;
    @ViewChild('viewEmployeeTempTransferModalComponent', { static: true }) viewEmployeeTempTransferModal: ViewEmployeeTempTransferModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    maxFromDateFilter : moment.Moment;
		minFromDateFilter : moment.Moment;
    maxToDateFilter : moment.Moment;
		minToDateFilter : moment.Moment;
        organizationUnitDisplayNameFilter = '';
        userNameFilter = '';




    constructor(
        injector: Injector,
        private _employeeTempTransfersServiceProxy: EmployeeTempTransfersServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    getEmployeeTempTransfers(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._employeeTempTransfersServiceProxy.getAll(
            this.filterText,
            this.maxFromDateFilter,
            this.minFromDateFilter,
            this.maxToDateFilter,
            this.minToDateFilter,
            this.organizationUnitDisplayNameFilter,
            this.userNameFilter,
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

    createEmployeeTempTransfer(): void {
        this.createOrEditEmployeeTempTransferModal.show();
    }

    deleteEmployeeTempTransfer(employeeTempTransfer: EmployeeTempTransferDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._employeeTempTransfersServiceProxy.delete(employeeTempTransfer.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._employeeTempTransfersServiceProxy.getEmployeeTempTransfersToExcel(
        this.filterText,
            this.maxFromDateFilter,
            this.minFromDateFilter,
            this.maxToDateFilter,
            this.minToDateFilter,
            this.organizationUnitDisplayNameFilter,
            this.userNameFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
}
