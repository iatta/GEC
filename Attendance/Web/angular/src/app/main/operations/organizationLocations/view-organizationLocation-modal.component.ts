import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { GetOrganizationLocationForViewDto, OrganizationLocationDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewOrganizationLocationModal',
    templateUrl: './view-organizationLocation-modal.component.html'
})
export class ViewOrganizationLocationModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetOrganizationLocationForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetOrganizationLocationForViewDto();
        this.item.organizationLocation = new OrganizationLocationDto();
    }

    show(item: GetOrganizationLocationForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
