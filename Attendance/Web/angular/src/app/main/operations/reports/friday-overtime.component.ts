import { TransactionsServiceProxy } from '@shared/service-proxies/service-proxies';
import { AppConsts } from '@shared/AppConsts';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InOutReportOutput, NormalOverTimeReportOutput } from '../../../../shared/service-proxies/service-proxies';
import { Component, OnInit, Injector, Input } from '@angular/core';
import * as moment from 'moment';

@Component({
    templateUrl: './friday-overtime.component.html',
    styleUrls:['./normal-overtime.component.css']
})

export class FridayOvertimeComponent extends AppComponentBase implements OnInit {
    fromDate:moment.Moment =moment();
    toDate:moment.Moment=moment();
    data:NormalOverTimeReportOutput[] = [];
    dataLoaded=false;
    isDataAvailable:boolean =false;
    dataIsLoading=false;

    constructor(injector: Injector,   private _transactionService: TransactionsServiceProxy) {

        super(injector);
     }

    ngOnInit(): void {
        this.toDate  =moment(new Date());
        this.fromDate  =moment(new Date());
        this.isDataAvailable = true;
        // throw new Error("Method not implemented.");
    }

    generateReport(){
        this.dataIsLoading=true;
        this.dataLoaded=false;
        this.data =[];
        this._transactionService.getFridayOverTime(this.fromDate,this.toDate).subscribe((result) => {
            console.log(result);

            if(result.length > 0){
                this.data = result;
                this.dataLoaded=true;
                this.dataIsLoading=false;
            }else{
                this.dataIsLoading=false;
                this.message.warn('No Data To Display');
            }

        });
    }

    exportExcel() {
        import("xlsx").then(xlsx => {
            let mappedData = this.data.map(item => {return {...item,attendanceDate : item.attendanceDate.format('DD/MM/YYYY')}});
            const worksheet = xlsx.utils.json_to_sheet(mappedData);
            const workbook = { Sheets: { 'data': worksheet }, SheetNames: ['data'] };
            const excelBuffer: any = xlsx.write(workbook, { bookType: 'xlsx', type: 'array' });
            this.saveAsExcelFile(excelBuffer, "FridayOverTime");
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

}
