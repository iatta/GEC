import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { TransactionLogsServiceProxy, CreateOrEditTransactionLogDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { TransactionLogTransactionLookupTableModalComponent } from './transactionLog-transaction-lookup-table-modal.component';
import { TransactionLogUserLookupTableModalComponent } from './transactionLog-user-lookup-table-modal.component';

@Component({
    selector: 'createOrEditTransactionLogModal',
    templateUrl: './create-or-edit-transactionLog-modal.component.html'
})
export class CreateOrEditTransactionLogModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('transactionLogTransactionLookupTableModal', { static: true }) transactionLogTransactionLookupTableModal: TransactionLogTransactionLookupTableModalComponent;
    @ViewChild('transactionLogUserLookupTableModal', { static: true }) transactionLogUserLookupTableModal: TransactionLogUserLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    transactionLog: CreateOrEditTransactionLogDto = new CreateOrEditTransactionLogDto();

    transactionTransaction_Date = '';
    userName = '';


    constructor(
        injector: Injector,
        private _transactionLogsServiceProxy: TransactionLogsServiceProxy
    ) {
        super(injector);
    }

    show(transactionLogId?: number): void {

        if (!transactionLogId) {
            this.transactionLog = new CreateOrEditTransactionLogDto();
            this.transactionLog.id = transactionLogId;
            this.transactionTransaction_Date = '';
            this.userName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._transactionLogsServiceProxy.getTransactionLogForEdit(transactionLogId).subscribe(result => {
                this.transactionLog = result.transactionLog;

                this.transactionTransaction_Date = result.transactionTransaction_Date;
                this.userName = result.userName;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
            this.saving = true;

			
            this._transactionLogsServiceProxy.createOrEdit(this.transactionLog)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectTransactionModal() {
        this.transactionLogTransactionLookupTableModal.id = this.transactionLog.transactionId;
        this.transactionLogTransactionLookupTableModal.displayName = this.transactionTransaction_Date;
        this.transactionLogTransactionLookupTableModal.show();
    }
    openSelectUserModal() {
        this.transactionLogUserLookupTableModal.id = this.transactionLog.actionBy;
        this.transactionLogUserLookupTableModal.displayName = this.userName;
        this.transactionLogUserLookupTableModal.show();
    }


    setTransactionIdNull() {
        this.transactionLog.transactionId = null;
        this.transactionTransaction_Date = '';
    }
    setActionByNull() {
        this.transactionLog.actionBy = null;
        this.userName = '';
    }


    getNewTransactionId() {
        this.transactionLog.transactionId = this.transactionLogTransactionLookupTableModal.id;
        this.transactionTransaction_Date = this.transactionLogTransactionLookupTableModal.displayName;
    }
    getNewActionBy() {
        this.transactionLog.actionBy = this.transactionLogUserLookupTableModal.id;
        this.userName = this.transactionLogUserLookupTableModal.displayName;
    }


    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
