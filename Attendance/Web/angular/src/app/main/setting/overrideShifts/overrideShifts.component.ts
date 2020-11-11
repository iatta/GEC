import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { OverrideShiftsServiceProxy, OverrideShiftDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditOverrideShiftModalComponent } from './create-or-edit-overrideShift-modal.component';
import { ViewOverrideShiftModalComponent } from './view-overrideShift-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { EntityTypeHistoryModalComponent } from '@app/shared/common/entityHistory/entity-type-history-modal.component';
import * as _ from 'lodash';
import * as moment from 'moment';

@Component({
    templateUrl: './overrideShifts.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class OverrideShiftsComponent extends AppComponentBase {

    @ViewChild('createOrEditOverrideShiftModal', { static: true }) createOrEditOverrideShiftModal: CreateOrEditOverrideShiftModalComponent;
    @ViewChild('viewOverrideShiftModalComponent', { static: true }) viewOverrideShiftModal: ViewOverrideShiftModalComponent;
    @ViewChild('entityTypeHistoryModal', { static: true }) entityTypeHistoryModal: EntityTypeHistoryModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    maxDayFilter : moment.Moment;
		minDayFilter : moment.Moment;
        userNameFilter = '';
        shiftNameEnFilter = '';


    _entityTypeFullName = 'Pixel.Attendance.Setting.OverrideShift';
    entityHistoryEnabled = false;

    constructor(
        injector: Injector,
        private _overrideShiftsServiceProxy: OverrideShiftsServiceProxy,
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

    getOverrideShifts(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._overrideShiftsServiceProxy.getAll(
            this.filterText,
            this.maxDayFilter,
            this.minDayFilter,
            this.userNameFilter,
            this.shiftNameEnFilter,
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

    createOverrideShift(): void {
        this.createOrEditOverrideShiftModal.show();
    }

    showHistory(overrideShift: OverrideShiftDto): void {
        this.entityTypeHistoryModal.show({
            entityId: overrideShift.id.toString(),
            entityTypeFullName: this._entityTypeFullName,
            entityTypeDescription: ''
        });
    }

    deleteOverrideShift(overrideShift: OverrideShiftDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._overrideShiftsServiceProxy.delete(overrideShift.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._overrideShiftsServiceProxy.getOverrideShiftsToExcel(
        this.filterText,
            this.maxDayFilter,
            this.minDayFilter,
            this.userNameFilter,
            this.shiftNameEnFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
}
