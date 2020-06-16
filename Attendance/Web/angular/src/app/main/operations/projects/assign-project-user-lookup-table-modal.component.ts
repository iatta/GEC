import { NotifyService } from '@abp/notify/notify.service';
import { ProjectUserInputDto, ProjectUserDto } from './../../../../shared/service-proxies/service-proxies';
import { Component, ViewChild, Injector, Output, EventEmitter, ViewEncapsulation} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import {ProjectsServiceProxy, ProjectUserLookupTableDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';

@Component({
    selector: 'assignProjectUserLookupTableModal',
    styleUrls: ['./project-user-lookup-table-modal.component.less'],
    encapsulation: ViewEncapsulation.None,
    templateUrl: './assign-project-user-lookup-table-modal.component.html'
})
export class AssignProjectUserLookupTableModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    filterText = '';
    projectId: number;
    displayName: string;
    
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    active = false;
    saving = false;
    selectedUsers:number[];

    constructor(
        injector: Injector,
        private _projectsServiceProxy: ProjectsServiceProxy,
        private _notifyService: NotifyService
    ) {
        super(injector);
    }

    show(projectId:number): void {
        this.projectId = projectId;
        this.active = true;
        this.paginator.rows = 5;
        this.getAll();
        this.modal.show();
    }

    getAll(event?: LazyLoadEvent) {
        if (!this.active) {
            return;
        }

        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._projectsServiceProxy.getAllUserForLookupTable(
            this.filterText,
            this.primengTableHelper.getSorting(this.dataTable),
            this.primengTableHelper.getSkipCount(this.paginator, event),
            this.primengTableHelper.getMaxResultCount(this.paginator, event)
        ).subscribe(result => {
            this.primengTableHelper.totalRecordsCount = result.totalCount;
            this.primengTableHelper.records = result.items;
            this.primengTableHelper.hideLoadingIndicator();
            this.getProjectUsers();
        });
    }

    getProjectUsers():void{
        this._projectsServiceProxy.getProjectUsers(this.projectId).subscribe((result)=>{
            this.selectedUsers = [];
            result.forEach(element => {
                this.selectedUsers.push(element.userId);
            });
        })
    }
    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    setAndSave() {
        const input = new ProjectUserInputDto();
        input.projectId = this.projectId;
        input.projectUsers = [];
        this.selectedUsers.forEach(element => {
            let projectUserToAdd = new ProjectUserDto();
            projectUserToAdd.userId = element;
            projectUserToAdd.projectId = this.projectId;
            input.projectUsers.push(projectUserToAdd);
        });

        this._projectsServiceProxy.updateProjectUsers(input).subscribe((result)=>{
            this.notify.success(this.l('SuccessfullyDeleted'));
            this.selectedUsers=[];
            this.active = false;
            this.modal.hide();
        });

        // this.id = user.id;
        // this.displayName = user.displayName;
        // this.active = false;
        // this.modal.hide();
        // this.modalSave.emit(null);
    }

    close(): void {
        this.active = false;
        this.modal.hide();
        this.modalSave.emit(null);
    }
}
