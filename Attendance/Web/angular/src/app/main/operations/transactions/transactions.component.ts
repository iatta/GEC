import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TransactionsServiceProxy, TransactionDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditTransactionModalComponent } from './create-or-edit-transaction-modal.component';
import { ViewTransactionModalComponent } from './view-transaction-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';

@Component({
    templateUrl: './transactions.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class TransactionsComponent extends AppComponentBase {

    @ViewChild('createOrEditTransactionModal', { static: true }) createOrEditTransactionModal: CreateOrEditTransactionModalComponent;
    @ViewChild('viewTransactionModalComponent', { static: true }) viewTransactionModal: ViewTransactionModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
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

    getTransactions(event?: LazyLoadEvent) {
        // if (this.primengTableHelper.shouldResetPaging(event)) {
        //     this.paginator.changePage(0);
        //     return;
        // }

        // this.primengTableHelper.showLoadingIndicator();

        // this._transactionsServiceProxy.getAll(
        //     this.filterText,
        //     this.maxTransTypeFilter == null ? this.maxTransTypeFilterEmpty: this.maxTransTypeFilter,
        //     this.minTransTypeFilter == null ? this.minTransTypeFilterEmpty: this.minTransTypeFilter,
        //     this.primengTableHelper.getSorting(this.dataTable),
        //     this.primengTableHelper.getSkipCount(this.paginator, event),
        //     this.primengTableHelper.getMaxResultCount(this.paginator, event)
        // ).subscribe(result => {
        //     this.primengTableHelper.totalRecordsCount = result.totalCount;
        //     this.primengTableHelper.records = result.items;
        //     this.primengTableHelper.hideLoadingIndicator();
        // });
    }

    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    createTransaction(): void {
        this.createOrEditTransactionModal.show();
    }

    deleteTransaction(transaction: TransactionDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._transactionsServiceProxy.delete(transaction.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        // this._transactionsServiceProxy.getTransactionsToExcel(
        // this.filterText,
        //     this.maxTransTypeFilter == null ? this.maxTransTypeFilterEmpty: this.maxTransTypeFilter,
        //     this.minTransTypeFilter == null ? this.minTransTypeFilterEmpty: this.minTransTypeFilter,
        // )
        // .subscribe(result => {
        //     this._fileDownloadService.downloadTempFile(result);
        //  });
    }
}
