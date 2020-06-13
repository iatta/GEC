import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { EmployeeWarningsServiceProxy, CreateOrEditEmployeeWarningDto,UserServiceProxy } from '@shared/service-proxies/service-proxies';
import {CustomServiceProxy} from '@shared/service-proxies/custom-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { EmployeeWarningUserLookupTableModalComponent } from './employeeWarning-user-lookup-table-modal.component';
import { EmployeeWarningWarningTypeLookupTableModalComponent } from './employeeWarning-warningType-lookup-table-modal.component';

@Component({
    selector: 'createOrEditEmployeeWarningModal',
    templateUrl: './create-or-edit-employeeWarning-modal.component.html'
})
export class CreateOrEditEmployeeWarningModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('employeeWarningUserLookupTableModal', { static: true }) employeeWarningUserLookupTableModal: EmployeeWarningUserLookupTableModalComponent;
    @ViewChild('employeeWarningWarningTypeLookupTableModal', { static: true }) employeeWarningWarningTypeLookupTableModal: EmployeeWarningWarningTypeLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    employeeWarning: CreateOrEditEmployeeWarningDto = new CreateOrEditEmployeeWarningDto();

    userName = '';
    warningTypeNameAr = '';


    constructor(
        injector: Injector,
        private _employeeWarningsServiceProxy: EmployeeWarningsServiceProxy,
        private _userServiceProxy: UserServiceProxy,
        private  _commonService: CustomServiceProxy
    ) {
        super(injector);
    }

    show(employeeWarningId?: number): void {

        if (!employeeWarningId) {
            this.employeeWarning = new CreateOrEditEmployeeWarningDto();
            this.employeeWarning.id = employeeWarningId;
            this.employeeWarning.fromDate = moment().startOf('day');
            this.employeeWarning.toDate = moment().startOf('day');
            this.userName = '';
            this.warningTypeNameAr = '';

            this.active = true;
            this.modal.show();
        } else {
            this._employeeWarningsServiceProxy.getEmployeeWarningForEdit(employeeWarningId).subscribe(result => {
                this.employeeWarning = result.employeeWarning;

                this.userName = result.userName;
                this.warningTypeNameAr = result.warningTypeNameAr;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
            this.saving = true;

			
            this._employeeWarningsServiceProxy.createOrEdit(this.employeeWarning)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {

                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this._commonService.generateWarningeport(this.employeeWarning).subscribe((result:any)=>{
                    
                    
                    const blob = new Blob([result.body], { type: 'application/pdf' });
                    let fileURL = URL.createObjectURL(blob);
                    window.open(fileURL);
                    
                });
                this.modalSave.emit(null);
             });
    }

    openSelectUserModal() {
        this.employeeWarningUserLookupTableModal.id = this.employeeWarning.userId;
        this.employeeWarningUserLookupTableModal.displayName = this.userName;
        this.employeeWarningUserLookupTableModal.show();
    }
    openSelectWarningTypeModal() {
        this.employeeWarningWarningTypeLookupTableModal.id = this.employeeWarning.warningTypeId;
        this.employeeWarningWarningTypeLookupTableModal.displayName = this.warningTypeNameAr;
        this.employeeWarningWarningTypeLookupTableModal.show();
    }


    setUserIdNull() {
        this.employeeWarning.userId = null;
        this.userName = '';
    }
    setWarningTypeIdNull() {
        this.employeeWarning.warningTypeId = null;
        this.warningTypeNameAr = '';
    }


    getNewUserId() {
        this.employeeWarning.userId = this.employeeWarningUserLookupTableModal.id;
        this.userName = this.employeeWarningUserLookupTableModal.displayName;
    }
    getNewWarningTypeId() {
        this.employeeWarning.warningTypeId = this.employeeWarningWarningTypeLookupTableModal.id;
        this.warningTypeNameAr = this.employeeWarningWarningTypeLookupTableModal.displayName;
    }


    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
