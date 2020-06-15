import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { TypesOfPermitsServiceProxy, CreateOrEditTypesOfPermitDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';

@Component({
    selector: 'createOrEditTypesOfPermitModal',
    templateUrl: './create-or-edit-typesOfPermit-modal.component.html'
})
export class CreateOrEditTypesOfPermitModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    typesOfPermit: CreateOrEditTypesOfPermitDto = new CreateOrEditTypesOfPermitDto();



    constructor(
        injector: Injector,
        private _typesOfPermitsServiceProxy: TypesOfPermitsServiceProxy
    ) {
        super(injector);
    }

    show(typesOfPermitId?: number): void {

        if (!typesOfPermitId) {
            this.typesOfPermit = new CreateOrEditTypesOfPermitDto();
            this.typesOfPermit.id = typesOfPermitId;

            this.active = true;
            this.modal.show();
        } else {
            this._typesOfPermitsServiceProxy.getTypesOfPermitForEdit(typesOfPermitId).subscribe(result => {
                this.typesOfPermit = result.typesOfPermit;


                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
            this.saving = true;

			
            this._typesOfPermitsServiceProxy.createOrEdit(this.typesOfPermit)
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
