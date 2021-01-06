import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { TransactionsServiceProxy, CreateOrEditTransactionDto, TransactionLogsServiceProxy, GetTransactionLogForViewDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { ManualTransactionUserLookupTableModalComponent } from './manualTransaction-user-lookup-table-modal.component';
import { ManualTransactionMachineLookupTableModalComponent } from './manualTransaction-machine-lookup-table-modal.component';
import { result } from 'lodash';
@Component({
    selector: 'transactionInfoModal',
    templateUrl: './transaction-info-modal.component.html'
})

export class TransactionInfoeModalComponent extends AppComponentBase {
    @ViewChild('transactionInfoModal', { static: true }) modal: ModalDirective;

    active = false;
    data:GetTransactionLogForViewDto[]= [];

    constructor(
        injector: Injector,
        private _transactionLogsServiceProxy: TransactionLogsServiceProxy
    ) {
        super(injector);
    }

    show(intransId:number, outtransId:number){
        debugger
        this._transactionLogsServiceProxy.getTransactionLogByTransId(intransId,outtransId).subscribe((result) => {
            console.log(result);
            this.data = result;
            this.active = true;
            this.modal.show();

        })
    }


    close(): void {
        this.active = false;
        this.modal.hide();
    }

}
