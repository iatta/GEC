import { Table } from 'primeng/table';
import { UserLocationDto, AssignedLocationDto, TimeProfileDetailDto } from './../../../shared/service-proxies/service-proxies';
import { NgForm } from '@angular/forms';
import { OrganizationUnitsHorizontalTreeModalUserComponent } from './../shared/organization-horizontal-tree-modal-user.component';
import { OrganizationUnitsHorizontalTreeModalComponent } from './../shared/organization-horizontal-tree-modal.component';
import { SelectItem, FilterUtils } from 'primeng/api';
import { OrganizationUnitsHorizontalTreeComponent } from './../shared/organization-horizontal-tree.component';
import { AfterViewChecked, Component, ElementRef, EventEmitter, Injector, Output, ViewChild, OnInit } from '@angular/core';
import { AppConsts } from '@shared/AppConsts';
import { AppComponentBase } from '@shared/common/app-component-base';
import { JobTitleDto, JobTitleServiceProxy, CreateOrUpdateUserInput, OrganizationUnitDto, PasswordComplexitySetting, ProfileServiceProxy, UserEditDto, UserRoleDto, UserServiceProxy, GetShiftForViewDto, ShiftsServiceProxy, CreateOrEditTimeProfileDto, ShiftTypesServiceProxy, GetUserShiftForViewDto, UserShiftDto } from '@shared/service-proxies/service-proxies';
import { ModalDirective, TabsetComponent } from 'ngx-bootstrap';
import { IOrganizationUnitsHierarchicalTreeComponentData, OrganizationHierarchicalTreeComponent } from '../shared/organization-hierarchical-tree.component';
import { IOrganizationUnitsTreeComponentData, OrganizationUnitsTreeComponent } from '../shared/organization-unit-tree.component';

import * as _ from 'lodash';
import { finalize } from 'rxjs/operators';
import { Router, ActivatedRoute, ParamMap ,Params } from '@angular/router';
import { switchMap } from 'rxjs/operators';
import * as moment from 'moment';
import { rowsAnimation } from '@shared/animations/template.animations';

@Component({

    templateUrl: './manage-user.component.html',
    styles: [`.user-edit-dialog-profile-image {
             margin-bottom: 20px;
        }`
    ],
    animations: [rowsAnimation],
    styleUrls:['./manage-user.css', '../../main/operations/userShifts/manager-user-shift.component.less']
})
export class ManageUserComponent extends AppComponentBase implements OnInit {
    private table: Table;
    @ViewChild('dt',{ static: false }) set content(content: Table) {
        if(content) { // initially setter gets called with undefined
            this.table = content;
        }
     }

    @ViewChild('userTabs', {static: false}) userTabs: TabsetComponent;
    //@ViewChild('organizationUnitTree', {static: false}) organizationUnitTree: OrganizationHierarchicalTreeComponent;
     @ViewChild('organizationUnitTree', {static: false}) organizationUnitTree: OrganizationUnitsTreeComponent;
     @ViewChild('organizationUnitsHorizontalTreeModal', { static: true }) organizationUnitsHorizontalTreeModal: OrganizationUnitsHorizontalTreeModalUserComponent;
//   @ViewChild('organizationUnitTree', {static: false}) organizationUnitTree: OrganizationUnitsHorizontalTreeComponent;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();


    active = false;
    saving = false;
    canChangeUserName = true;
    isTwoFactorEnabled: boolean = this.setting.getBoolean('Abp.Zero.UserManagement.TwoFactorLogin.IsEnabled');
    isLockoutEnabled: boolean = this.setting.getBoolean('Abp.Zero.UserManagement.UserLockOut.IsEnabled');
    passwordComplexitySetting: PasswordComplexitySetting = new PasswordComplexitySetting();

    user: UserEditDto = new UserEditDto();
    roles: UserRoleDto[];

    sendActivationEmail = true;
    setRandomPassword = true;
    passwordComplexityInfo = '';
    profilePicture: string;

    allOrganizationUnits: OrganizationUnitDto[];
    memberedOrganizationUnits: string[];
    memberedOrganizationUnit:string;
    userPasswordRepeat = '';
    jobTitles:  JobTitleDto[];
    userId: number;
    titles:SelectItem[] = [];
    userTypes:SelectItem[] = [];
    nationalities:SelectItem[] = [];

    test:moment.Moment;
    usersList: SelectItem[] = [];

    shiftList: GetShiftForViewDto[] = [];
    selectedShift: GetShiftForViewDto;

    beaconList: SelectItem[];
    selectedBeacon:string;

    shiftTypeList: SelectItem[] = [];
    selectedShiftId: number;

    userLoaded: boolean = false;

    locations: UserLocationDto[];
    locationsList: SelectItem[];
    locationDateFrom: moment.Moment;
    locationDateTo: moment.Moment;
    assignedLocation:AssignedLocationDto[] = [];
    selectedLocation: any;

    startDate: moment.Moment = moment();
    endDate: moment.Moment = moment();


    cols: any[];
    rows = 10;
    first = 0;

    constructor(
        injector: Injector,
        private _userService: UserServiceProxy,
        private _profileService: ProfileServiceProxy,
        private _jobTitleServiceProxy: JobTitleServiceProxy,
        private _shiftsServiceProxy: ShiftsServiceProxy,
        private _shiftTypesServiceProxy:ShiftTypesServiceProxy,
        private route: ActivatedRoute,
        private router: Router,

    ) {
        super(injector);
    }
    ngOnInit(): void {
        this.cols = [
            { field: 'locationDisplayName', header: 'Location' },
            { field: 'fromDate', header: 'From Date' },
            { field: 'toDate', header: 'To Date' }
        ];

        this.initTimeProfile();

        this.usersList.push({label:this.l('ChooseEmployee'), value:null});

        this.titles.push({label: '-', value: null},{label: 'Mr', value: 1},
        {label: 'Mrs', value:2 },{label: 'Miss', value: 3});

        this.nationalities.push({label: this.l('ChooseNationality'), value: null},{label: this.l('Kuwait'), value: 'Kuwait'});
        this.userTypes.push({label: this.l('ChooseType'), value: null},{label: this.l('Staff'), value: 1},{label: this.l('Labor'), value: 2})
        this._jobTitleServiceProxy.getAllFlat().subscribe(result => {
           this.jobTitles = result;
        });

        this.route.params.subscribe((params: Params) => {
           this.userId = params['id'];
             this.show();
        });

        FilterUtils['dateRangeFilter'] = (value, filter): boolean => {
            return value.isSame(filter);
        }
    }

    initTimeProfile() : void {
    this._shiftsServiceProxy.getAllFlat().subscribe((result)=>{
            this.shiftList = result;
        });
    }
    overTimeChanged(event:any){
        if(!event.target.checked)
            this.user.isFixedOverTimeAllowed = false;
    }

    show(): void {

        if (!this.userId) {
            this.user = new UserEditDto();
            this.user.dateOfBirth = moment().startOf('day');
            this.user.joinDate = moment().startOf('day');

            this.user.terminationDate = moment().startOf('day');
            this.locationDateTo =  moment().startOf('day');
            this.locationDateFrom =  moment().startOf('day');


            this.setRandomPassword = true;
            this.sendActivationEmail = true;
            this.active = true;
        }

        this._userService.getUserForEdit(this.userId).subscribe(userResult => {

            debugger
            this.user = userResult.user;
            console.log(this.user);
            this.userLoaded = true;
            if(!this.user.userShifts)
                this.user.userShifts = []

            this.locationDateTo =  moment().startOf('day');
            this.locationDateFrom =  moment().startOf('day');

            if(!this.user.terminationDate){
                this.user.terminationDate = moment().startOf('day');
            }

            this.roles = userResult.roles;
            this.locationsList = [];
            this.locationsList.push({label:this.l('ChooseLocation'), value:null});
            userResult.locations.forEach(location => {
                this.locationsList.push(  {label:location.locationDisplayName, value:{id:location.locationId, name: location.locationDisplayName}},);
            });

            this.beaconList = [];
            userResult.allBeacons.forEach(beacon => {
                this.beaconList.push({label:beacon.name, value:beacon.uid});
            });

            this.selectedBeacon = userResult.user.beaconUid;


            this.nationalities =[];
            this.nationalities.push({label:this.l('ChooseNationality'), value:null});
            userResult.nationalities.forEach(nationality => {
                this.nationalities.push({label:nationality.nameAr, value:nationality.id});
            })

            this.locations = userResult.locations;

            if(userResult.locations.length > 0){
                userResult.locations.forEach(location => {
                    if(location.isAssigned){
                        let userLocationToAdd = new AssignedLocationDto();
                        userLocationToAdd.fromDate = location.fromDate;
                        userLocationToAdd.toDate =  location.toDate;
                        userLocationToAdd.locationId = location.locationId;
                        userLocationToAdd.locationDisplayName = location.locationDisplayName;
                        this.assignedLocation.push(userLocationToAdd);
                    }
                });
            }
            this.canChangeUserName = this.user.userName !== AppConsts.userManagement.defaultAdminUserName;

            this.allOrganizationUnits = userResult.allOrganizationUnits;

            this.memberedOrganizationUnits = userResult.memberedOrganizationUnits;
            this.memberedOrganizationUnit = userResult.memberOrganizationUnit;

            this.getProfilePicture(userResult.profilePictureId);

            this._userService.getUsersFlat().subscribe(result =>{
                this.usersList = [];
                this.usersList.push({label:this.l('ChooseEmployee'), value:null});
                result.forEach((user) => {
                    this.usersList.push({label:user.name, value:user.id});
                });
            });

            if (this.userId) {
                this.active = true;

                setTimeout(() => {
                    this.setRandomPassword = false;
                }, 0);

                this.sendActivationEmail = false;
            }


            this._profileService.getPasswordComplexitySetting().subscribe(passwordComplexityResult => {
                this.passwordComplexitySetting = passwordComplexityResult.setting;
                this.setPasswordComplexityInfo();
                //this.modal.show();
                this.onShown();
            });
        });

    }

    addUserLocation() : void{
        //check if the location Exist
        if(this.selectedLocation){
            var index = this.assignedLocation.findIndex(x => x.fromDate.isSame(this.locationDateFrom)   && x.toDate.isSame(this.locationDateTo)  && x.locationId === this.selectedLocation.id);
            if(index > -1){
                this.message.warn(this.l('LocationSameNameWithSameDate'));
                return;
            }

            let userLocationToAdd = new AssignedLocationDto();
            userLocationToAdd.fromDate = this.locationDateFrom;
            userLocationToAdd.toDate =  this.locationDateTo;
            userLocationToAdd.locationId = this.selectedLocation.id;
            userLocationToAdd.locationDisplayName = this.selectedLocation.name;
            this.assignedLocation.push(userLocationToAdd);
            this.selectedLocation =null;
            this.locationDateFrom =  moment().startOf('day');
            this.locationDateTo =  moment().startOf('day');
        }else{
            this.message.warn(this.l('SelectLocation'));
            return;
        }



    }
    removeUserLocation(rowData : any){
        let index = this.assignedLocation.indexOf(rowData);
        if(index > -1 ){
            this.message.confirm(
                this.l('UserDeleteUserLocation'),
                this.l('AreYouSure'),
                (isConfirmed) => {
                    if (isConfirmed) {
                        this.assignedLocation.splice(index , 1);
                    }
                }
            );

        }
    }

    back(): void{
        this.router.navigate(['app/admin/users']);
    }
    setPasswordComplexityInfo(): void {

        this.passwordComplexityInfo = '<ul>';

        if (this.passwordComplexitySetting.requireDigit) {
            this.passwordComplexityInfo += '<li>' + this.l('PasswordComplexity_RequireDigit_Hint') + '</li>';
        }

        if (this.passwordComplexitySetting.requireLowercase) {
            this.passwordComplexityInfo += '<li>' + this.l('PasswordComplexity_RequireLowercase_Hint') + '</li>';
        }

        if (this.passwordComplexitySetting.requireUppercase) {
            this.passwordComplexityInfo += '<li>' + this.l('PasswordComplexity_RequireUppercase_Hint') + '</li>';
        }

        if (this.passwordComplexitySetting.requireNonAlphanumeric) {
            this.passwordComplexityInfo += '<li>' + this.l('PasswordComplexity_RequireNonAlphanumeric_Hint') + '</li>';
        }

        if (this.passwordComplexitySetting.requiredLength) {
            this.passwordComplexityInfo += '<li>' + this.l('PasswordComplexity_RequiredLength_Hint', this.passwordComplexitySetting.requiredLength) + '</li>';
        }

        this.passwordComplexityInfo += '</ul>';
    }

    getProfilePicture(profilePictureId: string): void {
        if (!profilePictureId) {
            this.profilePicture = this.appRootUrl() + 'assets/common/images/default-profile-picture.png';
        } else {
            this._profileService.getProfilePictureById(profilePictureId).subscribe(result => {

                if (result && result.profilePicture) {
                    this.profilePicture = 'data:image/jpeg;base64,' + result.profilePicture;
                } else {
                    this.profilePicture = this.appRootUrl() + 'assets/common/images/default-profile-picture.png';
                }
            });
        }
    }

    onShown(): void {

        this.organizationUnitTree.data = <IOrganizationUnitsTreeComponentData>{
            allOrganizationUnits: this.allOrganizationUnits,
            selectedOrganizationUnits: this.memberedOrganizationUnits,
            selectedOrganizationUnit: this.memberedOrganizationUnit
        };

        if(!this.user.id){
            this.user.dateOfBirth = moment().startOf('day');
            this.user.joinDate = moment().startOf('day');
            this.user.terminationDate = moment().startOf('day');
        }

        document.getElementById('Name').focus();
    }

    save(userForm : NgForm): void {
        if(!this.userLoaded){
            this._userService.getExistUSerForActive(this.user.civilId).subscribe((userResult) => {
                console.log(userResult.user)
                if(userResult.user){
                    this.message.confirm(
                        this.l('ThisEmployeeExistBefore'),
                        this.l('DoYouWantToRecoverThisEmployee'),
                        (isConfirmed) => {
                            if (isConfirmed) {
                                this.userLoaded = true;

                                this.user = userResult.user;
                                this.roles = userResult.roles;
                                this.locations = userResult.locations;
                                this.canChangeUserName = this.user.userName !== AppConsts.userManagement.defaultAdminUserName;

                                this.allOrganizationUnits = userResult.allOrganizationUnits;
                                this.memberedOrganizationUnits = userResult.memberedOrganizationUnits;
                                this.memberedOrganizationUnit = userResult.memberOrganizationUnit;

                                this.getProfilePicture(userResult.profilePictureId);
                            }else {
                                this.userLoaded = false;
                                this.okSave(userForm);
                            }
                        }
                    );
                }
            });
        } else {
            this.okSave(userForm);
        }





    }

    okSave(userForm : NgForm):void{
        if(userForm.form.valid){

            let input = new CreateOrUpdateUserInput();
            debugger
            input.user = this.user;
            input.user.beaconUid = this.selectedBeacon;
            input.user.organizationUnitId = this.organizationUnitTree.getSelectedOrganizations();
            input.setRandomPassword = this.setRandomPassword;
            input.sendActivationEmail = this.sendActivationEmail;
            input.assignedRoleNames =
                _.map(
                    _.filter(this.roles, { isAssigned: true, inheritedFromOrganizationUnit: false }), role => role.roleName
                );

            input.assignedLocations = this.assignedLocation;
            input.organizationUnitId = this.organizationUnitTree.getSelectedOrganizations();
            this.saving = true;

            input.userLoaded = this.userLoaded;
            this._userService.createOrUpdateUser(input)
                .pipe(finalize(() => { this.saving = false; }))
                .subscribe(() => {

                    this.notify.info(this.l('SavedSuccessfully'));
                    this.router.navigate(['app/admin/users']);
                    this.saving = false;
                });
        }else{
            const form = document.getElementsByClassName('userFormManage')[0];
            const invalidControl = form.querySelector('.ng-invalid');
            let tab: any = invalidControl.closest('tab');
            for (let index = 0; index <  this.userTabs.tabs.length; index++) {
                if(this.userTabs.tabs[index].elementRef.nativeElement.attributes['name'].value === tab.attributes['name'].value)
                    this.userTabs.tabs[index].active = true;
            }
        }
    }

    openUnitModal(){
        this.organizationUnitsHorizontalTreeModal.show();
    }
    ouSelected(event: any): void {
        this.getUsers(event.id);
    }

    getUsers(unitId:number){
        this._userService.getUsersFlatByUnitId(unitId).subscribe((result) => {
            this.usersList = [];
            this.usersList.push({label:this.l('ChooseEmployee'), value:null});
            result.forEach((user) => {
                this.usersList.push({label:user.name, value:user.id});
            });

        });
    }
    close(): void {
        this.active = false;
        this.userPasswordRepeat = '';
        //this.modal.hide();
    }

    getAssignedRoleCount(): number {
        return _.filter(this.roles, { isAssigned: true }).length;
    }

    onDateSelect(value) {
        let newDate = moment(moment(value , 'YYYY-MM-DD').toISOString());
        this.table.filter(newDate, 'userShift.date', 'dateRangeFilter');

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
    addShifts(): void {
        //add shifts
        if(!this.selectedShift){
            this.message.error('Please Select Shift');
            return;
        }
        for (var m = moment(this.startDate); m.diff(this.endDate, 'days') <= 0; m.add(1, 'days')) {
            let isExist = this.user.userShifts.findIndex(
                x => x.userShift.date.isSame(m, 'year') &&
                x.userShift.date.isSame(m, 'month') &&
                 x.userShift.date.isSame(m, 'day') &&
                 x.userShift.shiftId == this.selectedShift.shift.id);
            if(isExist == -1){
             let userShiftToAdd = new GetUserShiftForViewDto();
             userShiftToAdd.shiftNameEn = this.selectedShift.shift.nameEn;

             userShiftToAdd.userShift = new UserShiftDto();
             userShiftToAdd.userShift.isNew = true;
             userShiftToAdd.userShift.userId = this.user.id;
             let date =moment(m);
             userShiftToAdd.userShift.date = date;
             userShiftToAdd.userShift.shiftId = this.selectedShift.shift.id;
             this.user.userShifts.unshift(userShiftToAdd);
            }
        }

        this.table.reset();
    }

    deleteUserShift(userShiftView: GetUserShiftForViewDto):void{
        let index = this.user.userShifts.findIndex(
            x => x.userShift.date.isSame(userShiftView.userShift.date, 'year') &&
            x.userShift.date.isSame(userShiftView.userShift.date, 'month') &&
             x.userShift.date.isSame(userShiftView.userShift.date, 'day') &&
             x.userShift.shiftId == userShiftView.userShift.shiftId);

             if(index > -1)
                this.user.userShifts[index].userShift.isDeleted = true;


        this.table.reset();
    }

}
