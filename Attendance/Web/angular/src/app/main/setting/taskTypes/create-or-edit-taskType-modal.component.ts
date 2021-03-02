import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { TaskTypesServiceProxy, CreateOrEditTaskTypeDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';

@Component({
    selector: 'createOrEditTaskTypeModal',
    templateUrl: './create-or-edit-taskType-modal.component.html'
})
export class CreateOrEditTaskTypeModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    taskType: CreateOrEditTaskTypeDto = new CreateOrEditTaskTypeDto();



    constructor(
        injector: Injector,
        private _taskTypesServiceProxy: TaskTypesServiceProxy
    ) {
        super(injector);
    }

    show(taskTypeId?: number): void {

        if (!taskTypeId) {
            this.taskType = new CreateOrEditTaskTypeDto();
            this.taskType.id = taskTypeId;

            this.active = true;
            this.modal.show();
        } else {
            this._taskTypesServiceProxy.getTaskTypeForEdit(taskTypeId).subscribe(result => {
                this.taskType = result.taskType;


                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
            this.saving = true;

			
            this._taskTypesServiceProxy.createOrEdit(this.taskType)
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
