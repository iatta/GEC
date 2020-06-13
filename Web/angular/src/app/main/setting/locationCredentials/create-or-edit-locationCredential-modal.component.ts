import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { LocationCredentialsServiceProxy, CreateOrEditLocationCredentialDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { LocationCredentialLocationLookupTableModalComponent } from './locationCredential-location-lookup-table-modal.component';

@Component({
    selector: 'createOrEditLocationCredentialModal',
    templateUrl: './create-or-edit-locationCredential-modal.component.html'
})
export class CreateOrEditLocationCredentialModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('locationCredentialLocationLookupTableModal', { static: true }) locationCredentialLocationLookupTableModal: LocationCredentialLocationLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    locationCredential: CreateOrEditLocationCredentialDto = new CreateOrEditLocationCredentialDto();

    locationDescriptionAr = '';


    constructor(
        injector: Injector,
        private _locationCredentialsServiceProxy: LocationCredentialsServiceProxy
    ) {
        super(injector);
    }

    show(locationCredentialId?: number): void {

        if (!locationCredentialId) {
            this.locationCredential = new CreateOrEditLocationCredentialDto();
            this.locationCredential.id = locationCredentialId;
            this.locationDescriptionAr = '';

            this.active = true;
            this.modal.show();
        } else {
            this._locationCredentialsServiceProxy.getLocationCredentialForEdit(locationCredentialId).subscribe(result => {
                this.locationCredential = result.locationCredential;

                this.locationDescriptionAr = result.locationDescriptionAr;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
            this.saving = true;

			
            this._locationCredentialsServiceProxy.createOrEdit(this.locationCredential)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectLocationModal() {
        this.locationCredentialLocationLookupTableModal.id = this.locationCredential.locationId;
        this.locationCredentialLocationLookupTableModal.displayName = this.locationDescriptionAr;
        this.locationCredentialLocationLookupTableModal.show();
    }


    setLocationIdNull() {
        this.locationCredential.locationId = null;
        this.locationDescriptionAr = '';
    }


    getNewLocationId() {
        this.locationCredential.locationId = this.locationCredentialLocationLookupTableModal.id;
        this.locationDescriptionAr = this.locationCredentialLocationLookupTableModal.displayName;
    }


    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
