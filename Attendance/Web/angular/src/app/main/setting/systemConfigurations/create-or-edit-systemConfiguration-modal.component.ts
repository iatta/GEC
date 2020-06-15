import { Component, ViewChild, Injector, Output, EventEmitter, OnInit } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { SystemConfigurationsServiceProxy, CreateOrEditSystemConfigurationDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';

@Component({
    selector: 'createOrEditSystemConfigurationModal',
    templateUrl: './create-or-edit-systemConfiguration-modal.component.html'
})
export class CreateOrEditSystemConfigurationModalComponent extends AppComponentBase implements OnInit {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    systemConfiguration: CreateOrEditSystemConfigurationDto = new CreateOrEditSystemConfigurationDto();



    constructor(
        injector: Injector,
        private _systemConfigurationsServiceProxy: SystemConfigurationsServiceProxy
    ) {
        super(injector);
    }
    ngOnInit(): void {
        this._systemConfigurationsServiceProxy.getSystemConfigurationForEdit(1).subscribe(result => {
            this.systemConfiguration = result.systemConfiguration;
            this.active = true;
        });
    }


    save(): void {
            this.saving = true;
            this._systemConfigurationsServiceProxy.createOrEdit(this.systemConfiguration)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
             });
    }







    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
