import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { EmployeeOfficialTasksServiceProxy, CreateOrEditEmployeeOfficialTaskDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { EmployeeOfficialTaskOfficialTaskTypeLookupTableModalComponent } from './employeeOfficialTask-officialTaskType-lookup-table-modal.component';

@Component({
    selector: 'createOrEditEmployeeOfficialTaskModal',
    templateUrl: './create-or-edit-employeeOfficialTask-modal.component.html'
})
export class CreateOrEditEmployeeOfficialTaskModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('employeeOfficialTaskOfficialTaskTypeLookupTableModal', { static: true }) employeeOfficialTaskOfficialTaskTypeLookupTableModal: EmployeeOfficialTaskOfficialTaskTypeLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    employeeOfficialTask: CreateOrEditEmployeeOfficialTaskDto = new CreateOrEditEmployeeOfficialTaskDto();

    officialTaskTypeNameAr = '';


    constructor(
        injector: Injector,
        private _employeeOfficialTasksServiceProxy: EmployeeOfficialTasksServiceProxy
    ) {
        super(injector);
    }

    show(employeeOfficialTaskId?: number): void {

        if (!employeeOfficialTaskId) {
            this.employeeOfficialTask = new CreateOrEditEmployeeOfficialTaskDto();
            this.employeeOfficialTask.id = employeeOfficialTaskId;
            this.employeeOfficialTask.fromDate = moment().startOf('day');
            this.employeeOfficialTask.toDate = moment().startOf('day');
            this.officialTaskTypeNameAr = '';

            this.active = true;
            this.modal.show();
        } else {
            this._employeeOfficialTasksServiceProxy.getEmployeeOfficialTaskForEdit(employeeOfficialTaskId).subscribe(result => {
                this.employeeOfficialTask = result.employeeOfficialTask;

                this.officialTaskTypeNameAr = result.officialTaskTypeNameAr;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
            this.saving = true;

			
            this._employeeOfficialTasksServiceProxy.createOrEdit(this.employeeOfficialTask)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectOfficialTaskTypeModal() {
        this.employeeOfficialTaskOfficialTaskTypeLookupTableModal.id = this.employeeOfficialTask.officialTaskTypeId;
        this.employeeOfficialTaskOfficialTaskTypeLookupTableModal.displayName = this.officialTaskTypeNameAr;
        this.employeeOfficialTaskOfficialTaskTypeLookupTableModal.show();
    }


    setOfficialTaskTypeIdNull() {
        this.employeeOfficialTask.officialTaskTypeId = null;
        this.officialTaskTypeNameAr = '';
    }


    getNewOfficialTaskTypeId() {
        this.employeeOfficialTask.officialTaskTypeId = this.employeeOfficialTaskOfficialTaskTypeLookupTableModal.id;
        this.officialTaskTypeNameAr = this.employeeOfficialTaskOfficialTaskTypeLookupTableModal.displayName;
    }


    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
