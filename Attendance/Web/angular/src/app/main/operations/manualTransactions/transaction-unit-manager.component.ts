import { PagedResultDtoOfGetTransactionForViewDto } from './../../../../shared/service-proxies/service-proxies';
import { filter } from 'rxjs/operators';
import { Component, Injector, ViewEncapsulation, ViewChild, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TransactionsServiceProxy, TransactionDto, ProjectDto, ProjectsServiceProxy, GetTransactionForViewDto } from '@shared/service-proxies/service-proxies';
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
    templateUrl: './transaction-unit-manager.component.html',
    animations: [rowsAnimation],
    styleUrls:['../../operations/userShifts/manager-user-shift.component.less']
})
export class UnitManagerTransactionsComponent extends AppComponentBase implements OnInit {

    

    projectId:number;
    fromDate:string;
    toDate:string;
    data:PagedResultDtoOfGetTransactionForViewDto = new PagedResultDtoOfGetTransactionForViewDto();
    selectedTransactions:GetTransactionForViewDto[] = [];
    projectList:ProjectDto[];
    selectedProject:ProjectDto;
    loading=false;
    saving=false;
    active = false;

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

        this.fromDate = moment().format('DD/MM/YYYY');
        this.toDate = moment().format('DD/MM/YYYY');
        this._projectServiceProxy.getAllFlatForOrganizationUnitManager().subscribe((result)=>{
            this.projectList = result;
            this.active = true;
            console.log(result);
        });
    }

    validateForm(){
        let valid = true;
        if(!this.selectedProject){
            this.message.warn('Please Select Project');
            valid = false;
            return valid;
        }
        return valid;
    }

    getDate() {
        if(this.validateForm()){
            let fromDateToPass = moment(this.fromDate,'DD/MM/YYYY');
            let toDateToPass = moment(this.toDate,'DD/MM/YYYY');
            this._transactionsServiceProxy.getAllTransactionForUnitManager(this.selectedProject.id,fromDateToPass,toDateToPass).subscribe((result)=>{
                console.log(result);
                this.data = result;
            });
        }
      
    }

    updateSingleTransaction(getTransactionForViewDto: GetTransactionForViewDto,value:boolean){
        debugger
        getTransactionForViewDto.transaction.unitManagerApprove = value;
        this._transactionsServiceProxy.updateSingleTransaction(getTransactionForViewDto).subscribe((result) => {
            console.log(result);
        });

    }

    BulkUpdateTransaction(){
        if(this.selectedTransactions.length == 0){
            this.selectedTransactions = this.data.items;
        }

        this.selectedTransactions.forEach(element => {
            element.transaction.unitManagerApprove = true;
        });

        this._transactionsServiceProxy.bulkUpdateTransactions(this.selectedTransactions).subscribe((result)=>{
            console.log(result);
        })
    }
}
