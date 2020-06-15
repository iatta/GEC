import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { WarningTypesServiceProxy, CreateOrEditWarningTypeDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';

@Component({
    selector: 'createOrEditWarningTypeModal',
    templateUrl: './create-or-edit-warningType-modal.component.html'
})
export class CreateOrEditWarningTypeModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    warningType: CreateOrEditWarningTypeDto = new CreateOrEditWarningTypeDto();



    constructor(
        injector: Injector,
        private _warningTypesServiceProxy: WarningTypesServiceProxy
    ) {
        super(injector);
    }

    show(warningTypeId?: number): void {

        if (!warningTypeId) {
            this.warningType = new CreateOrEditWarningTypeDto();
            this.warningType.id = warningTypeId;

            this.active = true;
            this.modal.show();
        } else {
            this._warningTypesServiceProxy.getWarningTypeForEdit(warningTypeId).subscribe(result => {
                this.warningType = result.warningType;


                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
            this.saving = true;

			
            this._warningTypesServiceProxy.createOrEdit(this.warningType)
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
