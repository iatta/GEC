import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { GetOfficialTaskTypeForViewDto, OfficialTaskTypeDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewOfficialTaskTypeModal',
    templateUrl: './view-officialTaskType-modal.component.html'
})
export class ViewOfficialTaskTypeModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetOfficialTaskTypeForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetOfficialTaskTypeForViewDto();
        this.item.officialTaskType = new OfficialTaskTypeDto();
    }

    show(item: GetOfficialTaskTypeForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
