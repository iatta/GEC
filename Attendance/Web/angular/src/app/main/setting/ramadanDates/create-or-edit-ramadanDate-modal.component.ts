import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { RamadanDatesServiceProxy, CreateOrEditRamadanDateDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';

@Component({
    selector: 'createOrEditRamadanDateModal',
    templateUrl: './create-or-edit-ramadanDate-modal.component.html'
})
export class CreateOrEditRamadanDateModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    ramadanDate: CreateOrEditRamadanDateDto = new CreateOrEditRamadanDateDto();



    constructor(
        injector: Injector,
        private _ramadanDatesServiceProxy: RamadanDatesServiceProxy
    ) {
        super(injector);
    }

    show(ramadanDateId?: number): void {

        if (!ramadanDateId) {
            this.ramadanDate = new CreateOrEditRamadanDateDto();
            this.ramadanDate.id = ramadanDateId;
            this.ramadanDate.fromDate = moment().startOf('day');
            this.ramadanDate.toDate = moment().startOf('day');

            this.active = true;
            this.modal.show();
        } else {
            this._ramadanDatesServiceProxy.getRamadanDateForEdit(ramadanDateId).subscribe(result => {
                this.ramadanDate = result.ramadanDate;


                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
            this.saving = true;

			
            this._ramadanDatesServiceProxy.createOrEdit(this.ramadanDate)
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
