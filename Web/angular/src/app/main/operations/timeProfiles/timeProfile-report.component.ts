import { SelectItem } from 'primeng/api';
import { AppComponentBase } from '@shared/common/app-component-base';
import { Component, Injector, OnInit } from '@angular/core';
import { ShiftsServiceProxy, GetShiftForViewDto,UserServiceProxy,UserReportDto } from '@shared/service-proxies/service-proxies';
import {CustomServiceProxy} from '@shared/service-proxies/custom-proxies';

@Component({
    selector: 'TimeProfileReport',
    templateUrl: './timeProfile-report.component.html'
})

export class TimeProfileReportComponent extends AppComponentBase implements OnInit {
    
    shifts: SelectItem[] = [];
    selectedShift:number;
    loaded=false;
    userList: UserReportDto[] = [];
    exportColumns: any[];
    cols: any[];
    selectedUsers: UserReportDto[];


    constructor(
        injector: Injector,
       private  _shiftServiceProxy: ShiftsServiceProxy,
       private  _userServiceProxy: UserServiceProxy,
       private  _customService: CustomServiceProxy
    ) {
        super(injector);
    }
    ngOnInit(): void {
        this.cols = [
            { field: 'userName', header: this.l('Employee') },
            { field: 'shiftName', header: this.l('ShiftName') },
            { field: 'startDate', header: this.l('StartDate') },
            { field: 'endDate', header: this.l('EndDate') }
        ];
        
        this.exportColumns = this.cols.map(col => ({title: col.header, dataKey: col.field}));

        this._shiftServiceProxy.getAllFlat().subscribe((result)=>{
            this.shifts.push({label:this.l('SelectShift'), value:null});
            result.forEach(element => {
                this.shifts.push({label:element.shift.nameAr, value:element.shift.id});
            });
            this.loaded = true;
        });
    }

    generateReport(){
        this._customService.getUsersByShiftIdReport(this.selectedShift).subscribe((result) => {
            this.userList = result;
        });
    }

    getDate() {
        let data = [];
        for(let user of this.userList) {
            
            data.push(user);
        }
        return data;
    }

    exportPdf() {
        import("jspdf").then(jsPDF => {
            import("jspdf-autotable").then(x => {
                const doc = new jsPDF.default(0,0);
                doc.autoTable(this.exportColumns, this.userList);
                doc.save('primengTable.pdf');
            })
        })
    }

    exportExcel() {
        import("xlsx").then(xlsx => {
            const worksheet = xlsx.utils.json_to_sheet(this.getDate());
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
}