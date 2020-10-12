import { NormalOverTimeReportOutput } from './../../../../shared/service-proxies/service-proxies';
import { result } from 'lodash';
import { TransactionsServiceProxy } from '@shared/service-proxies/service-proxies';
import { ManagerUnitsModalComponent } from '@app/admin/shared/manager-units-modal.component';

import { SelectItem } from 'primeng/api';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/common/app-component-base';
import { Component, ViewChild, Injector,ViewEncapsulation, Output, EventEmitter, OnInit } from '@angular/core';
import { CustomServiceProxy } from  '@shared/service-proxies/custom-proxies';
import * as moment from 'moment';


@Component({
    templateUrl: './manager-report.component.html',
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

export class ManagerReportComponent extends AppComponentBase implements OnInit {
 /**
  *
  */
 @ViewChild('managerUnitsModal', { static: true }) managerUnitsModalComponent: ManagerUnitsModalComponent;



 reports: SelectItem[];
 usersList: SelectItem[] = [];
 data:NormalOverTimeReportOutput[] = [];
 organizationUnitId:number;
 fromDate:moment.Moment;
 toDate:moment.Moment;
 saving = false;
 active =  false;
 loading = false;
 unitGlow=true;
 dataLoaded=false;

 organizationUnitName:string;

 constructor(
            injector: Injector,
            private _trnsactionService:TransactionsServiceProxy
        ) {
            super(injector);

        }

    ngOnInit(): void {
        this.fromDate =moment();
        this.toDate =moment();
        this.active = true;
    }

    generate(){
        this._trnsactionService.getDepartmentTransactions(this.organizationUnitId,this.fromDate,this.toDate).subscribe((result) => {
            if(result.length > 0){
                this.data = result;
                this.dataLoaded=true;
            }else{
                this.message.warn('No Data To Display');
            }
        })
    }

    exportExcel() {
        import("xlsx").then(xlsx => {
            let mappedData = this.data.map(item => {return {...item,attendanceDate : item.attendanceDate.format('DD/MM/YYYY')}});
            const worksheet = xlsx.utils.json_to_sheet(mappedData);
            const workbook = { Sheets: { 'data': worksheet }, SheetNames: ['data'] };
            const excelBuffer: any = xlsx.write(workbook, { bookType: 'xlsx', type: 'array' });
            this.saveAsExcelFile(excelBuffer, "NormalOvertime");
        });
    }

    saveAsExcelFile(buffer: any, fileName: string): void {
        import("file-saver").then(FileSaver => {
            let EXCEL_TYPE = 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=UTF-8';
            let EXCEL_EXTENSION = '.xlsx';
            const data: Blob = new Blob([buffer], {
                type: EXCEL_TYPE
            });
            FileSaver.saveAs(data, fileName + '_export_' + new Date().getTime() + EXCEL_EXTENSION);
        });
    }
    openUnitModal(){
        this.managerUnitsModalComponent.show();
    }

    ouSelected(event: any): void {
        console.log('in manage time component');
        this.organizationUnitId = event.id;
        this.organizationUnitName = event.displayName;
        this.unitGlow = false;
        // this.getUsers(event.id);
    }

}
