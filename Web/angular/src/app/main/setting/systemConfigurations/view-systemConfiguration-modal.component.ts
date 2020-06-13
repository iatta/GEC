import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { GetSystemConfigurationForViewDto, SystemConfigurationDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewSystemConfigurationModal',
    templateUrl: './view-systemConfiguration-modal.component.html'
})
export class ViewSystemConfigurationModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetSystemConfigurationForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetSystemConfigurationForViewDto();
        this.item.systemConfiguration = new SystemConfigurationDto();
    }

    show(item: GetSystemConfigurationForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
