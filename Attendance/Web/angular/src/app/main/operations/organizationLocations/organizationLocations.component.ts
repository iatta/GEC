import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { OrganizationLocationsServiceProxy, OrganizationLocationDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditOrganizationLocationModalComponent } from './create-or-edit-organizationLocation-modal.component';
import { ViewOrganizationLocationModalComponent } from './view-organizationLocation-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { EntityTypeHistoryModalComponent } from '@app/shared/common/entityHistory/entity-type-history-modal.component';
import * as _ from 'lodash';
import * as moment from 'moment';

@Component({
    templateUrl: './organizationLocations.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class OrganizationLocationsComponent extends AppComponentBase {

    @ViewChild('createOrEditOrganizationLocationModal', { static: true }) createOrEditOrganizationLocationModal: CreateOrEditOrganizationLocationModalComponent;
    @ViewChild('viewOrganizationLocationModalComponent', { static: true }) viewOrganizationLocationModal: ViewOrganizationLocationModalComponent;
    @ViewChild('entityTypeHistoryModal', { static: true }) entityTypeHistoryModal: EntityTypeHistoryModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
        organizationUnitDisplayNameFilter = '';
        locationTitleEnFilter = '';


    _entityTypeFullName = 'Pixel.Attendance.Operations.OrganizationLocation';
    entityHistoryEnabled = false;

    constructor(
        injector: Injector,
        private _organizationLocationsServiceProxy: OrganizationLocationsServiceProxy,
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

    getOrganizationLocations(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._organizationLocationsServiceProxy.getAll(
            this.filterText,
            this.organizationUnitDisplayNameFilter,
            this.locationTitleEnFilter,
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

    createOrganizationLocation(): void {
        this.createOrEditOrganizationLocationModal.show();
    }

    showHistory(organizationLocation: OrganizationLocationDto): void {
        this.entityTypeHistoryModal.show({
            entityId: organizationLocation.id.toString(),
            entityTypeFullName: this._entityTypeFullName,
            entityTypeDescription: ''
        });
    }

    deleteOrganizationLocation(organizationLocation: OrganizationLocationDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._organizationLocationsServiceProxy.delete(organizationLocation.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._organizationLocationsServiceProxy.getOrganizationLocationsToExcel(
        this.filterText,
            this.organizationUnitDisplayNameFilter,
            this.locationTitleEnFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
}
