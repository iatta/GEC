import { UserShiftShiftLookupTableModalComponent } from './userShift-shift-lookup-table-modal.component';
import { Table } from 'primeng/table';
import { OrganizationUnitsHorizontalTreeModalComponent } from './../../../admin/shared/organization-horizontal-tree-modal.component';
import { Component, ViewChild, Injector, Output, EventEmitter, OnInit } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { TimeProfilesServiceProxy, ShiftTypesServiceProxy, ShiftsServiceProxy, ShiftDto, TimeProfileDetailDto, CreateOrEditTimeProfileDto, UserServiceProxy, UserListDto, GetShiftTypeForViewDto, GetShiftForViewDto, OrganizationUnitServiceProxy, CreateOrEditUserShiftDto, UserShiftsServiceProxy, OverrideShiftsServiceProxy, CreateOrEditOverrideShiftDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import {SelectItem} from 'primeng/api';
import { Router, ActivatedRoute, ParamMap ,Params } from '@angular/router';
import { NgForm } from '@angular/forms';
import { rowsAnimation } from '@shared/animations/template.animations';

class UserShiftDay{
    userId: number;
    shiftIds: number[] = [];
    shiftNames: string[] = [];
    userName: string;
    date:string;
}
@Component({
    selector: 'ManageUserShift',
    styleUrls:['./manager-user-shift.component.less'],
    templateUrl: './override-user-shift.component.html',
    animations: [rowsAnimation]
})
export class OverrideUserShiftComponent extends AppComponentBase implements OnInit {

    @ViewChild('organizationUnitsHorizontalTreeModal', { static: true }) organizationUnitsHorizontalTreeModal: OrganizationUnitsHorizontalTreeModalComponent;
    @ViewChild('userShiftShiftLookupTableModal', { static: true }) userShiftShiftLookupTableModal: UserShiftShiftLookupTableModalComponent;

    @ViewChild('dt', { static: true }) table: Table;


    active = false;
    saving = false;
    generatingDays=false;
    unitGlow=true;
    hideTable=true;
    loading = false;

    usersList: SelectItem[] = [];
    selectedUsers: UserListDto[] = [];

    shiftList: GetShiftForViewDto[] = [];
    selectedShift: GetShiftForViewDto;

    startDate: moment.Moment = moment();
    endDate: moment.Moment = moment();


    UserShiftDays: UserShiftDay[] = [];

    constructor(
        injector: Injector,
        private _userShiftsServiceProxy: OverrideShiftsServiceProxy,
        private _shiftsServiceProxy: ShiftsServiceProxy,
       private _userServiceProxy: UserServiceProxy,
    ) {
        super(injector);
    }
    ngOnInit(): void {
        this.initUserShift();

    }

    initUserShift() : void {
        this._shiftsServiceProxy.getAllFlat().subscribe((result)=>{
            this.shiftList = result;
            this.active = true;
        });

    }

    validateForm():boolean{
        let isvalid = true;
        if(this.selectedUsers.length == 0){
            this.notify.error('Please Select At Least One Employee');
            isvalid = false;
            return isvalid;
        }

        if(!this.selectedShift){
            this.notify.error('Please Select Shift');
            isvalid = false;
            return isvalid;
        }

        return isvalid;
    }

    getUsers(unitId:number){

        this._userServiceProxy.getUsersFlatByUnitId(unitId).subscribe((result) => {
            this.usersList = [];
            debugger
            result.forEach((user) => {
                this.usersList.push({label:user.name + '(' + user.fingerCode + ')', value:{id : user.id , name: user.name}});
            });
            this.unitGlow = false;
        });
    }

    caluculateDays(){
        this.UserShiftDays = [];
        this.selectedUsers.forEach(user => {
            for (var m = moment(this.startDate); m.diff(this.endDate, 'days') <= 0; m.add(1, 'days')) {

                let userShiftDayToAdd = new UserShiftDay();
                userShiftDayToAdd.userId = user.id;
                userShiftDayToAdd.shiftIds.push(this.selectedShift.shift.id);
                userShiftDayToAdd.shiftNames.push(this.selectedShift.shift.nameEn);
                userShiftDayToAdd.userName = user.name;
                userShiftDayToAdd.date = m.format('DD/MM/YYYY');
                this.UserShiftDays.push(userShiftDayToAdd);
            }

        });
    }

    //generate days
    generateDays():void{
        if(!this.validateForm())
            return;
        this.loading = true;
        this.caluculateDays();
        this.hideTable = false;
        this.loading = false;
        console.log(this.UserShiftDays);
    }



    onDateSelect(value) {
        this.table.filter(this.formatDate(value), 'day', 'equals');
    }

    formatDate(date) {
        let month = date.getMonth() + 1;
        let day = date.getDate();

        if (month < 10) {
            month = '0' + month;
        }

        if (day < 10) {
            day = '0' + day;
        }

        return  day + '/' + month  + '/' + date.getFullYear();
    }

    save(userShift : NgForm): void {

        if(!this.validateForm())
            return;
        //populate array
        this.loading = true;
        if(this.UserShiftDays.length == 0)
            this.caluculateDays();

        let model: CreateOrEditOverrideShiftDto[] =[];
        if(this.UserShiftDays.length > 0){
            this.UserShiftDays.forEach(UserShiftDay => {
                UserShiftDay.shiftIds.forEach(shiftId => {
                    let modelToAdd = new CreateOrEditOverrideShiftDto();
                    modelToAdd.userId = UserShiftDay.userId;
                    modelToAdd.shiftId = shiftId;
                    modelToAdd.day = moment(UserShiftDay.date , 'DD/MM/YYYY');
                    model.push(modelToAdd);
                });
            });
        }


        this._userShiftsServiceProxy.bulkCreateOrEdit(model).subscribe((result)=>{
            console.log(result);
            this.loading = false;
            this.notify.success("Success");
        });
    }


    resetData(){
    }

    openUnitModal(){
        this.organizationUnitsHorizontalTreeModal.show();
    }


    ouSelected(event: any): void {
        this.getUsers(event.id);
        //console.log(event);
    }


    close(): void {
        // this.active = false;
        // this.router.navigate(['app/main/operations/timeProfiles']);
       // this.modal.hide();
    }

    openShiftModal(userShiftDay:UserShiftDay): void {
        this.userShiftShiftLookupTableModal.show(userShiftDay.shiftIds,userShiftDay.date ,userShiftDay.userId);
    }

    getNewShifts(): void {
        //update shifts
        let selectedUserId = this.userShiftShiftLookupTableModal.selectedUserId;
        let selectedDay = this.userShiftShiftLookupTableModal.selectedDay;
        let selectedShiftIds = this.userShiftShiftLookupTableModal.selectedShiftIds;

        let userIndex = this.UserShiftDays.findIndex(x => x.date == selectedDay && x.userId == selectedUserId);
        if(userIndex != -1){
            this.UserShiftDays[userIndex].shiftIds = [];
            this.UserShiftDays[userIndex].shiftNames = [];

            selectedShiftIds.forEach(id => {
                let shift = this.shiftList.find(x => x.shift.id == id);
                this.UserShiftDays[userIndex].shiftNames.push(shift.shift.nameEn);
                this.UserShiftDays[userIndex].shiftIds.push(id);
            });
        }

    }
}
