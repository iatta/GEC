import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { TransactionsServiceProxy, CreateOrEditTransactionDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { ManualTransactionUserLookupTableModalComponent } from './manualTransaction-user-lookup-table-modal.component';

@Component({
    selector: 'createOrEditManualTransactionModal',
    templateUrl: './create-or-edit-manualTransaction-modal.component.html'
})
export class CreateOrEditManualTransactionModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('manualTransactionUserLookupTableModal', { static: true }) manualTransactionUserLookupTableModal: ManualTransactionUserLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    manualTransaction: CreateOrEditTransactionDto = new CreateOrEditTransactionDto();

    userName = '';
    timeObj: Date = new Date();
    meridian = true;


    constructor(
        injector: Injector,
        private _transactionsServiceProxy: TransactionsServiceProxy
    ) {
        super(injector);
    }

    show(manualTransactionId?: number): void {

        if (!manualTransactionId) {
            this.manualTransaction = new CreateOrEditTransactionDto();
            this.manualTransaction.id = manualTransactionId;
            this.manualTransaction.transaction_Date = moment();
            
            
            this.timeObj.setHours(8);
            this.userName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._transactionsServiceProxy.getTransactionForEdit(manualTransactionId).subscribe(result => {
                this.manualTransaction = result.transaction;
                //
                const minutes = (parseInt(this.manualTransaction.time.split(':')[0]) * 60) + parseInt(this.manualTransaction.time.split(':')[1]);
                

                this.timeObj.setHours(Math.floor(minutes / 60));
                this.timeObj.setMinutes(minutes % 60);

                this.userName = result.userName;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        
            this.saving = true;
            const time = this.timeObj.getHours() + ':' + this.timeObj.getMinutes();
            this.manualTransaction.time = time.toString();

            this._transactionsServiceProxy.createOrEdit(this.manualTransaction)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectUserModal() {
        this.manualTransactionUserLookupTableModal.id = this.manualTransaction.pin;
        this.manualTransactionUserLookupTableModal.displayName = this.userName;
        this.manualTransactionUserLookupTableModal.show();
    }


    setUserIdNull() {
        this.manualTransaction.pin = null;
        this.userName = '';
    }


    getNewUserId() {
        this.manualTransaction.pin = this.manualTransactionUserLookupTableModal.id;
        this.userName = this.manualTransactionUserLookupTableModal.displayName;
    }


    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
