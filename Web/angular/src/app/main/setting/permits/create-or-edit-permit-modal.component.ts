import { NgForm } from '@angular/forms';
import { Component, ViewChild, Injector, Output, EventEmitter, OnInit } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { PermitsServiceProxy,TypesOfPermitsServiceProxy, CreateOrEditPermitDto ,TypesOfPermitDto ,GetTypesOfPermitForViewDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';

@Component({
    selector: 'createOrEditPermitModal',
    templateUrl: './create-or-edit-permit-modal.component.html'
})
export class CreateOrEditPermitModalComponent extends AppComponentBase implements OnInit {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;
    isDeducted = '0';

    permit: CreateOrEditPermitDto = new CreateOrEditPermitDto();
    typesOfPermit: GetTypesOfPermitForViewDto[] | undefined;


    constructor(
        injector: Injector,
        private _permitsServiceProxy: PermitsServiceProxy,
        private _typesOfPermitsServiceProxy: TypesOfPermitsServiceProxy
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this._typesOfPermitsServiceProxy.getAllFlat().subscribe(result => {
            console.log(result);
            this.typesOfPermit = result;
        });
    }



    show(permitId?: number): void {

        if (!permitId) {
            this.permit = new CreateOrEditPermitDto();
            this.permit.id = permitId;
            this.isDeducted = '0';
            this.active = true;
            this.modal.show();
        } else {
            this._permitsServiceProxy.getPermitForEdit(permitId).subscribe(result => {
                this.permit = result.permit;

                this.isDeducted = this.permit.isDeducted ? '1' : '0';

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(permitForm: NgForm): void {
        if (permitForm.form.valid) {
            this.saving = true;
            this.permit.isDeducted = this.isDeducted == '0' ? false : true;
            this._permitsServiceProxy.createOrEdit(this.permit)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
        }
    }







    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
