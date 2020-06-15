import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { ShiftTypesServiceProxy, CreateOrEditShiftTypeDto, ShiftTypeDetailDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';


@Component({
    selector: 'createOrEditShiftTypeModal',
    templateUrl: './create-or-edit-shiftType-modal.component.html'
})
export class CreateOrEditShiftTypeModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    shiftType: CreateOrEditShiftTypeDto = new CreateOrEditShiftTypeDto();
    shiftTypeDetailToAdd: ShiftTypeDetailDto;
    TimeObj: Date = new Date();
    meridian = true;

    constructor(
        injector: Injector,
        private _shiftTypesServiceProxy: ShiftTypesServiceProxy
    ) {
        super(injector);
    }

    show(shiftTypeId?: number): void {

        if (!shiftTypeId) {
            this.shiftType = new CreateOrEditShiftTypeDto();
            this.shiftType.id = shiftTypeId;
            this.shiftType.shiftTypeDetails = [];
            this.active = true;
            this.modal.show();
        } else {
            this._shiftTypesServiceProxy.getShiftTypeForEdit(shiftTypeId).subscribe(result => {
                this.shiftType = result.shiftType;
                if(this.shiftType.shiftTypeDetails.length > 0){
                    this.shiftType.shiftTypeDetails.forEach(function (value) {
                        if (value.inTimeFirstScan) {
                            value.selectedInTime = 'inTimeFirstScan';
                        } else {
                            value.selectedInTime = 'inTimeLastScan';
                        }
                        if (value.outTimeFirstScan) {
                            value.selectedOutTime = 'outTimeFirstScan';
                        } else {
                            value.selectedOutTime = 'outTimeLastScan';
                        }
                      });
                }

                this.TimeObj.setHours(Math.floor(this.shiftType.maxBoundryTime / 60));
                this.TimeObj.setMinutes(this.shiftType.maxBoundryTime % 60) ;

                this.active = true;
                this.modal.show();
            });
        }
    }

    addDuties() : void{
        this.shiftTypeDetailToAdd = new ShiftTypeDetailDto();
        this.shiftTypeDetailToAdd.selectedOutTime = 'outTimeLastScan';
        this.shiftTypeDetailToAdd.selectedInTime = 'inTimeFirstScan';

        this.shiftType.shiftTypeDetails.push(this.shiftTypeDetailToAdd);
    }
    deleteDutie(shiftTypeDetail){
        let index = this.shiftType.shiftTypeDetails.indexOf(shiftTypeDetail);
        if(index != -1)
            this.shiftType.shiftTypeDetails.splice(index , 1);
    }

    save(): void {

        this.shiftType.maxBoundryTime = (this.TimeObj.getHours() * 60) + this.TimeObj.getMinutes();

        if(this.shiftType.shiftTypeDetails.length > 0){
            this.shiftType.shiftTypeDetails.forEach(function (value) {
                if(value.selectedInTime === "inTimeFirstScan"){
                    value.inTimeFirstScan = true;
                    value.inTimeLastScan = false;
                }else{
                    value.inTimeFirstScan = false;
                    value.inTimeLastScan = true;
                }

                if(value.selectedOutTime === "outTimeFirstScan"){
                    value.outTimeFirstScan = true;
                    value.outTimeLastScan = false;
                }else{
                    value.outTimeFirstScan = false;
                    value.outTimeLastScan = true;
                }

              });
        }

            this.saving = true;

            this._shiftTypesServiceProxy.createOrEdit(this.shiftType)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }







    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
