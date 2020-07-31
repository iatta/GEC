import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { UserTimeSheetApprovesServiceProxy, CreateOrEditUserTimeSheetApproveDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { UserTimeSheetApproveUserLookupTableModalComponent } from './userTimeSheetApprove-user-lookup-table-modal.component';
import { UserTimeSheetApproveProjectLookupTableModalComponent } from './userTimeSheetApprove-project-lookup-table-modal.component';

@Component({
    selector: 'createOrEditUserTimeSheetApproveModal',
    templateUrl: './create-or-edit-userTimeSheetApprove-modal.component.html'
})
export class CreateOrEditUserTimeSheetApproveModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('userTimeSheetApproveUserLookupTableModal', { static: true }) userTimeSheetApproveUserLookupTableModal: UserTimeSheetApproveUserLookupTableModalComponent;
    @ViewChild('userTimeSheetApproveUserLookupTableModal2', { static: true }) userTimeSheetApproveUserLookupTableModal2: UserTimeSheetApproveUserLookupTableModalComponent;
    @ViewChild('userTimeSheetApproveProjectLookupTableModal', { static: true }) userTimeSheetApproveProjectLookupTableModal: UserTimeSheetApproveProjectLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    userTimeSheetApprove: CreateOrEditUserTimeSheetApproveDto = new CreateOrEditUserTimeSheetApproveDto();

    fromDate: Date;
    toDate: Date;
    userName = '';
    userName2 = '';
    projectNameEn = '';


    constructor(
        injector: Injector,
        private _userTimeSheetApprovesServiceProxy: UserTimeSheetApprovesServiceProxy
    ) {
        super(injector);
    }

    show(userTimeSheetApproveId?: number): void {
    this.fromDate = null;
    this.toDate = null;

        if (!userTimeSheetApproveId) {
            this.userTimeSheetApprove = new CreateOrEditUserTimeSheetApproveDto();
            this.userTimeSheetApprove.id = userTimeSheetApproveId;
            this.userName = '';
            this.userName2 = '';
            this.projectNameEn = '';

            this.active = true;
            this.modal.show();
        } else {
            this._userTimeSheetApprovesServiceProxy.getUserTimeSheetApproveForEdit(userTimeSheetApproveId).subscribe(result => {
                this.userTimeSheetApprove = result.userTimeSheetApprove;

                if (this.userTimeSheetApprove.fromDate) {
					this.fromDate = this.userTimeSheetApprove.fromDate.toDate();
                }
                if (this.userTimeSheetApprove.toDate) {
					this.toDate = this.userTimeSheetApprove.toDate.toDate();
                }
                this.userName = result.userName;
                this.userName2 = result.userName2;
                this.projectNameEn = result.projectNameEn;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
            this.saving = true;

			
        if (this.fromDate) {
            if (!this.userTimeSheetApprove.fromDate) {
                this.userTimeSheetApprove.fromDate = moment(this.fromDate).startOf('day');
            }
            else {
                this.userTimeSheetApprove.fromDate = moment(this.fromDate);
            }
        }
        else {
            this.userTimeSheetApprove.fromDate = null;
        }
        if (this.toDate) {
            if (!this.userTimeSheetApprove.toDate) {
                this.userTimeSheetApprove.toDate = moment(this.toDate).startOf('day');
            }
            else {
                this.userTimeSheetApprove.toDate = moment(this.toDate);
            }
        }
        else {
            this.userTimeSheetApprove.toDate = null;
        }
            this._userTimeSheetApprovesServiceProxy.createOrEdit(this.userTimeSheetApprove)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectUserModal() {
        this.userTimeSheetApproveUserLookupTableModal.id = this.userTimeSheetApprove.userId;
        this.userTimeSheetApproveUserLookupTableModal.displayName = this.userName;
        this.userTimeSheetApproveUserLookupTableModal.show();
    }
    openSelectUserModal2() {
        this.userTimeSheetApproveUserLookupTableModal2.id = this.userTimeSheetApprove.projectManagerId;
        this.userTimeSheetApproveUserLookupTableModal2.displayName = this.userName;
        this.userTimeSheetApproveUserLookupTableModal2.show();
    }
    openSelectProjectModal() {
        this.userTimeSheetApproveProjectLookupTableModal.id = this.userTimeSheetApprove.projectId;
        this.userTimeSheetApproveProjectLookupTableModal.displayName = this.projectNameEn;
        this.userTimeSheetApproveProjectLookupTableModal.show();
    }


    setUserIdNull() {
        this.userTimeSheetApprove.userId = null;
        this.userName = '';
    }
    setProjectManagerIdNull() {
        this.userTimeSheetApprove.projectManagerId = null;
        this.userName2 = '';
    }
    setProjectIdNull() {
        this.userTimeSheetApprove.projectId = null;
        this.projectNameEn = '';
    }


    getNewUserId() {
        this.userTimeSheetApprove.userId = this.userTimeSheetApproveUserLookupTableModal.id;
        this.userName = this.userTimeSheetApproveUserLookupTableModal.displayName;
    }
    getNewProjectManagerId() {
        this.userTimeSheetApprove.projectManagerId = this.userTimeSheetApproveUserLookupTableModal2.id;
        this.userName2 = this.userTimeSheetApproveUserLookupTableModal2.displayName;
    }
    getNewProjectId() {
        this.userTimeSheetApprove.projectId = this.userTimeSheetApproveProjectLookupTableModal.id;
        this.projectNameEn = this.userTimeSheetApproveProjectLookupTableModal.displayName;
    }


    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
