import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TimeProfileDetailsServiceProxy, TimeProfileDetailDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditTimeProfileDetailModalComponent } from './create-or-edit-timeProfileDetail-modal.component';
import { ViewTimeProfileDetailModalComponent } from './view-timeProfileDetail-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';

@Component({
    templateUrl: './timeProfileDetails.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class TimeProfileDetailsComponent extends AppComponentBase {

    @ViewChild('createOrEditTimeProfileDetailModal', { static: true }) createOrEditTimeProfileDetailModal: CreateOrEditTimeProfileDetailModalComponent;
    @ViewChild('viewTimeProfileDetailModalComponent', { static: true }) viewTimeProfileDetailModal: ViewTimeProfileDetailModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
        timeProfileDescriptionArFilter = '';
        shiftNameArFilter = '';




    constructor(
        injector: Injector,
        private _timeProfileDetailsServiceProxy: TimeProfileDetailsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    getTimeProfileDetails(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._timeProfileDetailsServiceProxy.getAll(
            this.filterText,
            this.timeProfileDescriptionArFilter,
            this.shiftNameArFilter,
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

    createTimeProfileDetail(): void {
        this.createOrEditTimeProfileDetailModal.show();
    }

    deleteTimeProfileDetail(timeProfileDetail: TimeProfileDetailDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._timeProfileDetailsServiceProxy.delete(timeProfileDetail.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._timeProfileDetailsServiceProxy.getTimeProfileDetailsToExcel(
        this.filterText,
            this.timeProfileDescriptionArFilter,
            this.shiftNameArFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
}
