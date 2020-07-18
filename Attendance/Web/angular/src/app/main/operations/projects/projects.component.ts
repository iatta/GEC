import { AssignProjectUserLookupTableModalComponent } from './assign-project-user-lookup-table-modal.component';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ProjectsServiceProxy, ProjectDto, GetProjectForViewDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditProjectModalComponent } from './create-or-edit-project-modal.component';
import { ViewProjectModalComponent } from './view-project-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { EntityTypeHistoryModalComponent } from '@app/shared/common/entityHistory/entity-type-history-modal.component';
import * as _ from 'lodash';
import * as moment from 'moment';
import { AssignProjectMachineLookupTableModalComponent } from './assign-project-machine-lookup-table-modal.component';

@Component({
    templateUrl: './projects.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class ProjectsComponent extends AppComponentBase {

    @ViewChild('assignProjectUserLookupTableModal', { static: true }) assignProjectUserLookupTableModal: AssignProjectUserLookupTableModalComponent;
    @ViewChild('assignProjectMachinesLookupTableModal', { static: true }) assignProjectMachinesLookupTableModal: AssignProjectMachineLookupTableModalComponent;
    @ViewChild('createOrEditProjectModal', { static: true }) createOrEditProjectModal: CreateOrEditProjectModalComponent;
    @ViewChild('viewProjectModalComponent', { static: true }) viewProjectModal: ViewProjectModalComponent;
    @ViewChild('entityTypeHistoryModal', { static: true }) entityTypeHistoryModal: EntityTypeHistoryModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    nameArFilter = '';
    nameEnFilter = '';
        userNameFilter = '';
        locationTitleEnFilter = '';
        organizationUnitDisplayNameFilter = '';


    _entityTypeFullName = 'Pixel.Attendance.Operations.Project';
    entityHistoryEnabled = false;

    constructor(
        injector: Injector,
        private _projectsServiceProxy: ProjectsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.entityHistoryEnabled = this.setIsEntityHistoryEnabled();
    }

    private setIsEntityHistoryEnabled(): boolean {
        let customSettings = (abp as any).custom;
        return customSettings.EntityHistory && customSettings.EntityHistory.isEnabled && _.filter(customSettings.EntityHistory.enabledEntities, entityType => entityType === this._entityTypeFullName).length === 1;
    }

    getProjects(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._projectsServiceProxy.getAll(
            this.filterText,
            this.nameArFilter,
            this.nameEnFilter,
            this.userNameFilter,
            this.locationTitleEnFilter,
            this.organizationUnitDisplayNameFilter,
            this.primengTableHelper.getSorting(this.dataTable),
            this.primengTableHelper.getSkipCount(this.paginator, event),
            this.primengTableHelper.getMaxResultCount(this.paginator, event)
        ).subscribe(result => {
            this.primengTableHelper.totalRecordsCount = result.totalCount;
            this.primengTableHelper.records = result.items;
            this.primengTableHelper.hideLoadingIndicator();
        });
    }

    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    createProject(): void {
        this.createOrEditProjectModal.show();
    }

    showHistory(project: ProjectDto): void {
        this.entityTypeHistoryModal.show({
            entityId: project.id.toString(),
            entityTypeFullName: this._entityTypeFullName,
            entityTypeDescription: ''
        });
    }

    deleteProject(project: ProjectDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._projectsServiceProxy.delete(project.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    openAssignProjectUsers(projectView:GetProjectForViewDto):void{
        this.assignProjectUserLookupTableModal.show(projectView.project.id);
    }


    openAssignProjectMachines(projectView:GetProjectForViewDto):void{
        this.assignProjectMachinesLookupTableModal.show(projectView.project.id);
    }
    exportToExcel(): void {
        this._projectsServiceProxy.getProjectsToExcel(
        this.filterText,
            this.nameArFilter,
            this.nameEnFilter,
            this.userNameFilter,
            this.locationTitleEnFilter,
            this.organizationUnitDisplayNameFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
}
