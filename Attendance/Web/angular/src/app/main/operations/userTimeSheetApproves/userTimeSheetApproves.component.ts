import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { UserTimeSheetApprovesServiceProxy, UserTimeSheetApproveDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditUserTimeSheetApproveModalComponent } from './create-or-edit-userTimeSheetApprove-modal.component';
import { ViewUserTimeSheetApproveModalComponent } from './view-userTimeSheetApprove-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { EntityTypeHistoryModalComponent } from '@app/shared/common/entityHistory/entity-type-history-modal.component';
import * as _ from 'lodash';
import * as moment from 'moment';

@Component({
    templateUrl: './userTimeSheetApproves.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class UserTimeSheetApprovesComponent extends AppComponentBase {

    @ViewChild('createOrEditUserTimeSheetApproveModal', { static: true }) createOrEditUserTimeSheetApproveModal: CreateOrEditUserTimeSheetApproveModalComponent;
    @ViewChild('viewUserTimeSheetApproveModalComponent', { static: true }) viewUserTimeSheetApproveModal: ViewUserTimeSheetApproveModalComponent;
    @ViewChild('entityTypeHistoryModal', { static: true }) entityTypeHistoryModal: EntityTypeHistoryModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    maxMonthFilter : number;
		maxMonthFilterEmpty : number;
		minMonthFilter : number;
		minMonthFilterEmpty : number;
    maxYearFilter : number;
		maxYearFilterEmpty : number;
		minYearFilter : number;
		minYearFilterEmpty : number;
    maxFromDateFilter : moment.Moment;
		minFromDateFilter : moment.Moment;
    maxToDateFilter : moment.Moment;
		minToDateFilter : moment.Moment;
    approvedUnitsFilter = '';
    projectManagerApproveFilter = -1;
    isClosedFilter = -1;
        userNameFilter = '';
        userName2Filter = '';
        projectNameEnFilter = '';


    _entityTypeFullName = 'Pixel.Attendance.Operations.UserTimeSheetApprove';
    entityHistoryEnabled = false;

    constructor(
        injector: Injector,
        private _userTimeSheetApprovesServiceProxy: UserTimeSheetApprovesServiceProxy,
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

    getUserTimeSheetApproves(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._userTimeSheetApprovesServiceProxy.getAll(
            this.filterText,
            this.maxMonthFilter == null ? this.maxMonthFilterEmpty: this.maxMonthFilter,
            this.minMonthFilter == null ? this.minMonthFilterEmpty: this.minMonthFilter,
            this.maxYearFilter == null ? this.maxYearFilterEmpty: this.maxYearFilter,
            this.minYearFilter == null ? this.minYearFilterEmpty: this.minYearFilter,
            this.maxFromDateFilter,
            this.minFromDateFilter,
            this.maxToDateFilter,
            this.minToDateFilter,
            this.approvedUnitsFilter,
            this.projectManagerApproveFilter,
            this.isClosedFilter,
            this.userNameFilter,
            this.userName2Filter,
            this.projectNameEnFilter,
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

    createUserTimeSheetApprove(): void {
        this.createOrEditUserTimeSheetApproveModal.show();
    }

    showHistory(userTimeSheetApprove: UserTimeSheetApproveDto): void {
        this.entityTypeHistoryModal.show({
            entityId: userTimeSheetApprove.id.toString(),
            entityTypeFullName: this._entityTypeFullName,
            entityTypeDescription: ''
        });
    }

    deleteUserTimeSheetApprove(userTimeSheetApprove: UserTimeSheetApproveDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._userTimeSheetApprovesServiceProxy.delete(userTimeSheetApprove.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._userTimeSheetApprovesServiceProxy.getUserTimeSheetApprovesToExcel(
        this.filterText,
            this.maxMonthFilter == null ? this.maxMonthFilterEmpty: this.maxMonthFilter,
            this.minMonthFilter == null ? this.minMonthFilterEmpty: this.minMonthFilter,
            this.maxYearFilter == null ? this.maxYearFilterEmpty: this.maxYearFilter,
            this.minYearFilter == null ? this.minYearFilterEmpty: this.minYearFilter,
            this.maxFromDateFilter,
            this.minFromDateFilter,
            this.maxToDateFilter,
            this.minToDateFilter,
            this.approvedUnitsFilter,
            this.projectManagerApproveFilter,
            this.isClosedFilter,
            this.userNameFilter,
            this.userName2Filter,
            this.projectNameEnFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
}
