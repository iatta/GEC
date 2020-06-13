import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TransServiceProxy, TranDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditTranModalComponent } from './create-or-edit-tran-modal.component';
import { ViewTranModalComponent } from './view-tran-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';

@Component({
    templateUrl: './trans.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class TransComponent extends AppComponentBase {

    @ViewChild('createOrEditTranModal', { static: true }) createOrEditTranModal: CreateOrEditTranModalComponent;
    @ViewChild('viewTranModalComponent', { static: true }) viewTranModal: ViewTranModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    scan1Filter = '';
    scan2Filter = '';
    scan3Filter = '';
    scan4Filter = '';
    scan5Filter = '';
    scan6Filter = '';
    scan8Filter = '';
    scanLocation1Filter = '';
    scanLocation2Filter = '';
    scanLocation3Filter = '';
    scanLocation4Filter = '';
    scanLocation5Filter = '';
    scanLocation6Filter = '';
    scanLocation7Filter = '';
    scanLocation8Filter = '';
    hasHolidayFilter = -1;
    hasVacationFilter = -1;
    hasOffDayFilter = -1;
    isAbsentFilter = -1;
    leaveCodeFilter = '';
    maxDesignationIDFilter : number;
		maxDesignationIDFilterEmpty : number;
		minDesignationIDFilter : number;
		minDesignationIDFilterEmpty : number;
    leaveRemarkFilter = '';
    maxNoShiftsFilter : number;
		maxNoShiftsFilterEmpty : number;
		minNoShiftsFilter : number;
		minNoShiftsFilterEmpty : number;
    shiftNameFilter = '';
    scanManual1Filter = -1;
    scanManual2Filter = -1;
    scanManual3Filter = -1;
    scanManual4Filter = -1;
    scanManual5Filter = -1;
    scanManual6Filter = -1;
    scanManual7Filter = -1;
    scanManual8Filter = -1;
        userNameFilter = '';




    constructor(
        injector: Injector,
        private _transServiceProxy: TransServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    getTrans(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._transServiceProxy.getAll(
            this.filterText,
            this.scan1Filter,
            this.scan2Filter,
            this.scan3Filter,
            this.scan4Filter,
            this.scan5Filter,
            this.scan6Filter,
            this.scan8Filter,
            this.scanLocation1Filter,
            this.scanLocation2Filter,
            this.scanLocation3Filter,
            this.scanLocation4Filter,
            this.scanLocation5Filter,
            this.scanLocation6Filter,
            this.scanLocation7Filter,
            this.scanLocation8Filter,
            this.hasHolidayFilter,
            this.hasVacationFilter,
            this.hasOffDayFilter,
            this.isAbsentFilter,
            this.leaveCodeFilter,
            this.maxDesignationIDFilter == null ? this.maxDesignationIDFilterEmpty: this.maxDesignationIDFilter,
            this.minDesignationIDFilter == null ? this.minDesignationIDFilterEmpty: this.minDesignationIDFilter,
            this.leaveRemarkFilter,
            this.maxNoShiftsFilter == null ? this.maxNoShiftsFilterEmpty: this.maxNoShiftsFilter,
            this.minNoShiftsFilter == null ? this.minNoShiftsFilterEmpty: this.minNoShiftsFilter,
            this.shiftNameFilter,
            this.scanManual1Filter,
            this.scanManual2Filter,
            this.scanManual3Filter,
            this.scanManual4Filter,
            this.scanManual5Filter,
            this.scanManual6Filter,
            this.scanManual7Filter,
            this.scanManual8Filter,
            this.userNameFilter,
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

    createTran(): void {
        this.createOrEditTranModal.show();
    }

    deleteTran(tran: TranDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._transServiceProxy.delete(tran.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._transServiceProxy.getTransToExcel(
        this.filterText,
            this.scan1Filter,
            this.scan2Filter,
            this.scan3Filter,
            this.scan4Filter,
            this.scan5Filter,
            this.scan6Filter,
            this.scan8Filter,
            this.scanLocation1Filter,
            this.scanLocation2Filter,
            this.scanLocation3Filter,
            this.scanLocation4Filter,
            this.scanLocation5Filter,
            this.scanLocation6Filter,
            this.scanLocation7Filter,
            this.scanLocation8Filter,
            this.hasHolidayFilter,
            this.hasVacationFilter,
            this.hasOffDayFilter,
            this.isAbsentFilter,
            this.leaveCodeFilter,
            this.maxDesignationIDFilter == null ? this.maxDesignationIDFilterEmpty: this.maxDesignationIDFilter,
            this.minDesignationIDFilter == null ? this.minDesignationIDFilterEmpty: this.minDesignationIDFilter,
            this.leaveRemarkFilter,
            this.maxNoShiftsFilter == null ? this.maxNoShiftsFilterEmpty: this.maxNoShiftsFilter,
            this.minNoShiftsFilter == null ? this.minNoShiftsFilterEmpty: this.minNoShiftsFilter,
            this.shiftNameFilter,
            this.scanManual1Filter,
            this.scanManual2Filter,
            this.scanManual3Filter,
            this.scanManual4Filter,
            this.scanManual5Filter,
            this.scanManual6Filter,
            this.scanManual7Filter,
            this.scanManual8Filter,
            this.userNameFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
}
