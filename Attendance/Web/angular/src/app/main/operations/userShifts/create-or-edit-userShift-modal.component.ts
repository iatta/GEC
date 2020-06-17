import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { UserShiftsServiceProxy, CreateOrEditUserShiftDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { UserShiftUserLookupTableModalComponent } from './userShift-user-lookup-table-modal.component';
import { UserShiftShiftLookupTableModalComponent } from './userShift-shift-lookup-table-modal.component';

@Component({
    selector: 'createOrEditUserShiftModal',
    templateUrl: './create-or-edit-userShift-modal.component.html'
})
export class CreateOrEditUserShiftModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('userShiftUserLookupTableModal', { static: true }) userShiftUserLookupTableModal: UserShiftUserLookupTableModalComponent;
    @ViewChild('userShiftShiftLookupTableModal', { static: true }) userShiftShiftLookupTableModal: UserShiftShiftLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    userShift: CreateOrEditUserShiftDto = new CreateOrEditUserShiftDto();

    userName = '';
    shiftNameEn = '';


    constructor(
        injector: Injector,
        private _userShiftsServiceProxy: UserShiftsServiceProxy
    ) {
        super(injector);
    }

    show(userShiftId?: number): void {

        if (!userShiftId) {
            this.userShift = new CreateOrEditUserShiftDto();
            this.userShift.id = userShiftId;
            this.userShift.date = moment().startOf('day');
            this.userName = '';
            this.shiftNameEn = '';

            this.active = true;
            this.modal.show();
        } else {
            this._userShiftsServiceProxy.getUserShiftForEdit(userShiftId).subscribe(result => {
                this.userShift = result.userShift;

                this.userName = result.userName;
                this.shiftNameEn = result.shiftNameEn;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
            this.saving = true;

			
            this._userShiftsServiceProxy.createOrEdit(this.userShift)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectUserModal() {
        this.userShiftUserLookupTableModal.id = this.userShift.userId;
        this.userShiftUserLookupTableModal.displayName = this.userName;
        this.userShiftUserLookupTableModal.show();
    }
    openSelectShiftModal() {
        // this.userShiftShiftLookupTableModal.id = this.userShift.shiftId;
        // this.userShiftShiftLookupTableModal.displayName = this.shiftNameEn;
        // this.userShiftShiftLookupTableModal.show();
    }


    setUserIdNull() {
        this.userShift.userId = null;
        this.userName = '';
    }
    setShiftIdNull() {
        this.userShift.shiftId = null;
        this.shiftNameEn = '';
    }


    getNewUserId() {
        this.userShift.userId = this.userShiftUserLookupTableModal.id;
        this.userName = this.userShiftUserLookupTableModal.displayName;
    }
    getNewShiftId() {
        // this.userShift.shiftId = this.userShiftShiftLookupTableModal.id;
        // this.shiftNameEn = this.userShiftShiftLookupTableModal.displayName;
    }


    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
