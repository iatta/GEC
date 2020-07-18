import { result } from 'lodash';
import { NotifyService } from '@abp/notify/notify.service';
import { ProjectUserInputDto, ProjectUserDto, MachinesServiceProxy, ProjectMachineDto, PagedResultDtoOfMachineLookupTableDto, ProjectMachineInputDto } from './../../../../shared/service-proxies/service-proxies';
import { Component, ViewChild, Injector, Output, EventEmitter, ViewEncapsulation} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import {ProjectsServiceProxy, ProjectUserLookupTableDto,MachineLookupTableDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { id } from '@swimlane/ngx-charts/release/utils';
import * as _ from 'lodash';

@Component({
    selector: 'assignProjectMachinesLookupTableModal',
    styleUrls: ['./project-user-lookup-table-modal.component.less'],
    encapsulation: ViewEncapsulation.None,
    templateUrl: './assign-project-machine-lookup-table-modal.component.html'
})
export class AssignProjectMachineLookupTableModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    filterText = '';
    projectId: number;
    displayName: string;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    active = false;
    saving = false;
    selectedMachines:number[] = [];
    isChecked = false;

    constructor(
        injector: Injector,
        private _machinesServiceProxy: MachinesServiceProxy,
        private _projectServiceProxy: ProjectsServiceProxy,
        private _notifyService: NotifyService
    ) {
        super(injector);
    }

    show(projectId:number): void {
        this.projectId = projectId;
        this.active = true;
        this.paginator.rows = 5;
        this.getProjectMachines();
        this.modal.show();
    }
    getProjectMachines(){

        this._projectServiceProxy.getProjecMachines(this.projectId).subscribe((result: ProjectMachineDto[])=>{
            this.selectedMachines = result.map(projectMachine => projectMachine.machineId);
            this.getAll();
        });
    }

    getAll(event?: LazyLoadEvent) {
        this.isChecked = false;
        if (!this.active) {
            return;
        }

        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();


        this._machinesServiceProxy.getAllMachinesForLookupTable(
            this.filterText,
            this.primengTableHelper.getSorting(this.dataTable),
            this.primengTableHelper.getSkipCount(this.paginator, event),
            this.primengTableHelper.getMaxResultCount(this.paginator, event)
        ).subscribe(result => {
            this.mapResult(result,this.selectedMachines);
            this.primengTableHelper.totalRecordsCount = result.totalCount;
            this.primengTableHelper.records = result.items;
            this.primengTableHelper.hideLoadingIndicator();
        });
    }

    mapResult(result: PagedResultDtoOfMachineLookupTableDto , selectedMachines:number[]):void{
            // _.forEach won't throw errors if arr is not an array...
            _.forEach(result.items, function (obj) {
                // _.set won't throw errors if obj is not an object. With more complex objects, if a portion of the path doesn't exist, _.set creates it
                if(selectedMachines.findIndex(x => x == obj.id) > -1){
                    _.set(obj, 'isSelected', true);
                }
                console.log(result);
            });
    }

    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    setAndSave() {
        const input = new ProjectMachineInputDto();
        input.projectId = this.projectId;
        input.projectMachines = [];
        this.selectedMachines.forEach(element => {
            let projectUserToAdd = new ProjectMachineDto();
            projectUserToAdd.machineId = element;
            projectUserToAdd.projectId = this.projectId;
            input.projectMachines.push(projectUserToAdd);
        });

        this._projectServiceProxy.updateProjecMachines(input).subscribe((result)=>{
            this.notify.success(this.l('SuccessfullyDeleted'));
            this.selectedMachines=[];
            this.active = false;
            this.modal.hide();
        });
        this.active = false;
        this.modal.hide();
        this.modalSave.emit(null);
    }

    selectAll(event:any){
        console.log(event);
        if (event === 'T') {
            this.primengTableHelper.records.forEach(element => {
                element.isSelected= true;
                let index = this.selectedMachines.findIndex(x => x === element.id);
                if(index === -1)
                    this.selectedMachines.push(element.id);
            });
        } else {
            this.primengTableHelper.records.forEach(element => {
                element.isSelected= false;
                let index = this.selectedMachines.findIndex(x => x === element.id);
                if(index > -1 )
                    this.selectedMachines.splice(index , 1);
            });

        }
    }

    recordChange( record: MachineLookupTableDto){
        debugger
        let index = this.selectedMachines.findIndex(x => x === record.id);
        if (record.isSelected) {
            if(index === -1) {
                this.selectedMachines.push(record.id);
            }
        } else {
            if (index > -1) {
                this.selectedMachines.splice(index, 1);
            }
        }
    }

    close(): void {

        this.active = false;
        this.modal.hide();
        this.modalSave.emit(null);
    }
}
