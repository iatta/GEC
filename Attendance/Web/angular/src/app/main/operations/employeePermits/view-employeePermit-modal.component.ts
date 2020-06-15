import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { GetEmployeePermitForViewDto, EmployeePermitDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewEmployeePermitModal',
    templateUrl: './view-employeePermit-modal.component.html'
})
export class ViewEmployeePermitModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetEmployeePermitForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetEmployeePermitForViewDto();
        this.item.employeePermit = new EmployeePermitDto();
    }

    show(item: GetEmployeePermitForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
