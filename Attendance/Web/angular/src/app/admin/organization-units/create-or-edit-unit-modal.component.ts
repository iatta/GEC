import { OrganizationLocationLocationLookupTableModalComponent } from './../../main/operations/organizationLocations/organizationLocation-location-lookup-table-modal.component';
import { NgForm } from '@angular/forms';
import { OrganizationManagerModalComponent } from './organization-manager-modal';
import { ChangeDetectorRef, Component, ElementRef, EventEmitter, Injector, Output, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { CreateOrganizationUnitInput, OrganizationLocationDto, OrganizationUnitDto, OrganizationUnitServiceProxy, UpdateOrganizationUnitInput } from '@shared/service-proxies/service-proxies';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';

export interface IOrganizationUnitOnEdit {
    id?: number;
    parentId?: number;
    displayName?: string;
    managerId?:number;
    userName?:string;
    hasApprove?:boolean;
    locations?:OrganizationLocationDto[];
}

@Component({
    selector: 'createOrEditOrganizationUnitModal',
    templateUrl: './create-or-edit-unit-modal.component.html'
})
export class CreateOrEditUnitModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', {static: true}) modal: ModalDirective;
    @ViewChild('organizationUnitDisplayName', {static: true}) organizationUnitDisplayNameInput: ElementRef;
    @ViewChild('organizationManagerModal', { static: true }) organizationManagerModal: OrganizationManagerModalComponent;
    @ViewChild('organizationLocationLocationLookupTableModal', { static: true }) organizationLocationLocationLookupTableModal: OrganizationLocationLocationLookupTableModalComponent;
    @Output() unitCreated: EventEmitter<OrganizationUnitDto> = new EventEmitter<OrganizationUnitDto>();
    @Output() unitUpdated: EventEmitter<OrganizationUnitDto> = new EventEmitter<OrganizationUnitDto>();

    active = false;
    saving = false;
    userName = '';

    organizationUnit: IOrganizationUnitOnEdit = {};

    constructor(
        injector: Injector,
        private _organizationUnitService: OrganizationUnitServiceProxy,
        private _changeDetector: ChangeDetectorRef
    ) {
        super(injector);
    }

    onShown(): void {
        document.getElementById('OrganizationUnitDisplayName').focus();
    }

    show(organizationUnit: IOrganizationUnitOnEdit): void {

        this.organizationUnit = organizationUnit;
        this.organizationUnit.locations = organizationUnit.locations;
        if(!this.organizationUnit.locations)
            this.organizationUnit.locations = [];


        this.userName = organizationUnit.userName;
        this.active = true;
        this.modal.show();
        this._changeDetector.detectChanges();
    }

    save(editForm:NgForm): void {
        if(editForm.valid){
            if (!this.organizationUnit.id) {
                this.createUnit();
            } else {
                this.updateUnit();
            }
        }

    }

    createUnit() {
        const createInput = new CreateOrganizationUnitInput();
        createInput.parentId = this.organizationUnit.parentId;
        createInput.displayName = this.organizationUnit.displayName;
        createInput.managerId = this.organizationUnit.managerId;
        createInput.hasApprove = this.organizationUnit.hasApprove;
        createInput.locations = this.organizationUnit.locations;

        this.saving = true;
        this._organizationUnitService
            .createOrganizationUnit(createInput)
            .pipe(finalize(() => this.saving = false))
            .subscribe((result: OrganizationUnitDto) => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                result.managerName = this.userName;
                this.unitCreated.emit(result);
            });
    }

    updateUnit() {

        const updateInput = new UpdateOrganizationUnitInput();
        updateInput.id = this.organizationUnit.id;
        updateInput.displayName = this.organizationUnit.displayName;
        updateInput.managerId = this.organizationUnit.managerId;
        updateInput.hasApprove = this.organizationUnit.hasApprove;
        updateInput.locations = this.organizationUnit.locations;
        this.saving = true;
        this._organizationUnitService
            .updateOrganizationUnit(updateInput)
            .pipe(finalize(() => this.saving = false))
            .subscribe((result: OrganizationUnitDto) => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                result.managerName = this.userName;
                this.unitUpdated.emit(result);
            });
    }

    close(): void {
        this.modal.hide();
        this.active = false;
    }

    //organization manager
    openSelectUserModal() {
        this.organizationManagerModal.id = this.organizationUnit.managerId;
        this.organizationManagerModal.displayName = this.organizationUnit.userName;
        this.organizationManagerModal.show();
    }



    setUserIdNull() {
        this.organizationUnit.managerId = null;
        this.userName = '';
    }

    getNewUserId() {
        this.organizationUnit.managerId = this.organizationManagerModal.id;
        this.userName = this.organizationManagerModal.displayName;
    }

    openSelectLocationModal() {
        debugger
        // this.organizationLocationLocationLookupTableModal.id = this.organizationLocation.locationId;
        // this.organizationLocationLocationLookupTableModal.displayName = this.locationTitleEn;
        this.organizationLocationLocationLookupTableModal.show();
    }


    setOrganizationUnitIdNull() {
        // this.organizationLocation.organizationUnitId = null;
        // this.organizationUnitDisplayName = '';
    }
    setLocationIdNull() {
        // this.organizationLocation.locationId = null;
        // this.locationTitleEn = '';
    }

    getNewLocationId() {
        // this.organizationLocation.locationId = this.organiz
        if(this.isLocationExist(this.organizationLocationLocationLookupTableModal.id)){
            this.message.warn("Location Already Added To This Unit");
        }else{
            if(this.organizationLocationLocationLookupTableModal.id > 0){
                let locationToAdd = new OrganizationLocationDto();
                locationToAdd.locationId = this.organizationLocationLocationLookupTableModal.id;
                locationToAdd.locationName = this.organizationLocationLocationLookupTableModal.displayName;
                this.organizationUnit.locations.push(locationToAdd);
            }

        }
    }


    isLocationExist(locationId:number){
        if(this.organizationUnit.locations.length  == 0)
            return false;

        let exist = this.organizationUnit.locations.findIndex(x =>  x.locationId == locationId);

        return exist > -1;
    }

    removeLocation(location: OrganizationLocationDto){
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {

                    let index = this.organizationUnit.locations.indexOf(location);
                    if(index > -1){
                        this.organizationUnit.locations.splice(index,1);
                        this.notify.success(this.l('SuccessfullyDeleted'));
                    }

                }
            }
        );

    }



}
