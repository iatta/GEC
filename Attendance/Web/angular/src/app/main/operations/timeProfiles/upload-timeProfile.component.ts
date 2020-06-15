import { AppConsts } from '@shared/AppConsts';
import { CreateTimeProfileFromExcelDto } from './../../../../shared/service-proxies/service-proxies';
import { TimeProfilesServiceProxy } from '@shared/service-proxies/service-proxies';
import {CustomServiceProxy} from '@shared/service-proxies/custom-proxies';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/common/app-component-base';
import { Component, ViewEncapsulation, Injector } from '@angular/core';
import * as XLSX from 'xlsx';
import { HttpParams, HttpHeaders, HttpClient } from '@angular/common/http';


@Component({
    templateUrl: './upload-timeProfile.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})

export class UploadTimeProfileComponent extends AppComponentBase {
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
       private  _timeProfilesServiceProxy:  TimeProfilesServiceProxy,
       private  _customService: CustomServiceProxy

    ) {
        super(injector);
        this.http = http;
    }

    onUpload(event) {
        debugger
        this.fileUploaded = event.target.files[0];
        this.readExcel();
    }

    readExcel(){
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
            this.jsonData = this.jsonData.filter(item => item.Date !== undefined);

            this.addTimeProfile();
        }

        readFile.readAsArrayBuffer(this.fileUploaded);

        
    
    }

    addTimeProfile(){
        let model = [];
        this.jsonData.forEach(element => {
            let modelToAdd = new CreateTimeProfileFromExcelDto();
            modelToAdd.employeeCode = element['Employee Code'];
            modelToAdd.date =element['Date'];
            modelToAdd.shiftType = element['Shift Type'];
            modelToAdd.shift = element['Shift '];
            model.push(modelToAdd);
        });

        if(model.length > 0){
            this._customService.createTimeProfileFromExcel(model).subscribe(()=>{
                this.message.success(this.l('TimeProfileAddedSuccessfully'));
            })
        }
    }

    downloadSample(){
        const url = AppConsts.remoteServiceBaseUrl + '/File/DownloadTimeProfileSample';
        location.href = url; //TODO: This causes reloading of same page in Firefox
    }
}
