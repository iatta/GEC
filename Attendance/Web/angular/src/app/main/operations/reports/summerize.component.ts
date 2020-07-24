import { result } from 'lodash';
import { ActualSummerizeTimeSheetDto } from './../../../../shared/service-proxies/service-proxies';
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

    validateForm(){
        let valid = true;
        if(!this.selectedProject){
            this.message.warn('Please Select Project');
            valid = false;
            return valid;
        }
        return valid;
    }

    generateReport(){
        if(this.validateForm()){
            this.dataIsLoading=true;
        this._transactionService.getActualSummerizeTimeSheet(this.selectedProject.id,this.startDate,this.endDate).subscribe((result) => {

            this.cols = [
                { field: 'code', header: 'code' },
                {field: 'userName', header: 'userName' }
            ];
            for (var m = moment(this.startDate); m.diff(this.endDate, 'days') <= 0; m.add(1, 'days')) {
                let dateHeader = m.format('DD/MM/YYYY');
                let dateToCompare = moment(m);
                debugger
                let dateIndex = result[0].details.findIndex(x => x.day.isSame(dateToCompare,'day'));
                this.cols.push({field: dateIndex, header:  dateHeader })
            }
            console.log(this.cols);
            this.data = result;
            this.dataLoaded = true;
            this.dataIsLoading=false;
        });

        }
    }

    isDate(field:any): boolean{
        if(field == 'code' || field == 'userName')
            return false;
        else
        return true

    }
}
