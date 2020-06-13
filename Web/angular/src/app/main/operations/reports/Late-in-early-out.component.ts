import { AppConsts } from '@shared/AppConsts';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InOutReportOutput } from './../../../../shared/service-proxies/service-proxies';
import { Component, OnInit, Injector, Input } from '@angular/core';
import * as moment from 'moment';

@Component({
    selector: 'late-in-early-out-component',
    templateUrl: './late-in-early-out.component.html',
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

export class LateInEarlyOutComponent extends AppComponentBase implements OnInit {

    defaultLogo = '';
    reportList:InOutReportOutput[] = [];
    rowGroupMetadata: any;
    show = false;
    reportName = 'تقرير دخول / خروج متأخر ';
    fromDate:moment.Moment;
    toDate:moment.Moment;


    constructor(injector: Injector) {
        super(injector);
     }

     setData(data: InOutReportOutput[] , fromDate:moment.Moment , toDate:moment.Moment){

         this.fromDate = fromDate;
         this.toDate = toDate;
        this.reportList = data;
        this.updateRowGroupMetaData();
        this.show = true;
     }

     updateRowGroupMetaData() {
        this.rowGroupMetadata = {};
        if (this.reportList) {
            for (let i = 0; i < this.reportList.length; i++) {
                let rowData = this.reportList[i];
                let emp = rowData.unitId;
                if (i === 0) {
                    this.rowGroupMetadata[emp] = { index: 0, size: 1 };
                }
                else{
                    let previousRowData = this.reportList[i - 1];
                    let previousRowGroup = previousRowData.unitId;
                    if (emp === previousRowGroup){
                        this.rowGroupMetadata[emp].size++;
                    }
                    else{
                        this.rowGroupMetadata[emp] = { index: i, size: 1 };
                    }

                }
            }
        }

        console.log(this.rowGroupMetadata );
    }


    getTotalLate(empId: any): number {
        let sum = 0;
        this.reportList.forEach(element => {
            if (element.empId === empId)
                sum+= element.attendanceLateIn + element.attendanceEarlyOut;
        });
        return sum;
    }

    exportExcel() {
        let new_list = this.reportList.map(function(obj) {
            return {
              date : moment(obj.attendanceDate).format('YYYY-MM-DD').toString(),
              timeIn: obj.attendanceInStr,
              timeOut: obj.attendanceOutStr
            }
          });

        import("xlsx").then(xlsx => {
            const worksheet = xlsx.utils.json_to_sheet(new_list , {cellDates: true} );
            const workbook = { Sheets: { 'data': worksheet }, SheetNames: ['data'] };
            const excelBuffer: any = xlsx.write(workbook, { bookType: 'xlsx', type: 'array' });
            this.saveAsExcelFile(excelBuffer, "primengTable");
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


    getRowClass(rowData: any) {
        if(rowData.isAbsent)
            return 'absent';

        if(rowData.isVacation)
            return 'vacation';

        if(rowData.isHoliday)
            return 'holiday';
    }

    getStatusText(rowData: any){
        if(rowData.leaveTypeNameA)
            return rowData.leaveTypeNameA;

        if(rowData.holidayName)
            return rowData.holidayName;

        if(rowData.isAbsent)
            return this.l('Absent');
    }

    close(): void {
        this.show = false;
    }

    ngOnInit() {
        this.defaultLogo =  AppConsts.appBaseUrl + '/assets/common/images/menu.png';
     }
}

