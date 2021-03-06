import { FilterUtils, SelectItem } from 'primeng/api';
import * as _ from 'lodash';
import { ActualSummerizeTimeSheetDto, ProjectManagerApproveInput, UserTimeSheetInput, ActualSummerizeTimeSheetOutput, UserFlatDto } from './../../../../shared/service-proxies/service-proxies';
import { TransactionsServiceProxy, ProjectDto, ProjectsServiceProxy } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { Component, OnInit, Injector, ViewChild } from '@angular/core';
import { Router, ActivatedRoute, ParamMap ,Params } from '@angular/router';
import * as moment from 'moment';
import { start } from 'repl';
import { rowsAnimation } from '@shared/animations/template.animations';
import { CreateOrEditAttendanceModalComponent } from '../manualTransactions/create-or-edit-attendance-modal.component';
import { TransactionInfoeModalComponent } from '../manualTransactions/transaction-info-modal.component';



@Component({
    templateUrl: './summerize.component.html',
    styleUrls:['./summerize.component.css'],
    animations: [rowsAnimation],
})


export class SummerizeReportComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditAttendanceModal', { static: true }) createOrEditAttendanceModal: CreateOrEditAttendanceModalComponent;
    @ViewChild('transactionInfoModal', { static: true }) transactionInfoModal: TransactionInfoeModalComponent;

    startDate:moment.Moment = moment().startOf('day');
    endDate:moment.Moment = moment().startOf('day');
    projectId:number;
    data:ActualSummerizeTimeSheetDto[] = [];
    dataLoaded= false;
    show = false;
    projectList:ProjectDto[];
    selectedProject:ProjectDto;
    cols: any[];
    isTrue:boolean = false;
    dataIsLoading = false;
    month:number;
    year:number;

    isMonth:boolean;
    isDateRange:boolean;
    selectedUsers:SelectItem[]=[];
    selectedUserIds:number[]=[];
    selectedDate='08/2020';
    selectedFirstDate='01/01/2020';
    selectedEndDate='01/01/2020';
    userIds:UserTimeSheetInput[] = [];
    userTypes:SelectItem[]= [];
    selectedUserType:SelectItem;
    summeryColSpan=0;
    summeryRowSpanTemp=0;
    footerCols:any[];
    response:ActualSummerizeTimeSheetOutput;
    isApproveDisable=false;

    isLoadUser:boolean;
    usersList: SelectItem[] = [];
    frozenCols:any[];
    constructor(
        injector: Injector,
        private route: ActivatedRoute,
        private _transactionService: TransactionsServiceProxy,
        private _projectServiceProxy: ProjectsServiceProxy,
        private router: Router,

    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.userTypes.push({label: this.l('All'), value: 0},{label: this.l('Staff'), value: 1},{label: this.l('Labor'), value: 2});

        this._projectServiceProxy.getAllFlatForProjectManager().subscribe((result)=>{
            this.projectList = result;
            this.show = true;
            console.log(result);
        });


    }
    onOpenCalendar(container) {
        container.monthSelectHandler = (event: any): void => {
          container._store.dispatch(container._actions.select(event.date));
        };
        container.setViewMode('month');
       }


    validateForm(){
        let valid = true;
        if(!this.selectedProject){
            this.message.warn('Please Select Project');
            valid = false;
            return valid;
        }
        if(!this.isMonth && !this.isDateRange){
            this.message.warn('Please Select Date Type');
            valid = false;
            return valid;
        }

        if(!this.selectedDate && this.isMonth){
            this.message.warn('Please Select Date');
            valid = false;
            return valid;
        }
        return valid;
    }

    generateReport(){
        if(this.validateForm()){
            debugger
            this.data = [];
            this.dataLoaded = false;
            this.selectedUserIds = _.map(this.selectedUsers, 'value');
            let date = new Date("2020-08-01");
            if(this.selectedDate != "08/2020")
                date = new Date(this.selectedDate);

            this.month = date.getMonth() + 1;
            this.year = date.getFullYear();
            this.dataIsLoading=true;
            debugger
            this._transactionService.getActualSummerizeTimeSheet(this.selectedProject.id,
                moment(this.selectedFirstDate),moment(this.selectedEndDate),this.month,
                this.year,this.selectedUserType.value,this.selectedUserIds , this.isMonth,this.isDateRange).subscribe((result) => {
               console.log(result);
                if(result.data.length > 0){
                    this.userIds = result.userIds;
                    this.cols = [];
                    this.footerCols=[];
                    this.frozenCols = [
                        { field: 'fingerCode', header: 'code' },
                        {field: 'userName', header: 'Employee' }
                    ];
                    let firstDay = new Date(this.year, this.month - 1, 1);
                    let lastDay = new Date(this.year, this.month, 0);
                    console.log(lastDay);

                    if(this.isMonth){
                        this.startDate = moment(firstDay);
                        this.endDate = moment(lastDay);
                    }else{
                        let dateStart = new Date(this.selectedFirstDate);
                        let dateEnd = new Date(this.selectedEndDate);
                        this.startDate = moment(dateStart);
                         this.endDate = moment(dateEnd);
                    }




                    for (let m = moment(this.startDate); m.diff(this.endDate, 'days') <= 0; m.add(1, 'days')) {
                        let dateHeader = m.format('DD/MM/YYYY');
                        let dateToCompare = moment(m);
                        let dateIndex = result.data[0].details.findIndex(x => x.day.isSame(dateToCompare,'day'));
                        this.cols.push({field: dateIndex, header:  dateHeader })
                    }
                    debugger
                    console.log(this.cols);
                    this.summeryColSpan =  this.frozenCols.length + this.cols.length ;
                    this.summeryRowSpanTemp =  this.cols.length ;
                    this.data = result.data;
                    this.response = result;
                    this.dataLoaded = true;
                    this.dataIsLoading=false;



                }else{
                    this.dataIsLoading=false;
                    this.message.warn('No Data To Display !!');
                }

        });

        }
    }

    isDate(field:any): boolean{

        if(field == 'fingerCode' || field == 'userName')
            return false;
        else
        return true

    }
    approve(){
        let modelToPass = new ProjectManagerApproveInput();
        modelToPass.userIds = this.userIds;
        modelToPass.projectId = this.selectedProject.id;
        debugger
        let date = new Date(this.selectedDate);
        modelToPass.month = date.getMonth() + 1;
        modelToPass.year = date.getFullYear();
        modelToPass.isDateRange = this.isDateRange;
        modelToPass.startDate = moment(this.startDate);
        modelToPass.endDate = moment(this.endDate);
        this.dataIsLoading=true;
        this._transactionService.pojectManagerApprove(modelToPass).subscribe((result) => {
            this.clearData();
            this.dataIsLoading=false;
            this.message.success('Approved');
        });

    }
    reject(){
        let modelToPass = new ProjectManagerApproveInput();
        modelToPass.userIds = this.userIds;
        modelToPass.projectId = this.selectedProject.id;
        modelToPass.month = this.month;
        modelToPass.year = this.year;
        this._transactionService.pojectManagerReject(modelToPass).subscribe((result) => {
            this.message.success('Approved');
        });

    }

    clearData(){
        this.dataLoaded = false;
        this.data = [];
    }

    openCreateOrEditTransaction(intransId:number,outTransId:number,transDate:moment.Moment,userId:number){
        this.createOrEditAttendanceModal.show(intransId,outTransId,transDate,userId);
    }
    onisMonthChanged(){
        this.isDateRange = !this.isMonth;
    }
    onisDateRangehanged(){
        this.isMonth = !this.isDateRange;
    }
    loadUser(){

        let date = new Date("2020-08-01");
            if(this.selectedDate != "08/2020")
                date = new Date(this.selectedDate);

        this.month = date.getMonth() + 1;
        this.year = date.getFullYear();
        this.dataIsLoading=true;
        if(this.validateForm()){
        this._transactionService.getUserByProject(this.selectedProject.id,
            this.startDate,this.endDate,this.month,
            this.year,this.selectedUserType.value,this.selectedUserIds , this.isMonth,this.isDateRange).subscribe((result:UserFlatDto[]) => {
                this.dataIsLoading=false;
                this.isLoadUser = true;
                result.forEach(element => {
                   this.usersList.push({label:element.name, value:element.id})
                });
            });
        }else{
            this.dataIsLoading=false;
        }

    }
    resetUsers(){
        this.selectedUsers = [];
        this.isLoadUser = false;
    }
    openTransactionInfo(intransId:number,outTransId:number){
        this.transactionInfoModal.show(intransId,outTransId)
    }
    // getRowClass(data:any,col:any){
    //     if(this.isDate(col.field)){
    //         if(data.details[col.field]['isProjectManagerApproved'] == true)
    //             return 'approved center';
    //     }
    //     return 'center';
    // }
}
