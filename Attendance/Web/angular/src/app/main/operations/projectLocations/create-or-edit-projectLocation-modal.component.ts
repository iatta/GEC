import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { ProjectLocationsServiceProxy, CreateOrEditProjectLocationDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { ProjectLocationProjectLookupTableModalComponent } from './projectLocation-project-lookup-table-modal.component';
import { ProjectLocationLocationLookupTableModalComponent } from './projectLocation-location-lookup-table-modal.component';

@Component({
    selector: 'createOrEditProjectLocationModal',
    templateUrl: './create-or-edit-projectLocation-modal.component.html'
})
export class CreateOrEditProjectLocationModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('projectLocationProjectLookupTableModal', { static: true }) projectLocationProjectLookupTableModal: ProjectLocationProjectLookupTableModalComponent;
    @ViewChild('projectLocationLocationLookupTableModal', { static: true }) projectLocationLocationLookupTableModal: ProjectLocationLocationLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    projectLocation: CreateOrEditProjectLocationDto = new CreateOrEditProjectLocationDto();

    projectNameEn = '';
    locationTitleEn = '';


    constructor(
        injector: Injector,
        private _projectLocationsServiceProxy: ProjectLocationsServiceProxy
    ) {
        super(injector);
    }

    show(projectLocationId?: number): void {

        if (!projectLocationId) {
            this.projectLocation = new CreateOrEditProjectLocationDto();
            this.projectLocation.id = projectLocationId;
            this.projectNameEn = '';
            this.locationTitleEn = '';

            this.active = true;
            this.modal.show();
        } else {
            this._projectLocationsServiceProxy.getProjectLocationForEdit(projectLocationId).subscribe(result => {
                this.projectLocation = result.projectLocation;

                this.projectNameEn = result.projectNameEn;
                this.locationTitleEn = result.locationTitleEn;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
            this.saving = true;

			
            this._projectLocationsServiceProxy.createOrEdit(this.projectLocation)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectProjectModal() {
        this.projectLocationProjectLookupTableModal.id = this.projectLocation.projectId;
        this.projectLocationProjectLookupTableModal.displayName = this.projectNameEn;
        this.projectLocationProjectLookupTableModal.show();
    }
    openSelectLocationModal() {
        this.projectLocationLocationLookupTableModal.id = this.projectLocation.locationId;
        this.projectLocationLocationLookupTableModal.displayName = this.locationTitleEn;
        this.projectLocationLocationLookupTableModal.show();
    }


    setProjectIdNull() {
        this.projectLocation.projectId = null;
        this.projectNameEn = '';
    }
    setLocationIdNull() {
        this.projectLocation.locationId = null;
        this.locationTitleEn = '';
    }


    getNewProjectId() {
        this.projectLocation.projectId = this.projectLocationProjectLookupTableModal.id;
        this.projectNameEn = this.projectLocationProjectLookupTableModal.displayName;
    }
    getNewLocationId() {
        this.projectLocation.locationId = this.projectLocationLocationLookupTableModal.id;
        this.locationTitleEn = this.projectLocationLocationLookupTableModal.displayName;
    }


    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
