import { AppConsts } from '@shared/AppConsts';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InOutReportOutput, FingerReportOutput } from './../../../../shared/service-proxies/service-proxies';
import { Component, OnInit, Injector, Input } from '@angular/core';
import * as moment from 'moment';
import * as _ from 'lodash';

@Component({
    selector: 'finger-report-component',
    templateUrl: './fingerReport.component.html'
})

export class FingerReportComponent extends AppComponentBase implements OnInit {


    defaultLogo = '';
    fingerReportList:FingerReportOutput[] = [];
    rowGroupMetadata: any;
    show = false;
    reportName = 'تقرير البصمات ';
    fromDate:moment.Moment;
    toDate:moment.Moment;
    newList:any;


    constructor(injector: Injector) {
        super(injector);
     }

     setData(data: FingerReportOutput[] , fromDate:moment.Moment , toDate:moment.Moment){

        this.fromDate = fromDate;
        this.toDate = toDate;
        this.fingerReportList = data;

        // this.newList = _.mapValues(_.groupBy(this.fingerReportList, 'userId'),
        //                   clist => clist.map(car => _.omit(car, 'userId')));
//let groupedList = this.groupBy(this.fingerReportList,"userId");
// console.log(this.fingerReportList.reduce((data, x) => {
//     (data[x['userId']] = data[x['userId']] || []).push(x);
//     return data;
//   }, {}));

        this.updateRowGroupMetaData();

        this.show = true;
        //console.log(this.fingerReportList);
     }
     
     groupBy(xs, key) {
        return xs.reduce(function(rv, x) {
          (rv[x[key]] = rv[x[key]] || []).push(x);
          return rv;
        }, {});
      };

     updateRowGroupMetaData() {
        this.rowGroupMetadata = {};
        if (this.fingerReportList) {
            for (let i = 0; i < this.fingerReportList.length; i++) {
                let rowData = this.fingerReportList[i];
                let emp = rowData.userId;
                if (i === 0) {
                    this.rowGroupMetadata[emp] = { index: 0, size: 1 };
                }
                else{
                    let previousRowData = this.fingerReportList[i - 1];
                    let previousRowGroup = previousRowData.userId;
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


    exportExcel() {
        let new_list = this.fingerReportList.map(function(obj) {
            return {
              date : moment(obj.creationTime).format('YYYY-MM-DD').toString(),
              timeIn: obj.scan1,
              timeOut: obj.scan2
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

    close(): void {
        this.show = false;
    }

    ngOnInit() {
        this.defaultLogo =  AppConsts.appBaseUrl + '/assets/common/images/menu.png';
    }
}

