import { AppConsts } from '@shared/AppConsts';
import { CreateTimeProfileFromExcelDto } from './../../../../shared/service-proxies/service-proxies';
import { TimeProfilesServiceProxy, CreateOrEditEmployeeVacationDto, EmployeeVacationsServiceProxy } from '@shared/service-proxies/service-proxies';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/common/app-component-base';
import { Component, ViewEncapsulation, Injector } from '@angular/core';
import * as XLSX from 'xlsx';
import { HttpParams, HttpHeaders, HttpClient } from '@angular/common/http';
import * as moment from 'moment';


@Component({
    templateUrl: './upload-emp-vacation.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})

export class UploadEmpVacationComponent extends AppComponentBase {
    /**
     *
     */
    private http: HttpClient;
    storeData: any;
    csvData: any;
    jsonData: any;
    textData: any;
    htmlData: any;
    fileUploaded: File;
    worksheet: any;
  
    uploadedFiles: any[] = [];
    constructor(
        http: HttpClient,
        injector: Injector,
       private  _employeeVacationsServiceProxy:  EmployeeVacationsServiceProxy

    ) {
        super(injector);
        this.http = http;
    }

    onUpload(event) {
        this.fileUploaded = event.target.files[0];
        this.readExcel();
    }

    readExcel(){
        debugger
        let readFile = new FileReader();
        readFile.onload = (e) => {
          this.storeData = readFile.result;
                let data = new Uint8Array(this.storeData);
                let arr = new Array();
          for (var i = 0; i != data.length; ++i) arr[i] = String.fromCharCode(data[i]);
          let bstr = arr.join("");
          let workbook = XLSX.read(bstr, { type: "binary" });
          let first_sheet_name = workbook.SheetNames[0];
            this.worksheet = workbook.Sheets[first_sheet_name];  
            this.jsonData = XLSX.utils.sheet_to_json(this.worksheet, { raw: false });
            this.jsonData = this.jsonData.filter(item => item.FromDate !== undefined);

            this.addTimeProfile();
        }

        readFile.readAsArrayBuffer(this.fileUploaded);

        
    
    }

    addTimeProfile(){
        let model = [];
        this.jsonData.forEach(element => {
            let modelToAdd = new CreateOrEditEmployeeVacationDto();
            modelToAdd.fromDate =moment(element['FromDate'],'DD/MM/YYYY');
            modelToAdd.toDate =moment(element['ToDate'],'DD/MM/YYYY');
            modelToAdd.description = element['Desc'];
            modelToAdd.leaveCode = element['LeaveCode'];
            modelToAdd.empCode = element['EmpCode'];

            model.push(modelToAdd);
        });

        if(model.length > 0){
            this._employeeVacationsServiceProxy.createFromExcel(model).subscribe(()=>{
                this.message.success(this.l('EmployeeVacationAddedSuccessfully'));
            })
        }
    }

    downloadSample(){
        const url = AppConsts.remoteServiceBaseUrl + '/File/DownloadEmployeeVacationSample';
        location.href = url; //TODO: This causes reloading of same page in Firefox
    }
}
