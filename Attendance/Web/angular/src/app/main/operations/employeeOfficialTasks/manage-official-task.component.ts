import { Router, ActivatedRoute, Params } from '@angular/router';
import { OrganizationUnitsHorizontalTreeModalComponent } from './../../../admin/shared/organization-horizontal-tree-modal.component';
import { SelectItem } from 'primeng/api';
import { Component, ViewChild, Injector, Output, EventEmitter, OnInit } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { EmployeeOfficialTasksServiceProxy, CreateOrEditEmployeeOfficialTaskDto, UserListDto, UserServiceProxy } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { EmployeeOfficialTaskOfficialTaskTypeLookupTableModalComponent } from './employeeOfficialTask-officialTaskType-lookup-table-modal.component';

@Component({
    selector: 'manageOfficialTask',
    templateUrl: './manage-official-task.component.html'
})
export class ManageOfficialTaskComponent extends AppComponentBase implements OnInit {

    @ViewChild('employeeOfficialTaskOfficialTaskTypeLookupTableModal', { static: true }) employeeOfficialTaskOfficialTaskTypeLookupTableModal: EmployeeOfficialTaskOfficialTaskTypeLookupTableModalComponent;
    @ViewChild('organizationUnitsHorizontalTreeModal', { static: true }) organizationUnitsHorizontalTreeModal: OrganizationUnitsHorizontalTreeModalComponent;
    active = false;
    saving = false;

    employeeOfficialTask: CreateOrEditEmployeeOfficialTaskDto = new CreateOrEditEmployeeOfficialTaskDto();

    officialTaskTypeNameAr = '';
    usersList: SelectItem[] = [];


    constructor(
        injector: Injector,
        private _employeeOfficialTasksServiceProxy: EmployeeOfficialTasksServiceProxy,
        private _userServiceProxy: UserServiceProxy,
        private route: ActivatedRoute,
        private router: Router,
    ) {
        super(injector);
    }
    ngOnInit(): void {
         this.route.params.subscribe((params: Params) => {
             this.show(params['id']);
        });
    }

    show(employeeOfficialTaskId?: number): void {

        if (!employeeOfficialTaskId) {
            this.employeeOfficialTask = new CreateOrEditEmployeeOfficialTaskDto();
            this.employeeOfficialTask.id = employeeOfficialTaskId;
            this.employeeOfficialTask.officialTaskTypeId = null;
            this.employeeOfficialTask.fromDate = moment().startOf('day');
            this.employeeOfficialTask.toDate = moment().startOf('day');
            this.officialTaskTypeNameAr = '';

            this.active = true;
        } else {
            this._userServiceProxy.getUsersFlat().subscribe(result =>{
                result.forEach((user) => {
                    this.usersList.push({label:user.name, value:user.id});
                });
                this._employeeOfficialTasksServiceProxy.getEmployeeOfficialTaskForEdit(employeeOfficialTaskId).subscribe(result => {
                    console.log(result);

                    this.employeeOfficialTask = result.employeeOfficialTask;

                    this.officialTaskTypeNameAr = result.officialTaskTypeNameAr;

                    this.active = true;
                });

            });

        }
    }

    save(): void {
            this.saving = true;


            this._employeeOfficialTasksServiceProxy.createOrEdit(this.employeeOfficialTask)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.router.navigate(['app/main/operations/employeeOfficialTasks']);
             });
    }

    openUnitModal(){
        this.organizationUnitsHorizontalTreeModal.show();
    }
    back(){
        this.router.navigate(['app/main/operations/employeeOfficialTasks']);
    }

    getUsers(unitId:number){
        this._userServiceProxy.getUsersFlatByUnitId(unitId).subscribe((result) => {

            this.employeeOfficialTask.selectedUsers = [];
            result.forEach((user) => {
                this.usersList.push({label:user.name, value: user.id });
            });

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

    ouSelected(event: any): void {

        console.log('in manage time component');

        this.getUsers(event.id);
        //console.log(event);
    }


    close(): void {
        this.active = false;

    }
}
