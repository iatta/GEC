import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { GetUserDelegationForViewDto, UserDelegationDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewUserDelegationModal',
    templateUrl: './view-userDelegation-modal.component.html'
})
export class ViewUserDelegationModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetUserDelegationForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetUserDelegationForViewDto();
        this.item.userDelegation = new UserDelegationDto();
    }

    show(item: GetUserDelegationForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
