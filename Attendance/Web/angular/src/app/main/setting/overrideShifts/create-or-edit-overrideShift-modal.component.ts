import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { OverrideShiftsServiceProxy, CreateOrEditOverrideShiftDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { OverrideShiftUserLookupTableModalComponent } from './overrideShift-user-lookup-table-modal.component';
import { OverrideShiftShiftLookupTableModalComponent } from './overrideShift-shift-lookup-table-modal.component';

@Component({
    selector: 'createOrEditOverrideShiftModal',
    templateUrl: './create-or-edit-overrideShift-modal.component.html'
})
export class CreateOrEditOverrideShiftModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('overrideShiftUserLookupTableModal', { static: true }) overrideShiftUserLookupTableModal: OverrideShiftUserLookupTableModalComponent;
    @ViewChild('overrideShiftShiftLookupTableModal', { static: true }) overrideShiftShiftLookupTableModal: OverrideShiftShiftLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    overrideShift: CreateOrEditOverrideShiftDto = new CreateOrEditOverrideShiftDto();

    userName = '';
    shiftNameEn = '';


    constructor(
        injector: Injector,
        private _overrideShiftsServiceProxy: OverrideShiftsServiceProxy
    ) {
        super(injector);
    }

    show(overrideShiftId?: number): void {

        if (!overrideShiftId) {
            this.overrideShift = new CreateOrEditOverrideShiftDto();
            this.overrideShift.id = overrideShiftId;
            this.overrideShift.day = moment().startOf('day');
            this.userName = '';
            this.shiftNameEn = '';

            this.active = true;
            this.modal.show();
        } else {
            this._overrideShiftsServiceProxy.getOverrideShiftForEdit(overrideShiftId).subscribe(result => {
                this.overrideShift = result.overrideShift;

                this.userName = result.userName;
                this.shiftNameEn = result.shiftNameEn;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
            this.saving = true;

			
            this._overrideShiftsServiceProxy.createOrEdit(this.overrideShift)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectUserModal() {
        this.overrideShiftUserLookupTableModal.id = this.overrideShift.userId;
        this.overrideShiftUserLookupTableModal.displayName = this.userName;
        this.overrideShiftUserLookupTableModal.show();
    }
    openSelectShiftModal() {
        this.overrideShiftShiftLookupTableModal.id = this.overrideShift.shiftId;
        this.overrideShiftShiftLookupTableModal.displayName = this.shiftNameEn;
        this.overrideShiftShiftLookupTableModal.show();
    }


    setUserIdNull() {
        this.overrideShift.userId = null;
        this.userName = '';
    }
    setShiftIdNull() {
        this.overrideShift.shiftId = null;
        this.shiftNameEn = '';
    }


    getNewUserId() {
        this.overrideShift.userId = this.overrideShiftUserLookupTableModal.id;
        this.userName = this.overrideShiftUserLookupTableModal.displayName;
    }
    getNewShiftId() {
        this.overrideShift.shiftId = this.overrideShiftShiftLookupTableModal.id;
        this.shiftNameEn = this.overrideShiftShiftLookupTableModal.displayName;
    }


    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
