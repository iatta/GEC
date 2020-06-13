import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { EmployeeOfficialTaskDetailEmployeeOfficialTaskLookupTableModalComponent } from './employeeOfficialTaskDetail-employeeOfficialTask-lookup-table-modal.component';
import { EmployeeOfficialTaskDetailUserLookupTableModalComponent } from './employeeOfficialTaskDetail-user-lookup-table-modal.component';

@Component({
    selector: 'createOrEditEmployeeOfficialTaskDetailModal',
    templateUrl: './create-or-edit-employeeOfficialTaskDetail-modal.component.html'
})
export class CreateOrEditEmployeeOfficialTaskDetailModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('employeeOfficialTaskDetailEmployeeOfficialTaskLookupTableModal', { static: true }) employeeOfficialTaskDetailEmployeeOfficialTaskLookupTableModal: EmployeeOfficialTaskDetailEmployeeOfficialTaskLookupTableModalComponent;
    @ViewChild('employeeOfficialTaskDetailUserLookupTableModal', { static: true }) employeeOfficialTaskDetailUserLookupTableModal: EmployeeOfficialTaskDetailUserLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;



    employeeOfficialTaskDescriptionAr = '';
    userName = '';


    constructor(
        injector: Injector,

    ) {
        super(injector);
    }

    show(employeeOfficialTaskDetailId?: number): void {

        if (!employeeOfficialTaskDetailId) {


            this.employeeOfficialTaskDescriptionAr = '';
            this.userName = '';

            this.active = true;
            this.modal.show();
        } else {

        }
    }

    save(): void {
            this.saving = true;



    }

    openSelectEmployeeOfficialTaskModal() {

        this.employeeOfficialTaskDetailEmployeeOfficialTaskLookupTableModal.displayName = this.employeeOfficialTaskDescriptionAr;
        this.employeeOfficialTaskDetailEmployeeOfficialTaskLookupTableModal.show();
    }
    openSelectUserModal() {

        this.employeeOfficialTaskDetailUserLookupTableModal.displayName = this.userName;
        this.employeeOfficialTaskDetailUserLookupTableModal.show();
    }


    setEmployeeOfficialTaskIdNull() {

        this.employeeOfficialTaskDescriptionAr = '';
    }
    setUserIdNull() {

        this.userName = '';
    }


    getNewEmployeeOfficialTaskId() {

    }
    getNewUserId() {

    }


    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
