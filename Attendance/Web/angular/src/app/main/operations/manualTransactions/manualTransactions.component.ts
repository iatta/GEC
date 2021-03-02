import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TransactionsServiceProxy, TransactionDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditManualTransactionModalComponent } from './create-or-edit-manualTransaction-modal.component';
import { ViewManualTransactionModalComponent } from './view-manualTransaction-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';

@Component({
    templateUrl: './manualTransactions.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class ManualTransactionsComponent extends AppComponentBase {

    @ViewChild('createOrEditManualTransactionModal', { static: true }) createOrEditManualTransactionModal: CreateOrEditManualTransactionModalComponent;
    @ViewChild('viewManualTransactionModalComponent', { static: true }) viewManualTransactionModal: ViewManualTransactionModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    maxTransDateFilter : moment.Moment;
		minTransDateFilter : moment.Moment;
        userNameFilter = '';
        machineNameEnFilter = '';
        maxTransTypeFilter : number;
        maxTransTypeFilterEmpty : number;
        minTransTypeFilter : number;
        minTransTypeFilterEmpty : number;



    constructor(
        injector: Injector,
        private _transactionsServiceProxy: TransactionsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    getManualTransactions(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._transactionsServiceProxy.getAll(
            this.filterText,
            this.maxTransTypeFilter == null ? this.maxTransTypeFilterEmpty: this.maxTransTypeFilter,
            this.minTransTypeFilter == null ? this.minTransTypeFilterEmpty: this.minTransTypeFilter,
            this.userNameFilter,
            this.maxTransDateFilter,
            this.minTransDateFilter,
            '',
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

    createManualTransaction(): void {
        this.createOrEditManualTransactionModal.show();
    }

    deleteManualTransaction(manualTransaction: TransactionDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._transactionsServiceProxy.delete(manualTransaction.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._transactionsServiceProxy.getTransactionsToExcel(
       this.filterText,
            this.maxTransTypeFilter == null ? this.maxTransTypeFilterEmpty: this.maxTransTypeFilter,
            this.minTransTypeFilter == null ? this.minTransTypeFilterEmpty: this.minTransTypeFilter
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }


}
