import { NgForm } from '@angular/forms';
import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { HolidaysServiceProxy, CreateOrEditHolidayDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';

@Component({
    selector: 'createOrEditHolidayModal',
    templateUrl: './create-or-edit-holiday-modal.component.html'
})
export class CreateOrEditHolidayModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    holiday: CreateOrEditHolidayDto = new CreateOrEditHolidayDto();



    constructor(
        injector: Injector,
        private _holidaysServiceProxy: HolidaysServiceProxy
    ) {
        super(injector);
    }

    show(holidayId?: number): void {

        if (!holidayId) {
            this.holiday = new CreateOrEditHolidayDto();
            this.holiday.id = holidayId;
            this.holiday.startDate = moment().startOf('day');
            this.holiday.endDate = moment().startOf('day');

            this.active = true;
            this.modal.show();
        } else {
            this._holidaysServiceProxy.getHolidayForEdit(holidayId).subscribe(result => {
                console.log(result.holiday);
                this.holiday = result.holiday;


                this.active = true;
                this.modal.show();
            });
        }
    }

    save(holidayForm : NgForm): void {
        if(holidayForm.form.valid){
            this.saving = true;


            this._holidaysServiceProxy.createOrEdit(this.holiday)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
        }

    }







    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
