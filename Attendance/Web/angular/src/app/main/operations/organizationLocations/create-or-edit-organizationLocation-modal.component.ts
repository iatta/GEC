import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { OrganizationLocationsServiceProxy, CreateOrEditOrganizationLocationDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { OrganizationLocationOrganizationUnitLookupTableModalComponent } from './organizationLocation-organizationUnit-lookup-table-modal.component';
import { OrganizationLocationLocationLookupTableModalComponent } from './organizationLocation-location-lookup-table-modal.component';

@Component({
    selector: 'createOrEditOrganizationLocationModal',
    templateUrl: './create-or-edit-organizationLocation-modal.component.html'
})
export class CreateOrEditOrganizationLocationModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('organizationLocationOrganizationUnitLookupTableModal', { static: true }) organizationLocationOrganizationUnitLookupTableModal: OrganizationLocationOrganizationUnitLookupTableModalComponent;
    @ViewChild('organizationLocationLocationLookupTableModal', { static: true }) organizationLocationLocationLookupTableModal: OrganizationLocationLocationLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    organizationLocation: CreateOrEditOrganizationLocationDto = new CreateOrEditOrganizationLocationDto();

    organizationUnitDisplayName = '';
    locationTitleEn = '';


    constructor(
        injector: Injector,
        private _organizationLocationsServiceProxy: OrganizationLocationsServiceProxy
    ) {
        super(injector);
    }

    show(organizationLocationId?: number): void {

        if (!organizationLocationId) {
            this.organizationLocation = new CreateOrEditOrganizationLocationDto();
            this.organizationLocation.id = organizationLocationId;
            this.organizationUnitDisplayName = '';
            this.locationTitleEn = '';

            this.active = true;
            this.modal.show();
        } else {
            this._organizationLocationsServiceProxy.getOrganizationLocationForEdit(organizationLocationId).subscribe(result => {
                this.organizationLocation = result.organizationLocation;

                this.organizationUnitDisplayName = result.organizationUnitDisplayName;
                this.locationTitleEn = result.locationTitleEn;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
            this.saving = true;

			
            this._organizationLocationsServiceProxy.createOrEdit(this.organizationLocation)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectOrganizationUnitModal() {
        this.organizationLocationOrganizationUnitLookupTableModal.id = this.organizationLocation.organizationUnitId;
        this.organizationLocationOrganizationUnitLookupTableModal.displayName = this.organizationUnitDisplayName;
        this.organizationLocationOrganizationUnitLookupTableModal.show();
    }
    openSelectLocationModal() {
        this.organizationLocationLocationLookupTableModal.id = this.organizationLocation.locationId;
        this.organizationLocationLocationLookupTableModal.displayName = this.locationTitleEn;
        this.organizationLocationLocationLookupTableModal.show();
    }


    setOrganizationUnitIdNull() {
        this.organizationLocation.organizationUnitId = null;
        this.organizationUnitDisplayName = '';
    }
    setLocationIdNull() {
        this.organizationLocation.locationId = null;
        this.locationTitleEn = '';
    }


    getNewOrganizationUnitId() {
        this.organizationLocation.organizationUnitId = this.organizationLocationOrganizationUnitLookupTableModal.id;
        this.organizationUnitDisplayName = this.organizationLocationOrganizationUnitLookupTableModal.displayName;
    }
    getNewLocationId() {
        this.organizationLocation.locationId = this.organizationLocationLocationLookupTableModal.id;
        this.locationTitleEn = this.organizationLocationLocationLookupTableModal.displayName;
    }


    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
