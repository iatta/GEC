import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { TransactionsServiceProxy, CreateOrEditTransactionDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';

@Component({
    selector: 'createOrEditTransactionModal',
    templateUrl: './create-or-edit-transaction-modal.component.html'
})
export class CreateOrEditTransactionModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    transaction: CreateOrEditTransactionDto = new CreateOrEditTransactionDto();



    constructor(
        injector: Injector,
        private _transactionsServiceProxy: TransactionsServiceProxy
    ) {
        super(injector);
    }

    show(transactionId?: number): void {

        if (!transactionId) {
            this.transaction = new CreateOrEditTransactionDto();
            this.transaction.id = transactionId;
            this.transaction.importDate = moment().startOf('day');

            this.active = true;
            this.modal.show();
        } else {
            this._transactionsServiceProxy.getTransactionForEdit(transactionId).subscribe(result => {
                this.transaction = result.transaction;


                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
            this.saving = true;

			
            this._transactionsServiceProxy.createOrEdit(this.transaction)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }







    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
