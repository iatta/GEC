import { SelectItem } from 'primeng/api';
import { ActualSummerizeTimeSheetDto, ProjectManagerApproveInput, ActualSummerizeTimeSheetOutput, UserTimeSheetInput, UserFlatDto } from './../../../../shared/service-proxies/service-proxies';
import { TransactionsServiceProxy, ProjectsServiceProxy, ProjectDto } from '@shared/service-proxies/service-proxies';
import { ActivatedRoute, Router } from '@angular/router';
import { AppComponentBase } from '@shared/common/app-component-base';
import { Component, OnInit, Injector, ViewChild } from '@angular/core';
import { rowsAnimation } from '@shared/animations/template.animations';
import * as moment from 'moment';
import { CreateOrEditAttendanceModalComponent } from '../manualTransactions/create-or-edit-attendance-modal.component';
import { TransactionInfoeModalComponent } from '../manualTransactions/transaction-info-modal.component';
import * as _ from 'lodash';

@Component({
    templateUrl: './hr-level-approve.component.html',
    styleUrls:['./maneger-level-approve.css'],
    animations: [rowsAnimation],
})

export class HrLevelApproveComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditAttendanceModal', { static: true }) createOrEditAttendanceModal: CreateOrEditAttendanceModalComponent;
    @ViewChild('transactionInfoModal', { static: true }) transactionInfoModal: TransactionInfoeModalComponent;

    startDate:moment.Moment = moment().startOf('day');
    endDate:moment.Moment = moment().startOf('day');
    projectId:number;
    data:ActualSummerizeTimeSheetDto[] = [];
    response:ActualSummerizeTimeSheetOutput = new ActualSummerizeTimeSheetOutput();
    dataLoaded= false;
    show = false;
    projectList:ProjectDto[];
    selectedProject:ProjectDto;
    selectedDate='08/2020';
    selectedFirstDate='01/01/2020';
    selectedEndDate='01/01/2020';
    active = false;
    cols: any[];
    isTrue:boolean = false;
    dataIsLoading = false;
    month:number;
    year:number;

    userIds:UserTimeSheetInput[] = [];
    userIdsToApprove: UserTimeSheetInput[] = [];
    unitIdToApprove:number;
    selectedUserType:SelectItem;
    userTypes:SelectItem[]= [];
    summeryRowSpan=0;
    selectedUsers:SelectItem[]=[];
    isMonth:boolean;
    isDateRange:boolean;
    selectedUserIds:number[]=[];

    isLoadUser:boolean;
    usersList: SelectItem[] = [];

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

        this._projectServiceProxy.getAllFlatForOrganizationUnitManager().subscribe((result)=>{
            this.projectList = result;
            this.show = true;
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

            this._transactionService.getMangerUsersToApprove(this.selectedProject.id,
                moment(this.selectedFirstDate),moment(this.selectedEndDate),this.month,this.year,this.selectedUserType.value,this.selectedUserIds , this.isMonth,this.isDateRange).subscribe((result) => {
                console.log(result);
                this.response =result;
                if(result.data.length > 0){
                    this.userIds = result.userIds;
                    this.userIdsToApprove = result.userIdsToApprove;
                    this.unitIdToApprove = result.unitIdToApprove;
                    this.cols = [
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
                    console.log(this.cols);
                    this.summeryRowSpan =  this.cols.length;
                    this.data = result.data;
                    this.dataLoaded = true;
                    this.dataIsLoading=false;



                }else{
                    this.dataIsLoading=false;
                    this.message.warn('No Data To Display !!');
                }

        });

        }
    }
    onisMonthChanged(){
        this.isDateRange = !this.isMonth;
    }
    onisDateRangehanged(){
        this.isMonth = !this.isDateRange;
    }

    openTransactionInfo(intransId:number,outTransId:number){
        this.transactionInfoModal.show(intransId,outTransId)
    }

    loadUser(){

        let date = new Date("2020-08-01");
            if(this.selectedDate != "08/2020")
                date = new Date(this.selectedDate);

        this.month = date.getMonth() + 1;
        this.year = date.getFullYear();
        this.dataIsLoading=true;
        if(this.validateForm()){
        this._transactionService.getUserByManagerUnit(this.selectedProject.id,
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

    isDate(field:any): boolean{
        if(field == 'fingerCode' || field == 'userName')
            return false;
        else
        return true

    }

    getRowClass(rowData:any,col:any):string {
        if(this.isDate(col.field)){
            if(rowData.details[col.field]['isWorkInAnotherProject'])
                return 'transferred'
            if(!rowData.details[col.field]['isProjectManagerApproved'])
            return 'pendingProjectManager';

        if( rowData.details[col.field]['isCurrentUnitApproved'])
                return 'approved';

        if(!rowData.details[col.field]['canManagerApprove'])
                return 'pendingManager';
        }
    }

    approve(){
        let modelToPass = new ProjectManagerApproveInput();
        modelToPass.userIds = this.userIdsToApprove;
        modelToPass.projectId = this.selectedProject.id;
        modelToPass.month = this.month;
        modelToPass.year = this.year;
        modelToPass.unitIdToApprove = this.unitIdToApprove;
        this._transactionService.unitManagerToApprove(modelToPass).subscribe((result) => {
            this.clearData();
            this.message.success('Approved');
        });

    }

    onOpenCalendar(container) {
        container.monthSelectHandler = (event: any): void => {
          container._store.dispatch(container._actions.select(event.date));
        };
        container.setViewMode('month');
       }


       clearData(){
        this.dataLoaded = false;
        this.data = [];

    }


    openCreateOrEditTransaction(intransId:number,outTransId:number,transDate:moment.Moment,userId:number){
        this.createOrEditAttendanceModal.show(intransId,outTransId,transDate,userId);
    }


}
