import { SelectItem } from 'primeng/api';
import { OrganizationUnitsHorizontalTreeModalComponent } from './../../../admin/shared/organization-horizontal-tree-modal.component';
import { Component, ViewChild, Injector, Output, EventEmitter, OnInit } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { EmployeePermitsServiceProxy, CreateOrEditEmployeePermitDto, UserServiceProxy } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { EmployeePermitUserLookupTableModalComponent } from './employeePermit-user-lookup-table-modal.component';
import { EmployeePermitPermitLookupTableModalComponent } from './employeePermit-permit-lookup-table-modal.component';
import {NgForm} from '@angular/forms';
@Component({
    selector: 'createOrEditEmployeePermitModal',
    templateUrl: './create-or-edit-employeePermit-modal.component.html'
})
export class CreateOrEditEmployeePermitModalComponent extends AppComponentBase implements OnInit {

    
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('employeePermitUserLookupTableModal', { static: true }) employeePermitUserLookupTableModal: EmployeePermitUserLookupTableModalComponent;
    @ViewChild('employeePermitPermitLookupTableModal', { static: true }) employeePermitPermitLookupTableModal: EmployeePermitPermitLookupTableModalComponent;
    @ViewChild('organizationUnitsHorizontalTreeModal', { static: true }) organizationUnitsHorizontalTreeModal: OrganizationUnitsHorizontalTreeModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;
    meridian = true;

    employeePermit: CreateOrEditEmployeePermitDto = new CreateOrEditEmployeePermitDto();

    userName = '';
    permitDescriptionAr = '';
    usersList: SelectItem[] = [];

    toTimeObj: Date = new Date();
    fromTimeObj: Date = new Date();

    permitMaxNumberOfHours: number;

    constructor(
        injector: Injector,
        private _employeePermitsServiceProxy: EmployeePermitsServiceProxy,
        private _userServiceProxy: UserServiceProxy
    ) {
        super(injector);
    }
    ngOnInit(): void {
        this.usersList.push({label:'اختر الموظف ', value:null});
    }

    show(employeePermitId?: number): void {

        if (!employeePermitId) {
            this.employeePermit = new CreateOrEditEmployeePermitDto();
            this.employeePermit.id = employeePermitId;

            this.fromTimeObj.setHours(8);
            this.toTimeObj.setHours(10);
            
            
            this.employeePermit.permitDate = moment().startOf('day');
            this.userName = '';
            this.permitDescriptionAr = '';

            this.active = true;
            this.modal.show();
        } else {
            this._employeePermitsServiceProxy.getEmployeePermitForEdit(employeePermitId).subscribe(result => {
                this.employeePermit = result.employeePermit;

                this.fromTimeObj.setHours(Math.floor(this.employeePermit.fromTime / 60));
                this.fromTimeObj.setMinutes(this.employeePermit.fromTime % 60);

                this.toTimeObj.setHours(Math.floor(this.employeePermit.toTime / 60));
                this.toTimeObj.setMinutes(this.employeePermit.toTime % 60);

                this.userName = result.userName;
                this.permitDescriptionAr = result.permitDescriptionAr;
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

    save(employeePermitForm : NgForm): void {
        //this.employeePermitForm.submit
        
        if(employeePermitForm.form.valid){
            this.saving = true;
            this.employeePermit.toTime = (this.toTimeObj.getHours() * 60) + this.toTimeObj.getMinutes();
            this.employeePermit.fromTime = (this.fromTimeObj.getHours() * 60) + this.fromTimeObj.getMinutes();
            
            this._employeePermitsServiceProxy.createOrEdit(this.employeePermit)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
        }
           
    }


    openUnitModal(){
        this.organizationUnitsHorizontalTreeModal.show();
    }

    openSelectUserModal() {
        this.employeePermitUserLookupTableModal.id = this.employeePermit.userId;
        this.employeePermitUserLookupTableModal.displayName = this.userName;
        this.employeePermitUserLookupTableModal.show();
    }
    openSelectPermitModal() {
        this.employeePermitPermitLookupTableModal.id = this.employeePermit.permitId;
        this.employeePermitPermitLookupTableModal.displayName = this.permitDescriptionAr;
        this.employeePermitPermitLookupTableModal.show();
    }


    setUserIdNull() {
        this.employeePermit.userId = null;
        this.userName = '';
    }
    setPermitIdNull() {
        this.employeePermit.permitId = null;
        this.permitDescriptionAr = '';
    }


    getNewUserId() {
        this.employeePermit.userId = this.employeePermitUserLookupTableModal.id;
        this.userName = this.employeePermitUserLookupTableModal.displayName;
    }
    getNewPermitId() {
        this.employeePermit.permitId = this.employeePermitPermitLookupTableModal.id;
        this.permitDescriptionAr = this.employeePermitPermitLookupTableModal.displayName;

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
