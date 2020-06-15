import { SelectItem } from 'primeng/api';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/common/app-component-base';
import { Component, ViewChild, Injector,ViewEncapsulation, Output, EventEmitter, OnInit } from '@angular/core';
import {ReportInput,EmployeeReportOutput,UserServiceProxy} from '@shared/service-proxies/service-proxies';
import {CustomServiceProxy} from '@shared/service-proxies/custom-proxies';

import * as moment from 'moment';

@Component({
    templateUrl: './employee-report.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})

export class EmployeeReportComponent extends AppComponentBase implements OnInit {
 /**
  *
  */

 reports: SelectItem[];
 days: SelectItem[];
 usersList: SelectItem[] = [];
 reportInput: ReportInput = new ReportInput();
 selectedReport: number;
 saving = false;
 active =  false;
 reportResult:EmployeeReportOutput[] = [];


 reportName: string = '';
 inOutReportLoaded:boolean = false;
 fingerReportLoaded:boolean = false;
 reportLoaded=false;
 pdfSrc:any;
 constructor(
            injector: Injector,
            private _userServiceProxy: UserServiceProxy,
            private  _customService: CustomServiceProxy

        ) {
            super(injector);

        }

    ngOnInit(): void {
       this.pdfSrc = "https://vadimdez.github.io/ng2-pdf-viewer/assets/pdf-test.pdf";
        this.reports = [
            {label: this.l('SequentialAbsetReport'), value: 1},
            {label: this.l('NonSequentialAbsetReport'), value: 2},
            {label: this.l('SequentialAttendanceReport'), value: 3},
            {label: this.l('NonSequentialAttendanceReport'), value: 4}
        ]


        this.selectedReport = 1;

        this.days = [];
        this.days.push({label: '4', value: 4},{label: '15', value: 15});


        this.reportLoaded = false;
        this.reportInput = new ReportInput();
        this.reportInput.daysCount = 4;
        this.reportInput.userIds = [];
        this.reportInput.fromDate = moment().startOf('year');
        this.reportInput.toDate = moment();
        this.active = true;
    }
    getPdf(){
        this._customService.getPdf().subscribe((result)=>{
            const blob = new Blob([result.body], { type: 'application/pdf' });
            let fileURL = URL.createObjectURL(blob);
            window.open(fileURL);
            this.pdfSrc = fileURL;
        });
    }
    generate() {
        this.reportLoaded = false;
        switch (this.selectedReport) {
            case 1:
                this.reportInput.type = 2;
            break;
            case 2:
                this.reportInput.type = 1;
            break;
            case 3:
                this.reportInput.type = 2;
            break;
            case 4:
                this.reportInput.type = 1;
            break;
        }
        this._customService.calculateDaysReport(this.reportInput).subscribe((result)=>{
            debugger
            switch (this.selectedReport) {
                case 1:
                    this.reportResult = result.filter(s => s.daysCount === this.reportInput.daysCount);
                break;
                case 2:
                    this.reportResult = result.filter(s => s.absentCount === this.reportInput.daysCount);
                break;
                case 3:
                    this.reportResult = result.filter(s => s.daysCount === this.reportInput.daysCount);
                break;
                case 4:
                    this.reportResult = result.filter(s => s.attendanceCount === this.reportInput.daysCount);
                break;
            }
            console.log(this.selectedReport);
            this.reportLoaded = true;
        });
    }

    setDays(selectedReport: any){
        switch (selectedReport) {
            case 1:
                this.days = [];
                this.days.push({label: '7', value: 7},{label: '15', value: 15});
                this.reportInput.daysCount = 7;
            break;
            case 2:
                this.days = [];
                this.days.push({label: '4', value: 4});
                this.reportInput.daysCount = 4;
            break;
            case 3:
                this.days = [];
                this.days.push({label: '180' , value: 180});
                this.reportInput.daysCount = 180;
            break;
            case 4:
                this.days = [];
                this.days.push({label: '180' , value:180});
                this.reportInput.daysCount = 180;
            break;
        }
    }





}
