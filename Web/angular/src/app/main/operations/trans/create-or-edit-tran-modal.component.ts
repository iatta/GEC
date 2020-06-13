import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { TransServiceProxy, CreateOrEditTranDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { TranUserLookupTableModalComponent } from './tran-user-lookup-table-modal.component';

@Component({
    selector: 'createOrEditTranModal',
    templateUrl: './create-or-edit-tran-modal.component.html'
})
export class CreateOrEditTranModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('tranUserLookupTableModal', { static: true }) tranUserLookupTableModal: TranUserLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    tran: CreateOrEditTranDto = new CreateOrEditTranDto();

    userName = '';


    constructor(
        injector: Injector,
        private _transServiceProxy: TransServiceProxy
    ) {
        super(injector);
    }

    show(tranId?: number): void {

        if (!tranId) {
            this.tran = new CreateOrEditTranDto();
            this.tran.id = tranId;
            this.userName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._transServiceProxy.getTranForEdit(tranId).subscribe(result => {
                this.tran = result.tran;

                this.userName = result.userName;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
            this.saving = true;

			
            this._transServiceProxy.createOrEdit(this.tran)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectUserModal() {
        this.tranUserLookupTableModal.id = this.tran.userId;
        this.tranUserLookupTableModal.displayName = this.userName;
        this.tranUserLookupTableModal.show();
    }


    setUserIdNull() {
        this.tran.userId = null;
        this.userName = '';
    }


    getNewUserId() {
        this.tran.userId = this.tranUserLookupTableModal.id;
        this.userName = this.tranUserLookupTableModal.displayName;
    }


    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
