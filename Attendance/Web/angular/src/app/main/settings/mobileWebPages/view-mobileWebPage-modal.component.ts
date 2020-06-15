import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { GetMobileWebPageForViewDto, MobileWebPageDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewMobileWebPageModal',
    templateUrl: './view-mobileWebPage-modal.component.html'
})
export class ViewMobileWebPageModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetMobileWebPageForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetMobileWebPageForViewDto();
        this.item.mobileWebPage = new MobileWebPageDto();
    }

    show(item: GetMobileWebPageForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
