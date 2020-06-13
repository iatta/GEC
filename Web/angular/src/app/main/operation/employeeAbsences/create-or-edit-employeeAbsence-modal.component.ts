import { SelectItem } from 'primeng/api';
import { OrganizationUnitsHorizontalTreeModalComponent } from './../../../admin/shared/organization-horizontal-tree-modal.component';
import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { EmployeeAbsencesServiceProxy, CreateOrEditEmployeeAbsenceDto, UserServiceProxy } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { EmployeeAbsenceUserLookupTableModalComponent } from './employeeAbsence-user-lookup-table-modal.component';

@Component({
    selector: 'createOrEditEmployeeAbsenceModal',
    templateUrl: './create-or-edit-employeeAbsence-modal.component.html'
})
export class CreateOrEditEmployeeAbsenceModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('employeeAbsenceUserLookupTableModal', { static: true }) employeeAbsenceUserLookupTableModal: EmployeeAbsenceUserLookupTableModalComponent;
    @ViewChild('organizationUnitsHorizontalTreeModal', { static: true }) organizationUnitsHorizontalTreeModal: OrganizationUnitsHorizontalTreeModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;
    usersList: SelectItem[] = [];

    employeeAbsence: CreateOrEditEmployeeAbsenceDto = new CreateOrEditEmployeeAbsenceDto();

    userName = '';


    constructor(
        injector: Injector,
        private _employeeAbsencesServiceProxy: EmployeeAbsencesServiceProxy,
        private _userServiceProxy: UserServiceProxy,
    ) {
        super(injector);
    }

    show(employeeAbsenceId?: number): void {

        if (!employeeAbsenceId) {
            this.employeeAbsence = new CreateOrEditEmployeeAbsenceDto();
            this.employeeAbsence.id = employeeAbsenceId;
            this.employeeAbsence.fromDate = moment().startOf('day');
            this.employeeAbsence.toDate = moment().startOf('day');
            this.userName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._employeeAbsencesServiceProxy.getEmployeeAbsenceForEdit(employeeAbsenceId).subscribe(result => {
                this.employeeAbsence = result.employeeAbsence;

                this.userName = result.userName;
                this._userServiceProxy.getUsersFlat().subscribe(result =>{
                    this.usersList = [];
                    this.usersList.push({label:'اختر الموظف ', value:null});
                    result.forEach((user) => {
                        this.usersList.push({label:user.name, value:user.id});
                    });
                    this.active = true;
                    this.modal.show();
                });

            });
        }
    }


    openUnitModal(){
        this.organizationUnitsHorizontalTreeModal.show();
    }
    ouSelected(event: any): void {

        this.getUsers(event.id);
    }

    getUsers(unitId:number){
        this._userServiceProxy.getUsersFlatByUnitId(unitId).subscribe((result) => {
            console.log(result['items']);
            this.usersList = [];
            this.usersList.push({label:'اختر الموظف ', value:null});
            result.forEach((user) => {
                this.usersList.push({label:user.name, value:user.id});
            });

        });
    }

    save(): void {
            this.saving = true;


            this._employeeAbsencesServiceProxy.createOrEdit(this.employeeAbsence)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectUserModal() {
        this.employeeAbsenceUserLookupTableModal.id = this.employeeAbsence.userId;
        this.employeeAbsenceUserLookupTableModal.displayName = this.userName;
        this.employeeAbsenceUserLookupTableModal.show();
    }


    setUserIdNull() {
        this.employeeAbsence.userId = null;
        this.userName = '';
    }


    getNewUserId() {
        this.employeeAbsence.userId = this.employeeAbsenceUserLookupTableModal.id;
        this.userName = this.employeeAbsenceUserLookupTableModal.displayName;
    }


    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
