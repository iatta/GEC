import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { TransactionsServiceProxy, CreateOrEditTransactionDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { ManualTransactionUserLookupTableModalComponent } from './manualTransaction-user-lookup-table-modal.component';
import { result } from 'lodash';

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
    manualTransactionId:number;

    transactionLoaded = false;
    constructor(
        injector: Injector,
        private _transactionsServiceProxy: TransactionsServiceProxy
    ) {
        super(injector);
    }

    show(manualTransactionId?: number): void {
        this.manualTransactionId = manualTransactionId;
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
                let minutes = (parseInt(this.manualTransaction.time.split(':')[0]) * 60) + parseInt(this.manualTransaction.time.split(':')[1]);
                

                this.timeObj.setHours(Math.floor(minutes / 60));
                this.timeObj.setMinutes(minutes % 60);

                this.userName = result.userName;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        if( !this.manualTransaction.id && !this.transactionLoaded){
            this._transactionsServiceProxy.transactionExist(this.manualTransaction).subscribe((result)=>{
                debugger
                console.log(result);
                if(result.isExist){
                    this.message.confirm('This Transaction Alread Exist With Same Date And Same Type',' Do You Want To Reload It ?',(isConfirmed) => {
                        if(isConfirmed){
                            
                            this._transactionsServiceProxy.getTransactionForEdit(result.id).subscribe(result => {
                                this.transactionLoaded = true;
                                this.manualTransaction = result.transaction;
                                this.timeObj = new Date();
                                let minutes = (parseInt(this.manualTransaction.time.split(':')[0]) * 60) + parseInt(this.manualTransaction.time.split(':')[1]);
                                this.timeObj.setHours(Math.floor(minutes / 60));
                                this.timeObj.setMinutes(minutes % 60);
                                this.userName = result.userName;
                            });
                        }
                    });
                    
                }
            });
        }else{
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
