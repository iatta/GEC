import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { MachinesServiceProxy, CreateOrEditMachineDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { MachineOrganizationUnitLookupTableModalComponent } from './machine-organizationUnit-lookup-table-modal.component';

@Component({
    selector: 'createOrEditMachineModal',
    templateUrl: './create-or-edit-machine-modal.component.html'
})
export class CreateOrEditMachineModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('machineOrganizationUnitLookupTableModal', { static: true }) machineOrganizationUnitLookupTableModal: MachineOrganizationUnitLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    machine: CreateOrEditMachineDto = new CreateOrEditMachineDto();

    organizationUnitDisplayName = '';


    constructor(
        injector: Injector,
        private _machinesServiceProxy: MachinesServiceProxy
    ) {
        super(injector);
    }

    show(machineId?: number): void {

        if (!machineId) {
            this.machine = new CreateOrEditMachineDto();
            this.machine.id = machineId;
            this.organizationUnitDisplayName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._machinesServiceProxy.getMachineForEdit(machineId).subscribe(result => {
                this.machine = result.machine;

                this.organizationUnitDisplayName = result.organizationUnitDisplayName;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
            this.saving = true;

			
            this._machinesServiceProxy.createOrEdit(this.machine)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectOrganizationUnitModal() {
        this.machineOrganizationUnitLookupTableModal.id = this.machine.organizationUnitId;
        this.machineOrganizationUnitLookupTableModal.displayName = this.organizationUnitDisplayName;
        this.machineOrganizationUnitLookupTableModal.show();
    }


    setOrganizationUnitIdNull() {
        this.machine.organizationUnitId = null;
        this.organizationUnitDisplayName = '';
    }


    getNewOrganizationUnitId() {
        this.machine.organizationUnitId = this.machineOrganizationUnitLookupTableModal.id;
        this.organizationUnitDisplayName = this.machineOrganizationUnitLookupTableModal.displayName;
    }


    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
