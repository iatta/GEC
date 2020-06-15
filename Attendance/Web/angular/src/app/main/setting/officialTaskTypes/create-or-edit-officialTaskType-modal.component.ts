import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { OfficialTaskTypesServiceProxy, CreateOrEditOfficialTaskTypeDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';

@Component({
    selector: 'createOrEditOfficialTaskTypeModal',
    templateUrl: './create-or-edit-officialTaskType-modal.component.html'
})
export class CreateOrEditOfficialTaskTypeModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    officialTaskType: CreateOrEditOfficialTaskTypeDto = new CreateOrEditOfficialTaskTypeDto();



    constructor(
        injector: Injector,
        private _officialTaskTypesServiceProxy: OfficialTaskTypesServiceProxy
    ) {
        super(injector);
    }

    show(officialTaskTypeId?: number): void {

        if (!officialTaskTypeId) {
            this.officialTaskType = new CreateOrEditOfficialTaskTypeDto();
            this.officialTaskType.id = officialTaskTypeId;

            this.active = true;
            this.modal.show();
        } else {
            this._officialTaskTypesServiceProxy.getOfficialTaskTypeForEdit(officialTaskTypeId).subscribe(result => {
                this.officialTaskType = result.officialTaskType;


                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
            this.saving = true;


            this._officialTaskTypesServiceProxy.createOrEdit(this.officialTaskType)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }



    checkBoxChanged(event) : void{
        if(event.target.checked){
            switch (event.target.name) {
                case 'TypeIn':
                    this.officialTaskType.typeIn = true;
                    this.officialTaskType.typeOut = false;
                    this.officialTaskType.typeInOut = false;
                    break;
                case 'TypeOut':
                        this.officialTaskType.typeIn = false;
                        this.officialTaskType.typeOut = true;
                        this.officialTaskType.typeInOut = false;
                    break;
                case 'TypeInOut':
                        this.officialTaskType.typeIn = false;
                        this.officialTaskType.typeOut = false;
                        this.officialTaskType.typeInOut = true;
                    break;
                default:
                    break;
            }
        }
        console.log(event);
    }



    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
