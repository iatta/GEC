import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ShiftsServiceProxy, ShiftDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditShiftModalComponent } from './create-or-edit-shift-modal.component';
import { ViewShiftModalComponent } from './view-shift-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';
import { Router } from '@angular/router';
import { trigger,state,style,transition,animate } from '@angular/animations';


@Component({
    templateUrl: './shifts.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation(), trigger('rowExpansionTrigger', [
            state('void', style({
                transform: 'translateX(-10%)',
                opacity: 0
            })),
            state('active', style({
                transform: 'translateX(0)',
                opacity: 1
            })),
            transition('* <=> *', animate('400ms cubic-bezier(0.86, 0, 0.07, 1)'))
        ])]
})
export class ShiftsComponent extends AppComponentBase {

    @ViewChild('createOrEditShiftModal', { static: true }) createOrEditShiftModal: CreateOrEditShiftModalComponent;
    @ViewChild('viewShiftModalComponent', { static: true }) viewShiftModal: ViewShiftModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    nameArFilter = '';
    nameEnFilter = '';
    codeFilter = '';


    constructor(
        injector: Injector,
        private _shiftsServiceProxy: ShiftsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private router: Router,
    ) {
        super(injector);
    }

    getShifts(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._shiftsServiceProxy.getAll(
            this.filterText,
            this.nameArFilter,
            this.nameEnFilter,
            this.codeFilter,
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

    createShift(): void {
        //this.createOrEditShiftModal.show();
        this.router.navigate(['app/main/setting/shifts/manage']);
    }

    deleteShift(shift: ShiftDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._shiftsServiceProxy.delete(shift.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._shiftsServiceProxy.getShiftsToExcel(
        this.filterText,
            this.nameArFilter,
            this.nameEnFilter,
            this.codeFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
}
