import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { UserDelegationsServiceProxy, UserDelegationDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditUserDelegationModalComponent } from './create-or-edit-userDelegation-modal.component';
import { ViewUserDelegationModalComponent } from './view-userDelegation-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { EntityTypeHistoryModalComponent } from '@app/shared/common/entityHistory/entity-type-history-modal.component';
import * as _ from 'lodash';
import * as moment from 'moment';

@Component({
    templateUrl: './userDelegations.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class UserDelegationsComponent extends AppComponentBase {

    @ViewChild('createOrEditUserDelegationModal', { static: true }) createOrEditUserDelegationModal: CreateOrEditUserDelegationModalComponent;
    @ViewChild('viewUserDelegationModalComponent', { static: true }) viewUserDelegationModal: ViewUserDelegationModalComponent;
    @ViewChild('entityTypeHistoryModal', { static: true }) entityTypeHistoryModal: EntityTypeHistoryModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    maxFromDateFilter : moment.Moment;
		minFromDateFilter : moment.Moment;
    maxToDateFilter : moment.Moment;
		minToDateFilter : moment.Moment;
        userNameFilter = '';
        userName2Filter = '';


    _entityTypeFullName = 'Pixel.Attendance.Operations.UserDelegation';
    entityHistoryEnabled = false;

    constructor(
        injector: Injector,
        private _userDelegationsServiceProxy: UserDelegationsServiceProxy,
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

    getUserDelegations(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._userDelegationsServiceProxy.getAll(
            this.filterText,
            this.maxFromDateFilter,
            this.minFromDateFilter,
            this.maxToDateFilter,
            this.minToDateFilter,
            this.userNameFilter,
            this.userName2Filter,
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

    createUserDelegation(): void {
        this.createOrEditUserDelegationModal.show();
    }

    showHistory(userDelegation: UserDelegationDto): void {
        this.entityTypeHistoryModal.show({
            entityId: userDelegation.id.toString(),
            entityTypeFullName: this._entityTypeFullName,
            entityTypeDescription: ''
        });
    }

    deleteUserDelegation(userDelegation: UserDelegationDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._userDelegationsServiceProxy.delete(userDelegation.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._userDelegationsServiceProxy.getUserDelegationsToExcel(
        this.filterText,
            this.maxFromDateFilter,
            this.minFromDateFilter,
            this.maxToDateFilter,
            this.minToDateFilter,
            this.userNameFilter,
            this.userName2Filter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
}
