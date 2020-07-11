import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { BeaconsServiceProxy, CreateOrEditBeaconDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';

@Component({
    selector: 'createOrEditBeaconModal',
    templateUrl: './create-or-edit-beacon-modal.component.html'
})
export class CreateOrEditBeaconModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    beacon: CreateOrEditBeaconDto = new CreateOrEditBeaconDto();



    constructor(
        injector: Injector,
        private _beaconsServiceProxy: BeaconsServiceProxy
    ) {
        super(injector);
    }

    show(beaconId?: number): void {

        if (!beaconId) {
            this.beacon = new CreateOrEditBeaconDto();
            this.beacon.id = beaconId;

            this.active = true;
            this.modal.show();
        } else {
            this._beaconsServiceProxy.getBeaconForEdit(beaconId).subscribe(result => {
                this.beacon = result.beacon;


                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
            this.saving = true;

			
            this._beaconsServiceProxy.createOrEdit(this.beacon)
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
