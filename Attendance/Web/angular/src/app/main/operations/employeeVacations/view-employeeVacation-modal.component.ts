import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { GetEmployeeVacationForViewDto, EmployeeVacationDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewEmployeeVacationModal',
    templateUrl: './view-employeeVacation-modal.component.html'
})
export class ViewEmployeeVacationModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetEmployeeVacationForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetEmployeeVacationForViewDto();
        this.item.employeeVacation = new EmployeeVacationDto();
    }

    show(item: GetEmployeeVacationForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
