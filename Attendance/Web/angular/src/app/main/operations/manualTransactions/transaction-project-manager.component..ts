import { PagedResultDtoOfGetTransactionForViewDto } from './../../../../shared/service-proxies/service-proxies';
import { filter } from 'rxjs/operators';
import { Component, Injector, ViewEncapsulation, ViewChild, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TransactionsServiceProxy, TransactionDto, ProjectDto, ProjectsServiceProxy } from '@shared/service-proxies/service-proxies';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditManualTransactionModalComponent } from './create-or-edit-manualTransaction-modal.component';
import { ViewManualTransactionModalComponent } from './view-manualTransaction-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';
import { rowsAnimation } from '@shared/animations/template.animations';

@Component({
    templateUrl: './transaction-project-manager.component.html',
    animations: [rowsAnimation],
    styleUrls:['../../operations/userShifts/manager-user-shift.component.less']
})
export class ProjectManagerTransactionsComponent extends AppComponentBase implements OnInit {

    

    projectId:number;
    fromDate:moment.Moment = moment();
    toDate:moment.Moment =moment();
    data:PagedResultDtoOfGetTransactionForViewDto = new PagedResultDtoOfGetTransactionForViewDto();
    selectedTransactions:PagedResultDtoOfGetTransactionForViewDto[];
    projectList:ProjectDto[];
    selectedProject:ProjectDto;
    loading=false;
    saving=false;

    constructor(
        injector: Injector,
        private _transactionsServiceProxy: TransactionsServiceProxy,
        private _projectServiceProxy: ProjectsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,

    ) {
        super(injector);
    }
    ngOnInit(): void {
        this._projectServiceProxy.getAllFlatForProjectManager().subscribe((result)=>{
            this.projectList = result;
            console.log(result);
        });
    }

    getDate() {
        this._transactionsServiceProxy.getAllTransactionForProjectManager(this.selectedProject.id,this.fromDate,this.toDate).subscribe((result)=>{
            console.log(result);
            this.data = result;
        });
    }

    save(){

    }
}
