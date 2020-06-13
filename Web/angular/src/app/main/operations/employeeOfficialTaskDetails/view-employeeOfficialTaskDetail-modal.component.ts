import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import {  EmployeeOfficialTaskDetailDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewEmployeeOfficialTaskDetailModal',
    templateUrl: './view-employeeOfficialTaskDetail-modal.component.html'
})
export class ViewEmployeeOfficialTaskDetailModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;




    constructor(
        injector: Injector
    ) {
        super(injector);


    }



    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
