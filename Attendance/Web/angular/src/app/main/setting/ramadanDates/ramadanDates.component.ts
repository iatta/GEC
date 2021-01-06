import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { RamadanDatesServiceProxy, RamadanDateDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditRamadanDateModalComponent } from './create-or-edit-ramadanDate-modal.component';
import { ViewRamadanDateModalComponent } from './view-ramadanDate-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { EntityTypeHistoryModalComponent } from '@app/shared/common/entityHistory/entity-type-history-modal.component';
import * as _ from 'lodash';
import * as moment from 'moment';

@Component({
    templateUrl: './ramadanDates.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class RamadanDatesComponent extends AppComponentBase {

    @ViewChild('createOrEditRamadanDateModal', { static: true }) createOrEditRamadanDateModal: CreateOrEditRamadanDateModalComponent;
    @ViewChild('viewRamadanDateModalComponent', { static: true }) viewRamadanDateModal: ViewRamadanDateModalComponent;
    @ViewChild('entityTypeHistoryModal', { static: true }) entityTypeHistoryModal: EntityTypeHistoryModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    yearFilter = '';
    maxFromDateFilter : moment.Moment;
		minFromDateFilter : moment.Moment;
    maxToDateFilter : moment.Moment;
		minToDateFilter : moment.Moment;


    _entityTypeFullName = 'Pixel.Attendance.Setting.RamadanDate';
    entityHistoryEnabled = false;

    constructor(
        injector: Injector,
        private _ramadanDatesServiceProxy: RamadanDatesServiceProxy,
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

    getRamadanDates(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._ramadanDatesServiceProxy.getAll(
            this.filterText,
            this.yearFilter,
            this.maxFromDateFilter,
            this.minFromDateFilter,
            this.maxToDateFilter,
            this.minToDateFilter,
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

    createRamadanDate(): void {
        this.createOrEditRamadanDateModal.show();
    }

    showHistory(ramadanDate: RamadanDateDto): void {
        this.entityTypeHistoryModal.show({
            entityId: ramadanDate.id.toString(),
            entityTypeFullName: this._entityTypeFullName,
            entityTypeDescription: ''
        });
    }

    deleteRamadanDate(ramadanDate: RamadanDateDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._ramadanDatesServiceProxy.delete(ramadanDate.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._ramadanDatesServiceProxy.getRamadanDatesToExcel(
        this.filterText,
            this.yearFilter,
            this.maxFromDateFilter,
            this.minFromDateFilter,
            this.maxToDateFilter,
            this.minToDateFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
}
