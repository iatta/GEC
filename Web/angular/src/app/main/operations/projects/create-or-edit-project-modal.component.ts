import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { ProjectsServiceProxy, CreateOrEditProjectDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { ProjectUserLookupTableModalComponent } from './project-user-lookup-table-modal.component';
import { ProjectOrganizationUnitLookupTableModalComponent } from './project-organizationUnit-lookup-table-modal.component';
import { ProjectLocationLookupTableModalComponent } from './project-location-lookup-table-modal.component';

@Component({
    selector: 'createOrEditProjectModal',
    templateUrl: './create-or-edit-project-modal.component.html'
})
export class CreateOrEditProjectModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('projectUserLookupTableModal', { static: true }) projectUserLookupTableModal: ProjectUserLookupTableModalComponent;
    @ViewChild('projectOrganizationUnitLookupTableModal', { static: true }) projectOrganizationUnitLookupTableModal: ProjectOrganizationUnitLookupTableModalComponent;
    @ViewChild('projectLocationLookupTableModal', { static: true }) projectLocationLookupTableModal: ProjectLocationLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    project: CreateOrEditProjectDto = new CreateOrEditProjectDto();

    userName = '';
    organizationUnitDisplayName = '';
    locationTitleEn = '';


    constructor(
        injector: Injector,
        private _projectsServiceProxy: ProjectsServiceProxy
    ) {
        super(injector);
    }

    show(projectId?: number): void {

        if (!projectId) {
            this.project = new CreateOrEditProjectDto();
            this.project.id = projectId;
            this.userName = '';
            this.organizationUnitDisplayName = '';
            this.locationTitleEn = '';

            this.active = true;
            this.modal.show();
        } else {
            this._projectsServiceProxy.getProjectForEdit(projectId).subscribe(result => {
                this.project = result.project;

                this.userName = result.userName;
                this.organizationUnitDisplayName = result.organizationUnitDisplayName;
                this.locationTitleEn = result.locationTitleEn;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
            this.saving = true;

			
            this._projectsServiceProxy.createOrEdit(this.project)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectUserModal() {
        this.projectUserLookupTableModal.id = this.project.managerId;
        this.projectUserLookupTableModal.displayName = this.userName;
        this.projectUserLookupTableModal.show();
    }
    openSelectOrganizationUnitModal() {
        this.projectOrganizationUnitLookupTableModal.id = this.project.organizationUnitId;
        this.projectOrganizationUnitLookupTableModal.displayName = this.organizationUnitDisplayName;
        this.projectOrganizationUnitLookupTableModal.show();
    }
    openSelectLocationModal() {
        this.projectLocationLookupTableModal.id = this.project.locationId;
        this.projectLocationLookupTableModal.displayName = this.locationTitleEn;
        this.projectLocationLookupTableModal.show();
    }


    setManagerIdNull() {
        this.project.managerId = null;
        this.userName = '';
    }
    setOrganizationUnitIdNull() {
        this.project.organizationUnitId = null;
        this.organizationUnitDisplayName = '';
    }
    setLocationIdNull() {
        this.project.locationId = null;
        this.locationTitleEn = '';
    }


    getNewManagerId() {
        this.project.managerId = this.projectUserLookupTableModal.id;
        this.userName = this.projectUserLookupTableModal.displayName;
    }
    getNewOrganizationUnitId() {
        this.project.organizationUnitId = this.projectOrganizationUnitLookupTableModal.id;
        this.organizationUnitDisplayName = this.projectOrganizationUnitLookupTableModal.displayName;
    }
    getNewLocationId() {
        this.project.locationId = this.projectLocationLookupTableModal.id;
        this.locationTitleEn = this.projectLocationLookupTableModal.displayName;
    }


    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
