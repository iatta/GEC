import { ActualSummerizeTimeSheetDto, ProjectManagerApproveInput, ActualSummerizeTimeSheetOutput, UserTimeSheetInput } from './../../../../shared/service-proxies/service-proxies';
import { TransactionsServiceProxy, ProjectsServiceProxy, ProjectDto } from '@shared/service-proxies/service-proxies';
import { ActivatedRoute, Router } from '@angular/router';
import { AppComponentBase } from '@shared/common/app-component-base';
import { Component, OnInit, Injector } from '@angular/core';
import { rowsAnimation } from '@shared/animations/template.animations';
import * as moment from 'moment';

@Component({
    templateUrl: './maneger-level-approve.html',
    styleUrls:['./maneger-level-approve.css'],
    animations: [rowsAnimation],
})

export class ManagerLevelApproveComponent extends AppComponentBase implements OnInit {
    startDate:moment.Moment = moment().startOf('day');
    endDate:moment.Moment = moment().startOf('day');
    projectId:number;
    data:ActualSummerizeTimeSheetDto[] = [];
    response:ActualSummerizeTimeSheetOutput = new ActualSummerizeTimeSheetOutput();
    dataLoaded= false;
    show = false;
    projectList:ProjectDto[];
    selectedProject:ProjectDto;
    active = false;
    cols: any[];
    isTrue:boolean = false;
    dataIsLoading = false;
    month:number;
    year:number;
    selectedDate='';
    userIds:UserTimeSheetInput[] = [];
    userIdsToApprove: UserTimeSheetInput[] = [];

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
            this._transactionService.getMangerUsersToApprove(this.selectedProject.id,this.startDate,this.endDate,this.month,this.year).subscribe((result) => {
                console.log(result);
                this.response =result;
                if(result.data.length > 0){
                    this.userIds = result.userIds;
                    this.userIdsToApprove = result.userIdsToApprove;

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

    getRowClass(rowData:any):string {
        if(!rowData.isProjectManagerApproved)
            return 'pendingProjectManager';

        if(rowData.isCurrentUnitApproved)
                return 'approved';

        if(!rowData.canManagerApprove)
                return 'pendingManager';

    }

    approve(){
debugger
        let modelToPass = new ProjectManagerApproveInput();
        modelToPass.userIds = this.userIdsToApprove;
        modelToPass.projectId = this.selectedProject.id;
        modelToPass.month = this.month;
        modelToPass.year = this.year;
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

}
