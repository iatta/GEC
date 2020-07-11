﻿import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BeaconsServiceProxy, BeaconDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditBeaconModalComponent } from './create-or-edit-beacon-modal.component';
import { ViewBeaconModalComponent } from './view-beacon-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { EntityTypeHistoryModalComponent } from '@app/shared/common/entityHistory/entity-type-history-modal.component';
import * as _ from 'lodash';
import * as moment from 'moment';

@Component({
    templateUrl: './beacons.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class BeaconsComponent extends AppComponentBase {

    @ViewChild('createOrEditBeaconModal', { static: true }) createOrEditBeaconModal: CreateOrEditBeaconModalComponent;
    @ViewChild('viewBeaconModalComponent', { static: true }) viewBeaconModal: ViewBeaconModalComponent;
    @ViewChild('entityTypeHistoryModal', { static: true }) entityTypeHistoryModal: EntityTypeHistoryModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    nameFilter = '';
    uidFilter = '';
    maxMinorFilter : number;
		maxMinorFilterEmpty : number;
		minMinorFilter : number;
		minMinorFilterEmpty : number;
    maxMajorFilter : number;
		maxMajorFilterEmpty : number;
		minMajorFilter : number;
		minMajorFilterEmpty : number;


    _entityTypeFullName = 'Pixel.Attendance.Operations.Beacon';
    entityHistoryEnabled = false;

    constructor(
        injector: Injector,
        private _beaconsServiceProxy: BeaconsServiceProxy,
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

    getBeacons(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._beaconsServiceProxy.getAll(
            this.filterText,
            this.nameFilter,
            this.uidFilter,
            this.maxMinorFilter == null ? this.maxMinorFilterEmpty: this.maxMinorFilter,
            this.minMinorFilter == null ? this.minMinorFilterEmpty: this.minMinorFilter,
            this.maxMajorFilter == null ? this.maxMajorFilterEmpty: this.maxMajorFilter,
            this.minMajorFilter == null ? this.minMajorFilterEmpty: this.minMajorFilter,
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

    createBeacon(): void {
        this.createOrEditBeaconModal.show();
    }

    showHistory(beacon: BeaconDto): void {
        this.entityTypeHistoryModal.show({
            entityId: beacon.id.toString(),
            entityTypeFullName: this._entityTypeFullName,
            entityTypeDescription: ''
        });
    }

    deleteBeacon(beacon: BeaconDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._beaconsServiceProxy.delete(beacon.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._beaconsServiceProxy.getBeaconsToExcel(
        this.filterText,
            this.nameFilter,
            this.uidFilter,
            this.maxMinorFilter == null ? this.maxMinorFilterEmpty: this.maxMinorFilter,
            this.minMinorFilter == null ? this.minMinorFilterEmpty: this.minMinorFilter,
            this.maxMajorFilter == null ? this.maxMajorFilterEmpty: this.maxMajorFilter,
            this.minMajorFilter == null ? this.minMajorFilterEmpty: this.minMajorFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
}
