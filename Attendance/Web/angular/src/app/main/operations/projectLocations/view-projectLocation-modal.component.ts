import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { GetProjectLocationForViewDto, ProjectLocationDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewProjectLocationModal',
    templateUrl: './view-projectLocation-modal.component.html'
})
export class ViewProjectLocationModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetProjectLocationForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetProjectLocationForViewDto();
        this.item.projectLocation = new ProjectLocationDto();
    }

    show(item: GetProjectLocationForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
