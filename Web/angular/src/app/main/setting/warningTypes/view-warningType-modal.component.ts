import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { GetWarningTypeForViewDto, WarningTypeDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewWarningTypeModal',
    templateUrl: './view-warningType-modal.component.html'
})
export class ViewWarningTypeModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetWarningTypeForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetWarningTypeForViewDto();
        this.item.warningType = new WarningTypeDto();
    }

    show(item: GetWarningTypeForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
