import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { GetLeaveTypeForViewDto, LeaveTypeDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewLeaveTypeModal',
    templateUrl: './view-leaveType-modal.component.html'
})
export class ViewLeaveTypeModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetLeaveTypeForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetLeaveTypeForViewDto();
        this.item.leaveType = new LeaveTypeDto();
    }

    show(item: GetLeaveTypeForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
