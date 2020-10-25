import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { EmployeeTempTransfersServiceProxy, CreateOrEditEmployeeTempTransferDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { EmployeeTempTransferOrganizationUnitLookupTableModalComponent } from './employeeTempTransfer-organizationUnit-lookup-table-modal.component';
import { EmployeeTempTransferUserLookupTableModalComponent } from './employeeTempTransfer-user-lookup-table-modal.component';

@Component({
    selector: 'createOrEditEmployeeTempTransferModal',
    templateUrl: './create-or-edit-employeeTempTransfer-modal.component.html'
})
export class CreateOrEditEmployeeTempTransferModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('employeeTempTransferOrganizationUnitLookupTableModal', { static: true }) employeeTempTransferOrganizationUnitLookupTableModal: EmployeeTempTransferOrganizationUnitLookupTableModalComponent;
    @ViewChild('employeeTempTransferUserLookupTableModal', { static: true }) employeeTempTransferUserLookupTableModal: EmployeeTempTransferUserLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    employeeTempTransfer: CreateOrEditEmployeeTempTransferDto = new CreateOrEditEmployeeTempTransferDto();

    organizationUnitDisplayName = '';
    userName = '';


    constructor(
        injector: Injector,
        private _employeeTempTransfersServiceProxy: EmployeeTempTransfersServiceProxy
    ) {
        super(injector);
    }

    show(employeeTempTransferId?: number): void {

        if (!employeeTempTransferId) {
            this.employeeTempTransfer = new CreateOrEditEmployeeTempTransferDto();
            this.employeeTempTransfer.id = employeeTempTransferId;
            this.employeeTempTransfer.fromDate = moment().startOf('day');
            this.employeeTempTransfer.toDate = moment().startOf('day');
            this.organizationUnitDisplayName = '';
            this.userName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._employeeTempTransfersServiceProxy.getEmployeeTempTransferForEdit(employeeTempTransferId).subscribe(result => {
                this.employeeTempTransfer = result.employeeTempTransfer;

                this.organizationUnitDisplayName = result.organizationUnitDisplayName;
                this.userName = result.userName;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
            this.saving = true;

			
            this._employeeTempTransfersServiceProxy.createOrEdit(this.employeeTempTransfer)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectOrganizationUnitModal() {
        this.employeeTempTransferOrganizationUnitLookupTableModal.id = this.employeeTempTransfer.organizationUnitId;
        this.employeeTempTransferOrganizationUnitLookupTableModal.displayName = this.organizationUnitDisplayName;
        this.employeeTempTransferOrganizationUnitLookupTableModal.show();
    }
    openSelectUserModal() {
        this.employeeTempTransferUserLookupTableModal.id = this.employeeTempTransfer.userId;
        this.employeeTempTransferUserLookupTableModal.displayName = this.userName;
        this.employeeTempTransferUserLookupTableModal.show();
    }


    setOrganizationUnitIdNull() {
        this.employeeTempTransfer.organizationUnitId = null;
        this.organizationUnitDisplayName = '';
    }
    setUserIdNull() {
        this.employeeTempTransfer.userId = null;
        this.userName = '';
    }


    getNewOrganizationUnitId() {
        this.employeeTempTransfer.organizationUnitId = this.employeeTempTransferOrganizationUnitLookupTableModal.id;
        this.organizationUnitDisplayName = this.employeeTempTransferOrganizationUnitLookupTableModal.displayName;
    }
    getNewUserId() {
        this.employeeTempTransfer.userId = this.employeeTempTransferUserLookupTableModal.id;
        this.userName = this.employeeTempTransferUserLookupTableModal.displayName;
    }


    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
