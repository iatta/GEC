import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ShiftTypeDetailsServiceProxy, ShiftTypeDetailDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditShiftTypeDetailModalComponent } from './create-or-edit-shiftTypeDetail-modal.component';
import { ViewShiftTypeDetailModalComponent } from './view-shiftTypeDetail-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';

@Component({
    templateUrl: './shiftTypeDetails.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class ShiftTypeDetailsComponent extends AppComponentBase {

    @ViewChild('createOrEditShiftTypeDetailModal', { static: true }) createOrEditShiftTypeDetailModal: CreateOrEditShiftTypeDetailModalComponent;
    @ViewChild('viewShiftTypeDetailModalComponent', { static: true }) viewShiftTypeDetailModal: ViewShiftTypeDetailModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
        shiftTypeDescriptionArFilter = '';




    constructor(
        injector: Injector,
        private _shiftTypeDetailsServiceProxy: ShiftTypeDetailsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    getShiftTypeDetails(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._shiftTypeDetailsServiceProxy.getAll(
            this.filterText,
            this.shiftTypeDescriptionArFilter,
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

    createShiftTypeDetail(): void {
        this.createOrEditShiftTypeDetailModal.show();
    }

    deleteShiftTypeDetail(shiftTypeDetail: ShiftTypeDetailDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._shiftTypeDetailsServiceProxy.delete(shiftTypeDetail.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._shiftTypeDetailsServiceProxy.getShiftTypeDetailsToExcel(
        this.filterText,
            this.shiftTypeDescriptionArFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
}
