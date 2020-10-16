import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { GetLocationMachineForViewDto, LocationMachineDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewLocationMachineModal',
    templateUrl: './view-locationMachine-modal.component.html'
})
export class ViewLocationMachineModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetLocationMachineForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetLocationMachineForViewDto();
        this.item.locationMachine = new LocationMachineDto();
    }

    show(item: GetLocationMachineForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
