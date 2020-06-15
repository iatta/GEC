import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { ShiftTypeDetailsServiceProxy, CreateOrEditShiftTypeDetailDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { ShiftTypeDetailShiftTypeLookupTableModalComponent } from './shiftTypeDetail-shiftType-lookup-table-modal.component';

@Component({
    selector: 'createOrEditShiftTypeDetailModal',
    templateUrl: './create-or-edit-shiftTypeDetail-modal.component.html'
})
export class CreateOrEditShiftTypeDetailModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('shiftTypeDetailShiftTypeLookupTableModal', { static: true }) shiftTypeDetailShiftTypeLookupTableModal: ShiftTypeDetailShiftTypeLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    shiftTypeDetail: CreateOrEditShiftTypeDetailDto = new CreateOrEditShiftTypeDetailDto();

    shiftTypeDescriptionAr = '';


    constructor(
        injector: Injector,
        private _shiftTypeDetailsServiceProxy: ShiftTypeDetailsServiceProxy
    ) {
        super(injector);
    }

    show(shiftTypeDetailId?: number): void {

        if (!shiftTypeDetailId) {
            this.shiftTypeDetail = new CreateOrEditShiftTypeDetailDto();
            this.shiftTypeDetail.id = shiftTypeDetailId;
            this.shiftTypeDescriptionAr = '';

            this.active = true;
            this.modal.show();
        } else {
            this._shiftTypeDetailsServiceProxy.getShiftTypeDetailForEdit(shiftTypeDetailId).subscribe(result => {
                this.shiftTypeDetail = result.shiftTypeDetail;

                this.shiftTypeDescriptionAr = result.shiftTypeDescriptionAr;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
            this.saving = true;

			
            this._shiftTypeDetailsServiceProxy.createOrEdit(this.shiftTypeDetail)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectShiftTypeModal() {
        this.shiftTypeDetailShiftTypeLookupTableModal.id = this.shiftTypeDetail.shiftTypeId;
        this.shiftTypeDetailShiftTypeLookupTableModal.displayName = this.shiftTypeDescriptionAr;
        this.shiftTypeDetailShiftTypeLookupTableModal.show();
    }


    setShiftTypeIdNull() {
        this.shiftTypeDetail.shiftTypeId = null;
        this.shiftTypeDescriptionAr = '';
    }


    getNewShiftTypeId() {
        this.shiftTypeDetail.shiftTypeId = this.shiftTypeDetailShiftTypeLookupTableModal.id;
        this.shiftTypeDescriptionAr = this.shiftTypeDetailShiftTypeLookupTableModal.displayName;
    }


    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
