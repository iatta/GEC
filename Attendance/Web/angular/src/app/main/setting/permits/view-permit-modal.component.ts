import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { GetPermitForViewDto, PermitDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewPermitModal',
    templateUrl: './view-permit-modal.component.html'
})
export class ViewPermitModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetPermitForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetPermitForViewDto();
        this.item.permit = new PermitDto();
    }

    show(item: GetPermitForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
