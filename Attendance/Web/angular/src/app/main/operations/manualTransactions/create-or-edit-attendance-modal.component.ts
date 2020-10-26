import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { TransactionsServiceProxy, CreateOrEditTransactionDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { ManualTransactionUserLookupTableModalComponent } from './manualTransaction-user-lookup-table-modal.component';
import { ManualTransactionMachineLookupTableModalComponent } from './manualTransaction-machine-lookup-table-modal.component';
import { result } from 'lodash';
@Component({
    selector: 'createOrEditAttendanceModal',
    templateUrl: './create-or-edit-attendance-modal.component.html'
})
export class CreateOrEditAttendanceModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('manualTransactionUserLookupTableModal', { static: true }) manualTransactionUserLookupTableModal: ManualTransactionUserLookupTableModalComponent;
    @ViewChild('manualTransactionMachineLookupTableModal', { static: true }) manualTransactionMachineLookupTableModal: ManualTransactionMachineLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    inManualTransaction: CreateOrEditTransactionDto = new CreateOrEditTransactionDto();
    outManualTransaction: CreateOrEditTransactionDto = new CreateOrEditTransactionDto();

    userName = '';
    inTimeObj: Date = new Date();
    outTimeObj: Date = new Date();
    meridian = true;
    inManualTransactionId:number;
    outManualTransactionId:number;

    transactionLoaded = false;
    machineNameEn = '';


    constructor(
        injector: Injector,
        private _transactionsServiceProxy: TransactionsServiceProxy
    ) {
        super(injector);
    }

    show(inManualTransactionId?: number ,outManualTransactionId?: number,transDate?:moment.Moment,userId?:number): void {
        debugger
        this.inManualTransactionId = inManualTransactionId;
        this.outManualTransactionId = outManualTransactionId;
        if (!outManualTransactionId && !inManualTransactionId) {
            this.inManualTransaction = new CreateOrEditTransactionDto();
            this.inManualTransaction.id = null;
            this.inManualTransaction.transaction_Date = transDate;
            this.inManualTransaction.transaction_Date = transDate;
            this.inManualTransaction.pin = userId;
            this.inTimeObj.setHours(8);

            this.outManualTransaction = new CreateOrEditTransactionDto();
            this.outManualTransaction.id = null;
            this.outManualTransaction.transaction_Date = transDate;
            this.outManualTransaction.pin = userId;
            this.outTimeObj.setHours(8);

            this.userName = '';
            this.machineNameEn = '';

            this.active = true;
            this.modal.show();
        } else {
            this._transactionsServiceProxy.getTransactionForEdit(inManualTransactionId).subscribe(result => {
                this.inManualTransaction = result.transaction;
                //
                let minutes = (parseInt(this.inManualTransaction.time.split(':')[0]) * 60) + parseInt(this.inManualTransaction.time.split(':')[1]);


                this.inTimeObj.setHours(Math.floor(minutes / 60));
                this.inTimeObj.setMinutes(minutes % 60);
                this._transactionsServiceProxy.getTransactionForEdit(outManualTransactionId).subscribe(result => {
                    this.outManualTransaction = result.transaction;
                    //
                    let minutes = (parseInt(this.outManualTransaction.time.split(':')[0]) * 60) + parseInt(this.outManualTransaction.time.split(':')[1]);


                    this.outTimeObj.setHours(Math.floor(minutes / 60));
                    this.outTimeObj.setMinutes(minutes % 60);

                    this.userName = result.userName;
                    this.machineNameEn = result.machineNameEn;

                    this.active = true;
                    this.modal.show();

                })

            });
        }
    }

    save(): void {
        debugger
        this.saving = true;
        let strhour= ''
        let hours = this.inTimeObj.getHours();
        if(hours<10)
            strhour = '0'+hours.toString();
            else
            strhour = hours.toString();

        const time =  strhour + ':' + this.inTimeObj.getMinutes();
        this.inManualTransaction.time = time.toString();

        this._transactionsServiceProxy.createOrEdit(this.inManualTransaction)
        .subscribe(() => {
            debugger
            let strhour= ''
            let hours = this.outTimeObj.getHours();
            if(hours<10)
                strhour = '0'+hours.toString();
                else
                strhour = hours.toString();

            const time =  strhour + ':' + this.outTimeObj.getMinutes();
            this.outManualTransaction.time = time.toString();
            this.outManualTransaction.pin = this.inManualTransaction.pin;
            this.outManualTransaction.transaction_Date = this.inManualTransaction.transaction_Date;
            this.outManualTransaction.machineId = this.inManualTransaction.machineId;

            this._transactionsServiceProxy.createOrEdit(this.outManualTransaction)
            .pipe(finalize(() => { this.saving = false;}))
            .subscribe((res) => {
                debugger
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
            });
        });
    }




    openSelectUserModal() {
        this.manualTransactionUserLookupTableModal.id = this.inManualTransaction.pin;
        this.manualTransactionUserLookupTableModal.displayName = this.userName;
        this.manualTransactionUserLookupTableModal.show();
    }
    openSelectMachineModal() {
        this.manualTransactionMachineLookupTableModal.id = this.inManualTransaction.machineId;
        this.manualTransactionMachineLookupTableModal.displayName = this.machineNameEn;
        this.manualTransactionMachineLookupTableModal.show();
    }


    setUserIdNull() {
        this.inManualTransaction.pin = null;
        this.userName = '';
    }
    setMachineIdNull() {
        this.inManualTransaction.machineId = null;
        this.machineNameEn = '';
    }


    getNewUserId() {
        this.inManualTransaction.pin = this.manualTransactionUserLookupTableModal.id;
        this.userName = this.manualTransactionUserLookupTableModal.displayName;
    }
    getNewMachineId() {
        this.inManualTransaction.machineId = this.manualTransactionMachineLookupTableModal.id;
        this.machineNameEn = this.manualTransactionMachineLookupTableModal.displayName;
    }


    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
