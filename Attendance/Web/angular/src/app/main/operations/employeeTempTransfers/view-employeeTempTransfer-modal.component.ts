import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { GetEmployeeTempTransferForViewDto, EmployeeTempTransferDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewEmployeeTempTransferModal',
    templateUrl: './view-employeeTempTransfer-modal.component.html'
})
export class ViewEmployeeTempTransferModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetEmployeeTempTransferForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetEmployeeTempTransferForViewDto();
        this.item.employeeTempTransfer = new EmployeeTempTransferDto();
    }

    show(item: GetEmployeeTempTransferForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
