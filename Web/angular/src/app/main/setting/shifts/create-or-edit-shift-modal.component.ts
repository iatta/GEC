import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { ShiftsServiceProxy, CreateOrEditShiftDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';


@Component({
    selector: 'createOrEditShiftModal',
    templateUrl: './create-or-edit-shift-modal.component.html'
})
export class CreateOrEditShiftModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    shift: CreateOrEditShiftDto = new CreateOrEditShiftDto();

    meridian = true;

    TimeInObj  = {hour: 13, minute: 30};
    TimeOutObj  = {hour: 13, minute: 30};
    EarlyInObj  = {hour: 13, minute: 30};
    LateInObj  = {hour: 13, minute: 30};
    EarlyOutObj  = {hour: 13, minute: 30};
    LateOutObj  = {hour: 13, minute: 30};
    TimeInRangeFromObj  = {hour: 13, minute: 30};
    TimeInRangeToObj  = {hour: 13, minute: 30};
    TimeOutRangeFromObj  = {hour: 13, minute: 30};
    TimeOutRangeToObj  = {hour: 13, minute: 30};


    constructor(
        injector: Injector,
        private _shiftsServiceProxy: ShiftsServiceProxy
    ) {
        super(injector);
    }

    toggleMeridian() {
        this.meridian = !this.meridian;
    }

    show(shiftId?: number): void {

        if (!shiftId) {
            this.shift = new CreateOrEditShiftDto();
            this.shift.id = shiftId;

            this.active = true;
            this.modal.show();
        } else {
            this._shiftsServiceProxy.getShiftForEdit(shiftId).subscribe(result => {
                this.shift = result.shift;

                this.TimeInObj.hour = Math.floor(this.shift.timeIn / 60);
                this.TimeInObj.minute = this.shift.timeIn % 60 ;

                this.TimeOutObj.hour = Math.floor(this.shift.timeOut / 60);
                this.TimeOutObj.minute = this.shift.timeIn % 60;


                this.EarlyInObj.hour = Math.floor(this.shift.earlyIn / 60);
                this.EarlyInObj.minute = this.shift.timeIn % 60;

                this.LateInObj.hour = Math.floor(this.shift.lateIn / 60);
                this.LateInObj.minute = this.shift.timeIn % 60;

                this.EarlyOutObj.hour = Math.floor(this.shift.earlyOut / 60);
                this.EarlyOutObj.minute = this.shift.timeIn % 60;


                this.LateOutObj.hour = Math.floor(this.shift.lateOut / 60);
                this.LateOutObj.minute = this.shift.timeIn % 60;


                this.TimeInRangeFromObj.hour = Math.floor(this.shift.timeInRangeFrom / 60);
                this.TimeInRangeFromObj.minute = this.shift.timeIn % 60;

                this.TimeInRangeToObj.hour = Math.floor(this.shift.timeInRangeTo / 60);
                this.TimeInRangeToObj.minute = this.shift.timeIn % 60;

                this.TimeOutRangeFromObj.hour = Math.floor(this.shift.timeOutRangeFrom / 60);
                this.TimeOutRangeFromObj.minute =this.shift.timeIn % 60;

                this.TimeOutRangeToObj.hour = Math.floor(this.shift.timeOutRangeTo / 60);
                this.TimeOutRangeToObj.minute = this.shift.timeIn % 60;


                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
            this.saving = true;
            this.shift.timeIn = (this.TimeInObj.hour * 60) + this.TimeInObj.minute;
            this.shift.timeOut = (this.TimeOutObj.hour * 60) + this.TimeInObj.minute;
            this.shift.earlyIn = (this.EarlyInObj.hour * 60) + this.TimeInObj.minute;
            this.shift.lateIn = (this.LateInObj.hour * 60) + this.TimeInObj.minute;
            this.shift.earlyOut = (this.EarlyOutObj.hour * 60) + this.TimeInObj.minute;
            this.shift.lateOut = (this.LateOutObj.hour * 60) + this.TimeInObj.minute;

            this.shift.timeInRangeFrom = (this.TimeInRangeFromObj.hour * 60) + this.TimeInObj.minute;
            this.shift.timeInRangeTo = (this.TimeInRangeToObj.hour * 60) + this.TimeInObj.minute;
            this.shift.timeOutRangeFrom = (this.TimeOutRangeFromObj.hour * 60) + this.TimeInObj.minute;
            this.shift.timeOutRangeTo = (this.TimeOutRangeToObj.hour * 60) + this.TimeInObj.minute;


            this._shiftsServiceProxy.createOrEdit(this.shift)
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
