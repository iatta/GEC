import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { LeaveTypesServiceProxy, CreateOrEditLeaveTypeDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';

@Component({
    selector: 'createOrEditLeaveTypeModal',
    templateUrl: './create-or-edit-leaveType-modal.component.html'
})
export class CreateOrEditLeaveTypeModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    leaveType: CreateOrEditLeaveTypeDto = new CreateOrEditLeaveTypeDto();



    constructor(
        injector: Injector,
        private _leaveTypesServiceProxy: LeaveTypesServiceProxy
    ) {
        super(injector);
    }

    show(leaveTypeId?: number): void {

        if (!leaveTypeId) {
            this.leaveType = new CreateOrEditLeaveTypeDto();
            this.leaveType.id = leaveTypeId;

            this.active = true;
            this.modal.show();
        } else {
            this._leaveTypesServiceProxy.getLeaveTypeForEdit(leaveTypeId).subscribe(result => {
                this.leaveType = result.leaveType;


                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
            this.saving = true;

			
            this._leaveTypesServiceProxy.createOrEdit(this.leaveType)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }







    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
