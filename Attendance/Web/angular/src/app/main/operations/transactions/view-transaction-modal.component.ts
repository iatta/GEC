import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { GetTransactionForViewDto, TransactionDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewTransactionModal',
    templateUrl: './view-transaction-modal.component.html'
})
export class ViewTransactionModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetTransactionForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetTransactionForViewDto();
        this.item.transaction = new TransactionDto();
    }

    show(item: GetTransactionForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
