import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { TimeProfileDetailsServiceProxy, CreateOrEditTimeProfileDetailDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { TimeProfileDetailTimeProfileLookupTableModalComponent } from './timeProfileDetail-timeProfile-lookup-table-modal.component';
import { TimeProfileDetailShiftLookupTableModalComponent } from './timeProfileDetail-shift-lookup-table-modal.component';

@Component({
    selector: 'createOrEditTimeProfileDetailModal',
    templateUrl: './create-or-edit-timeProfileDetail-modal.component.html'
})
export class CreateOrEditTimeProfileDetailModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('timeProfileDetailTimeProfileLookupTableModal', { static: true }) timeProfileDetailTimeProfileLookupTableModal: TimeProfileDetailTimeProfileLookupTableModalComponent;
    @ViewChild('timeProfileDetailShiftLookupTableModal', { static: true }) timeProfileDetailShiftLookupTableModal: TimeProfileDetailShiftLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    timeProfileDetail: CreateOrEditTimeProfileDetailDto = new CreateOrEditTimeProfileDetailDto();

    timeProfileDescriptionAr = '';
    shiftNameAr = '';


    constructor(
        injector: Injector,
        private _timeProfileDetailsServiceProxy: TimeProfileDetailsServiceProxy
    ) {
        super(injector);
    }

    show(timeProfileDetailId?: number): void {

        if (!timeProfileDetailId) {
            this.timeProfileDetail = new CreateOrEditTimeProfileDetailDto();
            this.timeProfileDetail.id = timeProfileDetailId;
            this.timeProfileDescriptionAr = '';
            this.shiftNameAr = '';

            this.active = true;
            this.modal.show();
        } else {
            this._timeProfileDetailsServiceProxy.getTimeProfileDetailForEdit(timeProfileDetailId).subscribe(result => {
                this.timeProfileDetail = result.timeProfileDetail;

                this.timeProfileDescriptionAr = result.timeProfileDescriptionAr;
                this.shiftNameAr = result.shiftNameAr;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
            this.saving = true;

			
            this._timeProfileDetailsServiceProxy.createOrEdit(this.timeProfileDetail)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectTimeProfileModal() {
        this.timeProfileDetailTimeProfileLookupTableModal.id = this.timeProfileDetail.timeProfileId;
        this.timeProfileDetailTimeProfileLookupTableModal.displayName = this.timeProfileDescriptionAr;
        this.timeProfileDetailTimeProfileLookupTableModal.show();
    }
    openSelectShiftModal() {
        this.timeProfileDetailShiftLookupTableModal.id = this.timeProfileDetail.shiftId;
        this.timeProfileDetailShiftLookupTableModal.displayName = this.shiftNameAr;
        this.timeProfileDetailShiftLookupTableModal.show();
    }


    setTimeProfileIdNull() {
        this.timeProfileDetail.timeProfileId = null;
        this.timeProfileDescriptionAr = '';
    }
    setShiftIdNull() {
        this.timeProfileDetail.shiftId = null;
        this.shiftNameAr = '';
    }


    getNewTimeProfileId() {
        this.timeProfileDetail.timeProfileId = this.timeProfileDetailTimeProfileLookupTableModal.id;
        this.timeProfileDescriptionAr = this.timeProfileDetailTimeProfileLookupTableModal.displayName;
    }
    getNewShiftId() {
        this.timeProfileDetail.shiftId = this.timeProfileDetailShiftLookupTableModal.id;
        this.shiftNameAr = this.timeProfileDetailShiftLookupTableModal.displayName;
    }


    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
