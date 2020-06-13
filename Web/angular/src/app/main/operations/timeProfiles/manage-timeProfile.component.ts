import { OrganizationUnitsHorizontalTreeModalComponent } from './../../../admin/shared/organization-horizontal-tree-modal.component';
import { Component, ViewChild, Injector, Output, EventEmitter, OnInit } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { TimeProfilesServiceProxy, ShiftTypesServiceProxy, ShiftsServiceProxy, ShiftDto, TimeProfileDetailDto, CreateOrEditTimeProfileDto, UserServiceProxy, UserListDto, GetShiftTypeForViewDto, GetShiftForViewDto, OrganizationUnitServiceProxy } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { TimeProfileUserLookupTableModalComponent } from './timeProfile-user-lookup-table-modal.component';
import {SelectItem} from 'primeng/api';
import { Router, ActivatedRoute, ParamMap ,Params } from '@angular/router';
import { NgForm } from '@angular/forms';

export interface IShiftDay {
    shifts: GetShiftForViewDto [] | [] ;
    selectedShifts: ShiftDto[] | [];
    shiftypesDropdown: SelectItem[]|[];
    header: string;
    shiftTypeId:number;
}
export class ShiftDay implements IShiftDay {
    shifts: GetShiftForViewDto [] | [] ;
    selectedShifts: ShiftDto[] | [];
    shiftypesDropdown: SelectItem[]|[];
    header: string;
    shiftTypeId:number;

    constructor(data?: IShiftDay) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }
}
@Component({
    selector: 'ManageTimeProfile',
    templateUrl: './manage-timeProfile.component.html'
})
export class ManageTimeProfileComponent extends AppComponentBase implements OnInit {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('timeProfileUserLookupTableModal', { static: true }) timeProfileUserLookupTableModal: TimeProfileUserLookupTableModalComponent;
    @ViewChild('organizationUnitsHorizontalTreeModal', { static: true }) organizationUnitsHorizontalTreeModal: OrganizationUnitsHorizontalTreeModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;


    timeProfile: CreateOrEditTimeProfileDto = new CreateOrEditTimeProfileDto();

    userName = '';
    usersList: SelectItem[] = [];
    shiftypesDropdown: SelectItem[] = [];
    selectedUsers: UserListDto[] = [];
    shiftTypesList: GetShiftTypeForViewDto[] = [];
    shiftList: GetShiftForViewDto[] = [];

    selectedShifts: ShiftDto[] = [] ;
    modelToSave: CreateOrEditTimeProfileDto[]  =[];

    days: ShiftDay[] = [];



    constructor(
        injector: Injector,
        private _timeProfilesServiceProxy: TimeProfilesServiceProxy,
        private _userServiceProxy: UserServiceProxy,
        private _shiftsServiceProxy: ShiftsServiceProxy,
        private _shiftTypesServiceProxy: ShiftTypesServiceProxy,
        private _organizationUnitService: OrganizationUnitServiceProxy,
        private router: Router,
    ) {
        super(injector);
    }
    ngOnInit(): void {
        this.initTimeProfile();

    }

    initTimeProfile() : void {
        this._shiftsServiceProxy.getAllFlat().subscribe((result)=>{
            this.shiftList = result;
            this._shiftTypesServiceProxy.getAllFlat().subscribe((result)=>{
                this.shiftTypesList = result;
                result.forEach((shiftType)=>{
                    this.shiftypesDropdown.push({label:shiftType.shiftType.descriptionAr, value: shiftType.shiftType.id });
                });
                this.show();
            });
        });

    }
    onUserChange(event){
        console.log(event);
    }

    getUsers(unitId:number){
        this._userServiceProxy.getUsersFlatByUnitId(unitId).subscribe((result) => {

            this.usersList = [];
            result.forEach((user) => {
                this.usersList.push({label:user.name, value:{id : user.id , name: user.name}});
            });

        });
    }

    populateDays() :void{
        this.days = [];
        for (let index = 0; index < 7; index++) {
            let dayToPush = new ShiftDay();
            switch (index) {
                case 0:
                    dayToPush.header = "Satarday";
                    dayToPush.shiftTypeId = 1;
                    break;
                    case 1:
                    dayToPush.header = "Sunday";
                    dayToPush.shiftTypeId = 1;
                    break;
                    case 2:
                    dayToPush.header = "Monday";
                    dayToPush.shiftTypeId = 1;
                    break;
                    case 3:
                    dayToPush.header = "Tuesday";
                    dayToPush.shiftTypeId = 1;
                    break;
                    case 4:
                    dayToPush.header = "Wednesday";
                    dayToPush.shiftTypeId = 1;
                    break;
                    case 5:
                    dayToPush.header = "Thursday";
                    dayToPush.shiftTypeId = 1;
                    break;
                    case 6:
                    dayToPush.header = "Friday";
                    dayToPush.shiftTypeId = 1;
                    break;
                default:
                    break;
            }

            dayToPush.selectedShifts =[];
            if(this.shiftypesDropdown.length > 0)
                dayToPush.shiftTypeId = this.shiftypesDropdown[0].value;
            else
            dayToPush.shiftTypeId = 1;

            this.days.push(dayToPush);

        }
        this.active = true;


    }

    applyToAllDays(selectedDay): void{

        this.days.forEach(day => {
            day.shiftTypeId = selectedDay.shiftTypeId;
            day.selectedShifts = selectedDay.selectedShifts;
        });

        this.notify.info(this.l('ApplyedToAll'));
    }

    show(timeProfileId?: number): void {

        if (!timeProfileId) {
            this.timeProfile = new CreateOrEditTimeProfileDto();
            this.timeProfile.id = timeProfileId;
            this.timeProfile.startDate = moment().startOf('day');
            this.timeProfile.endDate = moment().startOf('day');
            this.userName = '';

            this.populateDays();
            // this.active = true;
            //this.modal.show();
        } else {
            this._timeProfilesServiceProxy.getTimeProfileForEdit(timeProfileId).subscribe(result => {
                this.timeProfile = result.timeProfile;

                this.userName = result.userName;


              //  this.modal.show();
              this.populateDays();
            });
        }
    }

    save(timeProfileForm : NgForm): void {
        debugger
        this.modelToSave = [];
        if(timeProfileForm.form.valid){
            this.saving = true;
            //generate profile model
            this.selectedUsers.forEach(user => {
                let timeProfileToAdd = new CreateOrEditTimeProfileDto();
                timeProfileToAdd.timeProfileDetails = [];
                timeProfileToAdd.userId = user.id;
                timeProfileToAdd.startDate = this.timeProfile.startDate;
                timeProfileToAdd.endDate = this.timeProfile.endDate;

        for (let index = 0; index < this.days.length; index++) {
            switch (index) {
                case 0:
                    timeProfileToAdd.shiftTypeID_Saturday = this.days[index].shiftTypeId;
                    if(this.days[index].selectedShifts.length > 0){
                        this.days[index].selectedShifts.forEach((shift:ShiftDto) => {
                            let profileDetailToAdd = new TimeProfileDetailDto();
                                profileDetailToAdd.shiftId = shift['shift'].id;
                                profileDetailToAdd.day = index + 1;
                                timeProfileToAdd.timeProfileDetails.push(profileDetailToAdd);
                            });
                    }
                    break;

            case 1:
                timeProfileToAdd.shiftTypeID_Sunday = this.days[index].shiftTypeId;
                if(this.days[index].selectedShifts.length > 0){
                    this.days[index].selectedShifts.forEach((shift:ShiftDto) => {
                        let profileDetailToAdd = new TimeProfileDetailDto();
                            profileDetailToAdd.shiftId = shift['shift'].id;
                            profileDetailToAdd.day = index + 1;
                            timeProfileToAdd.timeProfileDetails.push(profileDetailToAdd);
                        });
                }
                break;
            case 2:
                timeProfileToAdd.shiftTypeID_Monday = this.days[index].shiftTypeId;
                if(this.days[index].selectedShifts.length > 0){
                    this.days[index].selectedShifts.forEach((shift:ShiftDto) => {
                        let profileDetailToAdd = new TimeProfileDetailDto();
                            profileDetailToAdd.shiftId = shift['shift'].id;
                            profileDetailToAdd.day = index + 1;
                            timeProfileToAdd.timeProfileDetails.push(profileDetailToAdd);
                        });
                }
                break;
            case 3:
                timeProfileToAdd.shiftTypeID_Tuesday = this.days[index].shiftTypeId;
                if(this.days[index].selectedShifts.length > 0){
                    this.days[index].selectedShifts.forEach((shift:ShiftDto) => {
                        let profileDetailToAdd = new TimeProfileDetailDto();
                            profileDetailToAdd.shiftId = shift['shift'].id;
                            profileDetailToAdd.day = index + 1;
                            timeProfileToAdd.timeProfileDetails.push(profileDetailToAdd);
                        });
                }
                break;
            case 4:
                timeProfileToAdd.shiftTypeID_Wednesday = this.days[index].shiftTypeId;
                if(this.days[index].selectedShifts.length > 0){
                    this.days[index].selectedShifts.forEach((shift:ShiftDto) => {
                        let profileDetailToAdd = new TimeProfileDetailDto();
                            profileDetailToAdd.shiftId = shift['shift'].id;
                            profileDetailToAdd.day = index + 1;
                            timeProfileToAdd.timeProfileDetails.push(profileDetailToAdd);
                        });
                }
                break;
                case 5:
                    timeProfileToAdd.shiftTypeID_Thursday = this.days[index].shiftTypeId;
                    if(this.days[index].selectedShifts.length > 0){
                        this.days[index].selectedShifts.forEach((shift:ShiftDto) => {
                            let profileDetailToAdd = new TimeProfileDetailDto();
                                profileDetailToAdd.shiftId = shift['shift'].id;
                                profileDetailToAdd.day = index + 1;
                                timeProfileToAdd.timeProfileDetails.push(profileDetailToAdd);
                            });
                    }
                    break;
                    case 6:
                        timeProfileToAdd.shiftTypeID_Friday = this.days[index].shiftTypeId;
                        if(this.days[index].selectedShifts.length > 0){
                            this.days[index].selectedShifts.forEach((shift:ShiftDto) => {
                                let profileDetailToAdd = new TimeProfileDetailDto();
                                    profileDetailToAdd.shiftId = shift['shift'].id;
                                    profileDetailToAdd.day = index + 1;
                                    timeProfileToAdd.timeProfileDetails.push(profileDetailToAdd);
                                });
                        }
                        break;
                    default:
                        break;
                }

              }

              this.modelToSave.push(timeProfileToAdd);
            });

            this.saving = false;
            console.log(this.modelToSave);
            if(this.usersList.length == 0)
                this.notify.error(this.l('PleaseSelceUsers'));
            else {
                this._timeProfilesServiceProxy.createOrEdit(this.modelToSave)
                .pipe(finalize(() => { this.saving = false;}))
                .subscribe(() => {
                   this.notify.info(this.l('SavedSuccessfully'));
                   this.resetData();
                });
            }
        }



    }

    openSelectUserModal() {
        this.timeProfileUserLookupTableModal.id = this.timeProfile.userId;
        this.timeProfileUserLookupTableModal.displayName = this.userName;
        this.timeProfileUserLookupTableModal.show();
    }

    resetData(){
        this.timeProfile.id=null;
        this.timeProfile.startDate = moment().startOf('day');
        this.timeProfile.endDate = moment().startOf('day');
        this.selectedUsers = [];
        this.initTimeProfile();

    }


    setUserIdNull() {
        this.timeProfile.userId = null;
        this.userName = '';
    }


    getNewUserId() {
        this.timeProfile.userId = this.timeProfileUserLookupTableModal.id;
        this.userName = this.timeProfileUserLookupTableModal.displayName;
    }

    openUnitModal(){
        this.organizationUnitsHorizontalTreeModal.show();
    }


    ouSelected(event: any): void {

        console.log('in manage time component');

        this.getUsers(event.id);
        //console.log(event);
    }


    close(): void {
        // this.active = false;
        // this.router.navigate(['app/main/operations/timeProfiles']);
       // this.modal.hide();
    }
}
