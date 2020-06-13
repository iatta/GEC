import { style } from '@angular/animations';
import { SelectItem } from 'primeng/api';
import { OrganizationUnitsHorizontalTreeModalComponent } from './../../../admin/shared/organization-horizontal-tree-modal.component';
import { Component, ViewChild, Injector, Output, EventEmitter, OnInit } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { EmployeeVacationsServiceProxy, CreateOrEditEmployeeVacationDto, UserServiceProxy } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { EmployeeVacationUserLookupTableModalComponent } from './employeeVacation-user-lookup-table-modal.component';
import { EmployeeVacationLeaveTypeLookupTableModalComponent } from './employeeVacation-leaveType-lookup-table-modal.component';

@Component({
    selector: 'createOrEditEmployeeVacationModal',
    templateUrl: './create-or-edit-employeeVacation-modal.component.html',
    styles: ['body .ui-dropdown .ui-dropdown-label{display: inline !important}']
})
export class CreateOrEditEmployeeVacationModalComponent extends AppComponentBase implements OnInit {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('employeeVacationUserLookupTableModal', { static: true }) employeeVacationUserLookupTableModal: EmployeeVacationUserLookupTableModalComponent;
    @ViewChild('employeeVacationLeaveTypeLookupTableModal', { static: true }) employeeVacationLeaveTypeLookupTableModal: EmployeeVacationLeaveTypeLookupTableModalComponent;
    @ViewChild('organizationUnitsHorizontalTreeModal', { static: true }) organizationUnitsHorizontalTreeModal: OrganizationUnitsHorizontalTreeModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    employeeVacation: CreateOrEditEmployeeVacationDto = new CreateOrEditEmployeeVacationDto();

    userName = '';
    leaveTypeNameAr = '';
    usersList: SelectItem[] = [];


    constructor(
        injector: Injector,
        private _employeeVacationsServiceProxy: EmployeeVacationsServiceProxy,
        private _userServiceProxy: UserServiceProxy,
    ) {
        super(injector);
    }
    ngOnInit(): void {
        this.usersList.push({label:'اختر الموظف ', value:null});
    }

    show(employeeVacationId?: number): void {

        if (!employeeVacationId) {
            this.employeeVacation = new CreateOrEditEmployeeVacationDto();
            this.employeeVacation.id = employeeVacationId;
            this.employeeVacation.fromDate = moment().startOf('day');
            this.employeeVacation.toDate = moment().startOf('day');
            this.userName = '';
            this.leaveTypeNameAr = '';

            this.active = true;
            this.modal.show();
        } else {
            this._employeeVacationsServiceProxy.getEmployeeVacationForEdit(employeeVacationId).subscribe(result => {

                console.log(result.employeeVacation);
                this.employeeVacation = result.employeeVacation;

                this.userName = result.userName;
                this.leaveTypeNameAr = result.leaveTypeNameAr;
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

    save(): void {
            this.saving = true;
            this._employeeVacationsServiceProxy.createOrEdit(this.employeeVacation)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectUserModal() {
        this.employeeVacationUserLookupTableModal.id = this.employeeVacation.userId;
        this.employeeVacationUserLookupTableModal.displayName = this.userName;
        this.employeeVacationUserLookupTableModal.show();
    }
    openSelectLeaveTypeModal() {
        this.employeeVacationLeaveTypeLookupTableModal.id = this.employeeVacation.leaveTypeId;
        this.employeeVacationLeaveTypeLookupTableModal.displayName = this.leaveTypeNameAr;
        this.employeeVacationLeaveTypeLookupTableModal.show();
    }


    setUserIdNull() {
        this.employeeVacation.userId = null;
        this.userName = '';
    }
    setLeaveTypeIdNull() {
        this.employeeVacation.leaveTypeId = null;
        this.leaveTypeNameAr = '';
    }


    getNewUserId() {
        this.employeeVacation.userId = this.employeeVacationUserLookupTableModal.id;
        this.userName = this.employeeVacationUserLookupTableModal.displayName;
    }
    getNewLeaveTypeId() {
        this.employeeVacation.leaveTypeId = this.employeeVacationLeaveTypeLookupTableModal.id;
        this.leaveTypeNameAr = this.employeeVacationLeaveTypeLookupTableModal.displayName;
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


    close(): void {
        this.active = false;
        this.modal.hide();
    }


}
