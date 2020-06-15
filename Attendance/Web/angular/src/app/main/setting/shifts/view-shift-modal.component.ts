import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { GetShiftForViewDto, ShiftDto} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewShiftModal',
    templateUrl: './view-shift-modal.component.html'
})
export class ViewShiftModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetShiftForViewDto;




    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetShiftForViewDto();
        this.item.shift = new ShiftDto();
    }

    show(item: GetShiftForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
