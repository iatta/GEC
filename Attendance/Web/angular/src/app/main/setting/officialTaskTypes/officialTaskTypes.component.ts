import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { OfficialTaskTypesServiceProxy, OfficialTaskTypeDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditOfficialTaskTypeModalComponent } from './create-or-edit-officialTaskType-modal.component';
import { ViewOfficialTaskTypeModalComponent } from './view-officialTaskType-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';

@Component({
    templateUrl: './officialTaskTypes.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class OfficialTaskTypesComponent extends AppComponentBase {

    @ViewChild('createOrEditOfficialTaskTypeModal', { static: true }) createOrEditOfficialTaskTypeModal: CreateOrEditOfficialTaskTypeModalComponent;
    @ViewChild('viewOfficialTaskTypeModalComponent', { static: true }) viewOfficialTaskTypeModal: ViewOfficialTaskTypeModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    nameArFilter = '';
    nameEnFilter = '';
    typeInFilter = -1;
    typeOutFilter = -1;
    typeInOutFilter = -1;




    constructor(
        injector: Injector,
        private _officialTaskTypesServiceProxy: OfficialTaskTypesServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    getOfficialTaskTypes(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._officialTaskTypesServiceProxy.getAll(
            this.filterText,
            this.nameArFilter,
            this.nameEnFilter,
            this.typeInFilter,
            this.typeOutFilter,
            this.typeInOutFilter,
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

    createOfficialTaskType(): void {
        this.createOrEditOfficialTaskTypeModal.show();
    }

    deleteOfficialTaskType(officialTaskType: OfficialTaskTypeDto): void {
        this.message.confirm(
            '',
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._officialTaskTypesServiceProxy.delete(officialTaskType.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._officialTaskTypesServiceProxy.getOfficialTaskTypesToExcel(
        this.filterText,
            this.nameArFilter,
            this.nameEnFilter,
            this.typeInFilter,
            this.typeOutFilter,
            this.typeInOutFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
}
