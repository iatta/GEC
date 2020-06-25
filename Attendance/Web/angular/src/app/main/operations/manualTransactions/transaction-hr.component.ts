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
import { EditTimeTransactionModalComponent } from './edit-transaction-time-modal.component';

@Component({
    templateUrl: './transaction-hr.component.html',
    animations: [rowsAnimation],
    styleUrls:['../../operations/userShifts/manager-user-shift.component.less']
})
export class HrTransactionsComponent extends AppComponentBase implements OnInit {

    @ViewChild('editTimeTransactionModal', { static: true }) editTimeTransactionModal: EditTimeTransactionModalComponent;

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
    totalUserCount = 0;
    totalOverTime = 0;
    totalOverTimeString='';


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
        this._projectServiceProxy.getAllFlatForHr().subscribe((result)=>{
            debugger
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
                if(this.data.items.length){
                    //   this.totalUserCount =  this.data.items[0].totalUserCount;
                      this.totalOverTime =  this.data.items[0].totalOverTime;
                        let hours = Math.floor(this.totalOverTime  / 60);
                        let minutes = this.totalOverTime  % 60;
                        this.totalOverTimeString =  hours + ' hour/s  ' + minutes + ' minute/s';
                    }
            });
        }

    }

    updateSingleTransaction(getTransactionForViewDto: GetTransactionForViewDto,value:boolean){
        debugger
        getTransactionForViewDto.transaction.hrApprove = value;
        this._transactionsServiceProxy.updateSingleTransaction(getTransactionForViewDto).subscribe((result) => {
            console.log(result);
        });

    }

    BulkUpdateTransaction(){
        if(this.selectedTransactions.length == 0){
            this.selectedTransactions = this.data.items;
        }

        this.selectedTransactions.forEach(element => {
            if(element.transaction.projectManagerApprove && element.transaction.unitManagerApprove)
                element.transaction.hrApprove = true;
        });

        this._transactionsServiceProxy.bulkUpdateTransactions(this.selectedTransactions).subscribe((result)=>{
            console.log(result);
        })
    }

    edit(getTransactionForViewDto: GetTransactionForViewDto){
        this.editTimeTransactionModal.show(getTransactionForViewDto.transaction);
    }
    editTransaction(){
        this.loading = true;
        let modalTransaction = this.editTimeTransactionModal.manualTransaction;

        let index = this.data.items.findIndex(x => x.transaction.id == modalTransaction.id);
        if(index > -1 ){
            this.data.items[index].transaction.time = modalTransaction.time;
            this._transactionsServiceProxy.updateSingleTransaction(this.data.items[index]).subscribe((result) => {
                this.notify.success('Transaction updated successfully ');
                console.log(result);

                this.recalculate(this.data.items[index]);
            });
        }
    }


    recalculate(getTransactionForViewDto: GetTransactionForViewDto){
        this.data.items[0].totalOverTime = 0;
        let minutes = (parseInt(getTransactionForViewDto.transaction.time.split(':')[0]) * 60) + parseInt(getTransactionForViewDto.transaction.time.split(':')[1]);
        if(getTransactionForViewDto.transaction.keyType == 1){

            if(minutes > getTransactionForViewDto.timeIn)
                getTransactionForViewDto.attendance_LateIn = minutes - getTransactionForViewDto.timeIn;

        }

        if(getTransactionForViewDto.transaction.keyType == 2){
            if (minutes > getTransactionForViewDto.timeOut)
            {
                getTransactionForViewDto.overtime = minutes -  getTransactionForViewDto.timeOut;
                this.data.items[0].totalOverTime += minutes - getTransactionForViewDto.timeOut;
            }

            else
            getTransactionForViewDto.attendance_EarlyOut = getTransactionForViewDto.timeOut - minutes;
        }

        this.totalOverTime = 0;
        this.data.items.forEach(element => {
            this.totalOverTime += element.overtime;
        });

        let hours = Math.floor(this.totalOverTime  / 60);
        let otminutes = this.totalOverTime % 60;
        this.totalOverTimeString =  hours + ' hour/s ' + otminutes + ' minute/s';

        this.loading = false;

    }
}
