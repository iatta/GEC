import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { UserDelegationsServiceProxy, CreateOrEditUserDelegationDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { UserDelegationUserLookupTableModalComponent } from './userDelegation-user-lookup-table-modal.component';

@Component({
    selector: 'createOrEditUserDelegationModal',
    templateUrl: './create-or-edit-userDelegation-modal.component.html'
})
export class CreateOrEditUserDelegationModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('userDelegationUserLookupTableModal', { static: true }) userDelegationUserLookupTableModal: UserDelegationUserLookupTableModalComponent;
    @ViewChild('userDelegationUserLookupTableModal2', { static: true }) userDelegationUserLookupTableModal2: UserDelegationUserLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    userDelegation: CreateOrEditUserDelegationDto = new CreateOrEditUserDelegationDto();

    userName = '';
    userName2 = '';


    constructor(
        injector: Injector,
        private _userDelegationsServiceProxy: UserDelegationsServiceProxy
    ) {
        super(injector);
    }

    show(userDelegationId?: number): void {

        if (!userDelegationId) {
            this.userDelegation = new CreateOrEditUserDelegationDto();
            this.userDelegation.id = userDelegationId;
            this.userDelegation.fromDate = moment().startOf('day');
            this.userDelegation.toDate = moment().startOf('day');
            this.userName = '';
            this.userName2 = '';

            this.active = true;
            this.modal.show();
        } else {
            this._userDelegationsServiceProxy.getUserDelegationForEdit(userDelegationId).subscribe(result => {
                this.userDelegation = result.userDelegation;

                this.userName = result.userName;
                this.userName2 = result.userName2;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
            this.saving = true;

			
            this._userDelegationsServiceProxy.createOrEdit(this.userDelegation)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectUserModal() {
        this.userDelegationUserLookupTableModal.id = this.userDelegation.userId;
        this.userDelegationUserLookupTableModal.displayName = this.userName;
        this.userDelegationUserLookupTableModal.show();
    }
    openSelectUserModal2() {
        this.userDelegationUserLookupTableModal2.id = this.userDelegation.delegatedUserId;
        this.userDelegationUserLookupTableModal2.displayName = this.userName;
        this.userDelegationUserLookupTableModal2.show();
    }


    setUserIdNull() {
        this.userDelegation.userId = null;
        this.userName = '';
    }
    setDelegatedUserIdNull() {
        this.userDelegation.delegatedUserId = null;
        this.userName2 = '';
    }


    getNewUserId() {
        this.userDelegation.userId = this.userDelegationUserLookupTableModal.id;
        this.userName = this.userDelegationUserLookupTableModal.displayName;
    }
    getNewDelegatedUserId() {
        this.userDelegation.delegatedUserId = this.userDelegationUserLookupTableModal2.id;
        this.userName2 = this.userDelegationUserLookupTableModal2.displayName;
    }


    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
