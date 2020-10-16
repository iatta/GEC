import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { LocationMachinesServiceProxy, CreateOrEditLocationMachineDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { LocationMachineLocationLookupTableModalComponent } from './locationMachine-location-lookup-table-modal.component';
import { LocationMachineMachineLookupTableModalComponent } from './locationMachine-machine-lookup-table-modal.component';

@Component({
    selector: 'createOrEditLocationMachineModal',
    templateUrl: './create-or-edit-locationMachine-modal.component.html'
})
export class CreateOrEditLocationMachineModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('locationMachineLocationLookupTableModal', { static: true }) locationMachineLocationLookupTableModal: LocationMachineLocationLookupTableModalComponent;
    @ViewChild('locationMachineMachineLookupTableModal', { static: true }) locationMachineMachineLookupTableModal: LocationMachineMachineLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    locationMachine: CreateOrEditLocationMachineDto = new CreateOrEditLocationMachineDto();

    locationTitleAr = '';
    machineNameEn = '';


    constructor(
        injector: Injector,
        private _locationMachinesServiceProxy: LocationMachinesServiceProxy
    ) {
        super(injector);
    }

    show(locationMachineId?: number): void {

        if (!locationMachineId) {
            this.locationMachine = new CreateOrEditLocationMachineDto();
            this.locationMachine.id = locationMachineId;
            this.locationTitleAr = '';
            this.machineNameEn = '';

            this.active = true;
            this.modal.show();
        } else {
            this._locationMachinesServiceProxy.getLocationMachineForEdit(locationMachineId).subscribe(result => {
                this.locationMachine = result.locationMachine;

                this.locationTitleAr = result.locationTitleAr;
                this.machineNameEn = result.machineNameEn;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
            this.saving = true;

			
            this._locationMachinesServiceProxy.createOrEdit(this.locationMachine)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectLocationModal() {
        this.locationMachineLocationLookupTableModal.id = this.locationMachine.locationId;
        this.locationMachineLocationLookupTableModal.displayName = this.locationTitleAr;
        this.locationMachineLocationLookupTableModal.show();
    }
    openSelectMachineModal() {
        this.locationMachineMachineLookupTableModal.id = this.locationMachine.machineId;
        this.locationMachineMachineLookupTableModal.displayName = this.machineNameEn;
        this.locationMachineMachineLookupTableModal.show();
    }


    setLocationIdNull() {
        this.locationMachine.locationId = null;
        this.locationTitleAr = '';
    }
    setMachineIdNull() {
        this.locationMachine.machineId = null;
        this.machineNameEn = '';
    }


    getNewLocationId() {
        this.locationMachine.locationId = this.locationMachineLocationLookupTableModal.id;
        this.locationTitleAr = this.locationMachineLocationLookupTableModal.displayName;
    }
    getNewMachineId() {
        this.locationMachine.machineId = this.locationMachineMachineLookupTableModal.id;
        this.machineNameEn = this.locationMachineMachineLookupTableModal.displayName;
    }


    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
