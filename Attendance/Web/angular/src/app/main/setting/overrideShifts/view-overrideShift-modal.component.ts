import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { GetOverrideShiftForViewDto, OverrideShiftDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewOverrideShiftModal',
    templateUrl: './view-overrideShift-modal.component.html'
})
export class ViewOverrideShiftModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetOverrideShiftForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetOverrideShiftForViewDto();
        this.item.overrideShift = new OverrideShiftDto();
    }

    show(item: GetOverrideShiftForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
