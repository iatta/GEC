import { FilterUtils } from 'primeng/api';
import { result } from 'lodash';
import { ActualSummerizeTimeSheetDto, ProjectManagerApproveInput, UserTimeSheetInput } from './../../../../shared/service-proxies/service-proxies';
import { TransactionsServiceProxy, ProjectDto, ProjectsServiceProxy } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { Component, OnInit, Injector } from '@angular/core';
import { Router, ActivatedRoute, ParamMap ,Params } from '@angular/router';
import * as moment from 'moment';
import { start } from 'repl';
import { rowsAnimation } from '@shared/animations/template.animations';



@Component({
    templateUrl: './summerize.component.html',
    styleUrls:['./summerize.component.css'],
    animations: [rowsAnimation],
})


export class SummerizeReportComponent extends AppComponentBase implements OnInit {

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
    selectedDate='';
    userIds:UserTimeSheetInput[] = [];

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

        if(!this.selectedDate){
            this.message.warn('Please Select Date');
            valid = false;
            return valid;
        }
        return valid;
    }

    generateReport(){
        if(this.validateForm()){

            let date = new Date(this.selectedDate);
            this.month = date.getMonth() + 1;
            this.year = date.getFullYear();
            this.dataIsLoading=true;
            this._transactionService.getActualSummerizeTimeSheet(this.selectedProject.id,this.startDate,this.endDate,this.month,this.year).subscribe((result) => {
                if(result.data.length > 0){
                    this.userIds = result.userIds;
                    this.cols = [
                        { field: 'fingerCode', header: 'code' },
                        {field: 'userName', header: 'userName' }
                    ];
                    let firstDay = new Date(this.year, this.month - 1, 1);
                    let lastDay = new Date(this.year, this.month, 0);
                    console.log(lastDay);

                    this.startDate = moment(firstDay);
                    this.endDate = moment(lastDay);

                    for (let m = moment(this.startDate); m.diff(this.endDate, 'days') <= 0; m.add(1, 'days')) {
                        let dateHeader = m.format('DD/MM/YYYY');
                        let dateToCompare = moment(m);
                        let dateIndex = result.data[0].details.findIndex(x => x.day.isSame(dateToCompare,'day'));
                        this.cols.push({field: dateIndex, header:  dateHeader })
                    }
                    console.log(this.cols);
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
        modelToPass.month = this.month;
        modelToPass.year = this.year;
        this._transactionService.pojectManagerApprove(modelToPass).subscribe((result) => {
            this.clearData();
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
}
