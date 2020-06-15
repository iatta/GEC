import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { GetManualTransactionForViewDto, ManualTransactionDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewManualTransactionModal',
    templateUrl: './view-manualTransaction-modal.component.html'
})
export class ViewManualTransactionModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetManualTransactionForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetManualTransactionForViewDto();
        this.item.manualTransaction = new ManualTransactionDto();
    }

    show(item: GetManualTransactionForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
