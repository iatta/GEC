import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { GetTransactionLogForViewDto, TransactionLogDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewTransactionLogModal',
    templateUrl: './view-transactionLog-modal.component.html'
})
export class ViewTransactionLogModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetTransactionLogForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetTransactionLogForViewDto();
        this.item.transactionLog = new TransactionLogDto();
    }

    show(item: GetTransactionLogForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
