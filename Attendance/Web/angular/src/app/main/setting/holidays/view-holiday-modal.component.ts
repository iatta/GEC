import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { GetHolidayForViewDto, HolidayDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewHolidayModal',
    templateUrl: './view-holiday-modal.component.html'
})
export class ViewHolidayModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetHolidayForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetHolidayForViewDto();
        this.item.holiday = new HolidayDto();
    }

    show(item: GetHolidayForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
