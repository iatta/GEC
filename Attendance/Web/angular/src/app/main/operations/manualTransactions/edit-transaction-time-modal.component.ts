import { transition } from '@angular/animations';
import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { TransactionsServiceProxy, CreateOrEditTransactionDto ,TransactionDto} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { ManualTransactionUserLookupTableModalComponent } from './manualTransaction-user-lookup-table-modal.component';
import { result } from 'lodash';

@Component({
    selector: 'editTimeTransactionModal',
    templateUrl: './edit-transaction-time-modal.component.html'
})
export class EditTimeTransactionModalComponent extends AppComponentBase {

    @ViewChild('editTransactionTimeModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    manualTransaction: TransactionDto = new TransactionDto();

    userName = '';
    timeObj: Date = new Date();
    meridian = true;
    manualTransactionId:number;


    transactionLoaded = false;
    constructor(
        injector: Injector,
    ) {
        super(injector);
    }

    show(transaction: TransactionDto): void {

            this.manualTransaction =transaction;

            let minutes = (parseInt(this.manualTransaction.time.split(':')[0]) * 60) + parseInt(this.manualTransaction.time.split(':')[1]);
            this.timeObj.setHours(Math.floor(minutes / 60));
            this.timeObj.setMinutes(minutes % 60);

            this.active = true;
            this.modal.show();
    }

    save(): void {

        const time = this.timeObj.getHours() + ':' + this.timeObj.getMinutes();
        this.manualTransaction.time = time.toString();
        this.close();
        this.modalSave.emit(null);
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
