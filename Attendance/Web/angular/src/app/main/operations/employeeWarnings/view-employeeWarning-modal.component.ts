import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { GetEmployeeWarningForViewDto, EmployeeWarningDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewEmployeeWarningModal',
    templateUrl: './view-employeeWarning-modal.component.html'
})
export class ViewEmployeeWarningModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetEmployeeWarningForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetEmployeeWarningForViewDto();
        this.item.employeeWarning = new EmployeeWarningDto();
    }

    show(item: GetEmployeeWarningForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
