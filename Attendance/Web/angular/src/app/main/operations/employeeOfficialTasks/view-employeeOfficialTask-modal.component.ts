import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { GetEmployeeOfficialTaskForViewDto, EmployeeOfficialTaskDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewEmployeeOfficialTaskModal',
    templateUrl: './view-employeeOfficialTask-modal.component.html'
})
export class ViewEmployeeOfficialTaskModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetEmployeeOfficialTaskForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetEmployeeOfficialTaskForViewDto();
        this.item.employeeOfficialTask = new EmployeeOfficialTaskDto();
    }

    show(item: GetEmployeeOfficialTaskForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
