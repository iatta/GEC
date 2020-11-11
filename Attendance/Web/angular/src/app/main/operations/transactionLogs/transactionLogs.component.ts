import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TransactionLogsServiceProxy, TransactionLogDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditTransactionLogModalComponent } from './create-or-edit-transactionLog-modal.component';
import { ViewTransactionLogModalComponent } from './view-transactionLog-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { EntityTypeHistoryModalComponent } from '@app/shared/common/entityHistory/entity-type-history-modal.component';
import * as _ from 'lodash';
import * as moment from 'moment';

@Component({
    templateUrl: './transactionLogs.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class TransactionLogsComponent extends AppComponentBase {

    @ViewChild('createOrEditTransactionLogModal', { static: true }) createOrEditTransactionLogModal: CreateOrEditTransactionLogModalComponent;
    @ViewChild('viewTransactionLogModalComponent', { static: true }) viewTransactionLogModal: ViewTransactionLogModalComponent;
    @ViewChild('entityTypeHistoryModal', { static: true }) entityTypeHistoryModal: EntityTypeHistoryModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    oldValueFilter = '';
    newValueFilter = '';
        transactionTransaction_DateFilter = '';
        userNameFilter = '';


    _entityTypeFullName = 'Pixel.Attendance.Operations.TransactionLog';
    entityHistoryEnabled = false;

    constructor(
        injector: Injector,
        private _transactionLogsServiceProxy: TransactionLogsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.entityHistoryEnabled = this.setIsEntityHistoryEnabled();
    }

    private setIsEntityHistoryEnabled(): boolean {
        let customSettings = (abp as any).custom;
        return customSettings.EntityHistory && customSettings.EntityHistory.isEnabled && _.filter(customSettings.EntityHistory.enabledEntities, entityType => entityType === this._entityTypeFullName).length === 1;
    }

    getTransactionLogs(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._transactionLogsServiceProxy.getAll(
            this.filterText,
            this.oldValueFilter,
            this.newValueFilter,
            this.transactionTransaction_DateFilter,
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

    createTransactionLog(): void {
        this.createOrEditTransactionLogModal.show();
    }

    showHistory(transactionLog: TransactionLogDto): void {
        this.entityTypeHistoryModal.show({
            entityId: transactionLog.id.toString(),
            entityTypeFullName: this._entityTypeFullName,
            entityTypeDescription: ''
        });
    }

    deleteTransactionLog(transactionLog: TransactionLogDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._transactionLogsServiceProxy.delete(transactionLog.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._transactionLogsServiceProxy.getTransactionLogsToExcel(
        this.filterText,
            this.oldValueFilter,
            this.newValueFilter,
            this.transactionTransaction_DateFilter,
            this.userNameFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
}
