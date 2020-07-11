import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { GetBeaconForViewDto, BeaconDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewBeaconModal',
    templateUrl: './view-beacon-modal.component.html'
})
export class ViewBeaconModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetBeaconForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetBeaconForViewDto();
        this.item.beacon = new BeaconDto();
    }

    show(item: GetBeaconForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
