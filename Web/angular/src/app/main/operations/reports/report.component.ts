import { PermitReportComponent } from './permit.component';
import { LateInEarlyOutComponent } from './Late-in-early-out.component';
import { AbsentComponent } from './absent.component';
import { ForgetInOutComponent } from './forget-in-out.component';
import { FingerReportComponent } from './fingerReport.component';
import { InOutReportOutput,FingerReportOutput } from './../../../../shared/service-proxies/service-proxies';
import { OrganizationUnitsHorizontalTreeModalComponent } from './../../../admin/shared/organization-horizontal-tree-modal.component';
import { SelectItem } from 'primeng/api';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/common/app-component-base';
import { Component, ViewChild, Injector,ViewEncapsulation, Output, EventEmitter, OnInit } from '@angular/core';
import { ReportInput, UserServiceProxy } from '@shared/service-proxies/service-proxies';
import { CustomServiceProxy } from  '@shared/service-proxies/custom-proxies';

import * as moment from 'moment';
import { InOutComponent } from './inOut.component';

@Component({
    templateUrl: './report.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
    styles: [`
        .vacation {
            background-color: #2CA8B1 !important;
            color: #ffffff !important;
        }
        .holiday {
            background-color: #91e8a4 !important;
            color: #ffffff !important;
        }


        .absent {
            background-color: #b12c2cc7 !important;
            color: #ffffff !important;
        }
    `
    ]
})

export class ReportComponent extends AppComponentBase implements OnInit {
 /**
  *
  */
 @ViewChild('organizationUnitsHorizontalTreeModal', { static: true }) organizationUnitsHorizontalTreeModal: OrganizationUnitsHorizontalTreeModalComponent;
 @ViewChild('inOutReport', { static: true }) InOutComponentReport: InOutComponent;
 @ViewChild('fingerReport', { static: true }) FingerReportComponent: FingerReportComponent;
 @ViewChild('forgetInOuReport', { static: true }) ForgetInOutComponent: ForgetInOutComponent;
 @ViewChild('absentReport', { static: true }) AbsentComponent: AbsentComponent;
 @ViewChild('lateInEarlyOut', { static: true }) LateInEarlyOutComponent: LateInEarlyOutComponent;
 @ViewChild('permit', { static: true }) PermitReportComponent: PermitReportComponent;


 reports: SelectItem[];
 usersList: SelectItem[] = [];
 reportInput: ReportInput = new ReportInput();
 selectedReport: number;
 saving = false;
 active =  false;



 reportName: string = '';
 inOutReportLoaded:boolean = false;
 fingerReportLoaded:boolean = false;
 reportLoaded=false;
 loading=false;
 reportSrc:any;

 constructor(
            injector: Injector,
            private _userServiceProxy: UserServiceProxy,
            private _customServiceProxy:CustomServiceProxy

        ) {
            super(injector);

        }

    ngOnInit(): void {
        //console.log(abp.localization.currentLanguage);
        this.reports = [
            {label: this.l('InOutReport'), value: 1},
            {label: this.l('FingerReport'), value: 2},
            {label: this.l('ForgetInOutReport'), value: 3},
            {label: this.l('AbsentReport'), value: 4},
            {label: this.l('LateInEarlyOutReport'), value: 5},
            {label: this.l('PermitReport'), value: 6},
        ]
        this.selectedReport = 1;
        this.reportLoaded = false;
        this.reportInput = new ReportInput();
        this.reportInput.userIds = [];
        this.reportInput.fromDate = moment();
        this.reportInput.toDate = moment();
        this.active = true;
    }

    getUsers(unitId:number){
        this._userServiceProxy.getUsersFlatByUnitId(unitId).subscribe((result) => {

            this.reportInput.userIds = [];
            result.forEach((user) => {
                this.usersList.push({label:user.name, value: user.id });
            });

        });
    }

    openUnitModal(){
        this.organizationUnitsHorizontalTreeModal.show();
    }

    ouSelected(event: any): void {

        console.log('in manage time component');

        this.getUsers(event.id);
        //console.log(event);
    }


        generate() {
            if(this.reportInput.userIds.length > 0){
                this.FingerReportComponent.close();
                this.InOutComponentReport.close();
                this.ForgetInOutComponent.close();
                this.AbsentComponent.close();
                this.LateInEarlyOutComponent.close();
                this.reportLoaded = false;
                switch (this.selectedReport) {
                    case 1:
                        this.generateInOutReport();
                        break;
                    case 2:
                            this.generateFingerReport();
                        break;
                    case 3:
                            this.generateForgetInOutReport();
                        break;
                    case 4:
                            this.generateAbsentReport();
                        break;
                    case 5:
                        this.generateLateInEarlyOutReport();
                    break;
                    case 6:
                        this.generatePermitReport();
                    break;
                    default:
                        break;
                }
            }else{
                this.message.warn(this.l('PleaseSelectUser'));
            }

        }
        generatePermitReport(){
            this._customServiceProxy.generatePermitReport(this.reportInput).subscribe((result)=>{
                this.reportLoaded = true;
                this.PermitReportComponent.setData(result,this.reportInput.fromDate,this.reportInput.toDate);
            });

        }

        generateLateInEarlyOutReport(){
            this._customServiceProxy.generateInOutReport(this.reportInput).subscribe((result)=>{
                this.reportLoaded = true;
                this.LateInEarlyOutComponent.setData(result,this.reportInput.fromDate,this.reportInput.toDate);
            });
        }

        generateAbsentReport(){
            this._customServiceProxy.generateInOutReport(this.reportInput).subscribe((result)=>{
                this.reportLoaded = true;
                this.AbsentComponent.setData(result,this.reportInput.fromDate,this.reportInput.toDate);
            });
        }
        generateInOutReport(){
            this.loading = true;
            this._customServiceProxy.generateInOutReport(this.reportInput).subscribe((result:any)=>{
                debugger    
                this.loading = false;
                // this.reportLoaded = true;
                // this.InOutComponentReport.setData(result,this.reportInput.fromDate,this.reportInput.toDate);
                const blob = new Blob([result.body], { type: 'application/pdf' });
                let fileURL = URL.createObjectURL(blob);
                this.reportSrc = blob;
                this.reportLoaded = true;
                // window.open(fileURL);
            });
        }
        generateForgetInOutReport(){
            this.loading = true;
            this._customServiceProxy.generateForgetInOutReport(this.reportInput).subscribe((result:any)=>{

                this.loading = false;
                // this.reportLoaded = true;
                // this.ForgetInOutComponent.setData(result,this.reportInput.fromDate,this.reportInput.toDate);
                const blob = new Blob([result.body], { type: 'application/pdf' });
                let fileURL = URL.createObjectURL(blob);
                this.reportSrc = blob;
                this.reportLoaded = true;
                //window.open(fileURL);
                //this.pdfSrc = fileURL;
            });
        }

        generateFingerReport(){

            this._customServiceProxy.generateFingerReport(this.reportInput).subscribe((result:any)=>{
                // this.reportLoaded = true;
                // this.FingerReportComponent.setData(result , this.reportInput.fromDate,this.reportInput.toDate);
                const blob = new Blob([result.body], { type: 'application/pdf' });
                let fileURL = URL.createObjectURL(blob);
                //window.open(fileURL);
                this.reportSrc = blob;
                this.reportLoaded = true;
            });
        }





}
