import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { MobileWebPagesServiceProxy, CreateOrEditMobileWebPageDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';

@Component({
    selector: 'createOrEditMobileWebPageModal',
    templateUrl: './create-or-edit-mobileWebPage-modal.component.html'
})
export class CreateOrEditMobileWebPageModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    mobileWebPage: CreateOrEditMobileWebPageDto = new CreateOrEditMobileWebPageDto();



    constructor(
        injector: Injector,
        private _mobileWebPagesServiceProxy: MobileWebPagesServiceProxy
    ) {
        super(injector);
    }

    show(mobileWebPageId?: number): void {

        if (!mobileWebPageId) {
            this.mobileWebPage = new CreateOrEditMobileWebPageDto();
            this.mobileWebPage.id = mobileWebPageId;

            this.active = true;
            this.modal.show();
        } else {
            this._mobileWebPagesServiceProxy.getMobileWebPageForEdit(mobileWebPageId).subscribe(result => {
                this.mobileWebPage = result.mobileWebPage;


                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
            this.saving = true;

			
            this._mobileWebPagesServiceProxy.createOrEdit(this.mobileWebPage)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }







    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
