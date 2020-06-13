import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { GetTimeProfileDetailForViewDto, TimeProfileDetailDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewTimeProfileDetailModal',
    templateUrl: './view-timeProfileDetail-modal.component.html'
})
export class ViewTimeProfileDetailModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetTimeProfileDetailForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetTimeProfileDetailForViewDto();
        this.item.timeProfileDetail = new TimeProfileDetailDto();
    }

    show(item: GetTimeProfileDetailForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
