import { NgForm } from '@angular/forms';
import { OrganizationManagerModalComponent } from './organization-manager-modal';
import { ChangeDetectorRef, Component, ElementRef, EventEmitter, Injector, Output, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { CreateOrganizationUnitInput, OrganizationUnitDto, OrganizationUnitServiceProxy, UpdateOrganizationUnitInput } from '@shared/service-proxies/service-proxies';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';

export interface IOrganizationUnitOnEdit {
    id?: number;
    parentId?: number;
    displayName?: string;
    managerId?:number;
    userName?:string;
    hasApprove?:boolean;
}

@Component({
    selector: 'createOrEditOrganizationUnitModal',
    templateUrl: './create-or-edit-unit-modal.component.html'
})
export class CreateOrEditUnitModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', {static: true}) modal: ModalDirective;
    @ViewChild('organizationUnitDisplayName', {static: true}) organizationUnitDisplayNameInput: ElementRef;
    @ViewChild('organizationManagerModal', { static: true }) organizationManagerModal: OrganizationManagerModalComponent;

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


}
