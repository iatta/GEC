import { Component, ViewChild, Injector, Output, EventEmitter, OnInit } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { TimeProfilesServiceProxy, ShiftTypesServiceProxy, ShiftsServiceProxy,ShiftDto , TimeProfileDetailDto,  CreateOrEditTimeProfileDto, UserServiceProxy ,UserListDto , GetShiftTypeForViewDto , GetShiftForViewDto} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { TimeProfileUserLookupTableModalComponent } from './timeProfile-user-lookup-table-modal.component';
import {SelectItem} from 'primeng/api';


@Component({
    selector: 'createOrEditTimeProfileModal',
    templateUrl: './create-or-edit-timeProfile-modal.component.html'
})
export class CreateOrEditTimeProfileModalComponent extends AppComponentBase implements OnInit {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('timeProfileUserLookupTableModal', { static: true }) timeProfileUserLookupTableModal: TimeProfileUserLookupTableModalComponent;

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


    constructor(
        injector: Injector,
        private _timeProfilesServiceProxy: TimeProfilesServiceProxy,
        private _userServiceProxy: UserServiceProxy,
        private _shiftsServiceProxy: ShiftsServiceProxy,
        private _shiftTypesServiceProxy: ShiftTypesServiceProxy
    ) {
        super(injector);
    }
    ngOnInit(): void {
        this._userServiceProxy.getUsersFlat().subscribe((result) => {
            console.log(result['items']);
          
            result['items'].forEach((user)=>{
                this.usersList.push({label:user.name, value:{id : user.id , name: user.name}});
            });
            
        });
        
        this._shiftsServiceProxy.getAllFlat().subscribe((result)=>{
            this.shiftList = result['items'];
            console.log(this.shiftList );
        });

        this._shiftTypesServiceProxy.getAllFlat().subscribe((result)=>{
            this.shiftTypesList = result['items'];
            result['items'].forEach((shiftType)=>{
                this.shiftypesDropdown.push({label:shiftType.shiftType.descriptionAr, value: shiftType.shiftType.id });
            });
        });
    }
    applyToAllDays(selectedVal): void{
        this.timeProfile.shiftTypeID_Saturday = selectedVal;
        this.timeProfile.shiftTypeID_Sunday = selectedVal;
        this.timeProfile.shiftTypeID_Monday = selectedVal;
        this.timeProfile.shiftTypeID_Tuesday = selectedVal;
        this.timeProfile.shiftTypeID_Wednesday = selectedVal;
        this.timeProfile.shiftTypeID_Thursday = selectedVal;
        this.timeProfile.shiftTypeID_Friday = selectedVal;
    }
    show(timeProfileId?: number): void {

        if (!timeProfileId) {
            this.timeProfile = new CreateOrEditTimeProfileDto();
            this.timeProfile.id = timeProfileId;
            this.timeProfile.startDate = moment().startOf('day');
            this.timeProfile.endDate = moment().startOf('day');
            this.userName = '';

            this.timeProfile.shiftTypeID_Saturday = 1;
            this.timeProfile.shiftTypeID_Sunday = 1;
            this.timeProfile.shiftTypeID_Monday = 1;
            this.timeProfile.shiftTypeID_Tuesday = 1;
            this.timeProfile.shiftTypeID_Wednesday = 1;
            this.timeProfile.shiftTypeID_Thursday = 1;
            this.timeProfile.shiftTypeID_Friday = 1;


            this.active = true;
            this.modal.show();
        } else {
            this._timeProfilesServiceProxy.getTimeProfileForEdit(timeProfileId).subscribe(result => {
                this.timeProfile = result.timeProfile;

                this.userName = result.userName;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
            this.saving = true;
            //generate profile model 
            debugger
            this.modelToSave = [];
            if(this.selectedUsers.length > 0){
                this.selectedUsers.forEach(user => {
                    let timeProfileToAdd = new CreateOrEditTimeProfileDto();
                    timeProfileToAdd.userId = user.id;
                    timeProfileToAdd.shiftTypeID_Saturday = this.timeProfile.shiftTypeID_Saturday;
                    timeProfileToAdd.shiftTypeID_Sunday = this.timeProfile.shiftTypeID_Sunday;
                    timeProfileToAdd.shiftTypeID_Monday = this.timeProfile.shiftTypeID_Monday;
                    timeProfileToAdd.shiftTypeID_Tuesday = this.timeProfile.shiftTypeID_Tuesday;
                    timeProfileToAdd.shiftTypeID_Wednesday = this.timeProfile.shiftTypeID_Wednesday;
                    timeProfileToAdd.shiftTypeID_Thursday = this.timeProfile.shiftTypeID_Thursday;
                    timeProfileToAdd.shiftTypeID_Friday = this.timeProfile.shiftTypeID_Friday;

                    
                    if(this.selectedShifts.length > 0) {
                        timeProfileToAdd.timeProfileDetails = [];
                        this.selectedShifts.forEach(shift => {
                            let profileDetailToAdd = new TimeProfileDetailDto();
                            profileDetailToAdd.shiftId = shift['shift'].id;   
                            timeProfileToAdd.timeProfileDetails.push(profileDetailToAdd);
                        });
                    }

                    this.modelToSave.push(timeProfileToAdd);
                });
                
                

              
            }
            this.saving = false;
			console.log(this.modelToSave);
            this._timeProfilesServiceProxy.createOrEdit(this.modelToSave)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectUserModal() {
        this.timeProfileUserLookupTableModal.id = this.timeProfile.userId;
        this.timeProfileUserLookupTableModal.displayName = this.userName;
        this.timeProfileUserLookupTableModal.show();
    }


    setUserIdNull() {
        this.timeProfile.userId = null;
        this.userName = '';
    }


    getNewUserId() {
        this.timeProfile.userId = this.timeProfileUserLookupTableModal.id;
        this.userName = this.timeProfileUserLookupTableModal.displayName;
    }


    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
