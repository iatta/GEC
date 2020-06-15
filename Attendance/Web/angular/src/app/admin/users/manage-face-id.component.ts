import { AppConsts } from '@shared/AppConsts';
import { NetworkingHelpers ,UXNextStep } from './../../../assets/processors/helpers/NetworkingHelpers';
import { NgForm } from '@angular/forms';
import { Component, OnInit, Injector, NgZone } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import {UserServiceProxy,GetUserForFaceIdOutput,UpdateUserFaceIdInput}  from '@shared/service-proxies/service-proxies';

import * as ZoomAuthentication from '../../../../src/assets/core-sdk/ZoomAuthentication.js/ZoomAuthentication';
import { LivenessCheckProcessor } from '../../../../src/assets/processors/LivenessCheckProcessor';
import { SampleAppUtilities } from './SampleAppUtilities';
import { DomSanitizer } from '@angular/platform-browser';

const ZoomSDK = ZoomAuthentication.ZoomSDK;
declare var ZoomGlobalState: any;

@Component({

    templateUrl: './manage-face-id.component.html'
})

export class ManageFaceIdComponent extends AppComponentBase {

    civilId:string;
    active:boolean = false;
    mySrc:any;
    testSrc:any =  AppConsts.appBaseUrl + '/assets/common/images/default-profile-picture.png';
    private zone:NgZone;
    user: GetUserForFaceIdOutput = new GetUserForFaceIdOutput();
    public zoomFaceMapResultCallback: ZoomAuthentication.ZoomFaceMapResultCallback | null;
    public latestZoomSessionResult: ZoomAuthentication.ZoomSessionResult | null;
    photoLoaded:boolean = false;
    updatingUserFace:boolean = false;

    constructor(
        injector: Injector,
        private _userService: UserServiceProxy,
        private _sanitizer: DomSanitizer,
        private _ngZone: NgZone,


    ) {
        super(injector);
        this.zoomFaceMapResultCallback = null;
        this.latestZoomSessionResult = null;
        this.zone = _ngZone;
    }

    initZoom():void {
        ZoomSDK.setResourceDirectory('../../../assets/core-sdk/ZoomAuthentication.js/resources');

        // Set the directory path for required ZoOm images.
        ZoomSDK.setImagesDirectory('../../../assets/core-sdk/zoom_images');

        // Initialize ZoOm and configure the UI features.
        ZoomSDK.initialize(ZoomGlobalState.DeviceLicenseKeyIdentifier, this.initializeComplete.bind(this));
    }

  // This is called to start a liveness check when the "Liveness Check" buttin is clicked
    livenessCheck(){
        this.updatingUserFace = true;
        new LivenessCheckProcessor((isSuccess: boolean, zoomSessionResult: ZoomAuthentication.ZoomSessionResult) => {
            this.updatingUserFace = true;
            this.photoLoaded = false;
            let ttt = zoomSessionResult.faceMetrics.auditTrail[0].toString();
            this.testSrc =  this._sanitizer.bypassSecurityTrustUrl(ttt);
            this.photoLoaded = true;
            ZoomGlobalState.userIdentifier = 'PADA' + this.civilId;
             NetworkingHelpers.getEnrollmentResponseFromZoomServer(zoomSessionResult, this.zoomFaceMapResultCallback , (Response: any)=>{
                this.updatingUserFace = true;
                zoomSessionResult.faceMetrics.getFaceMapBase64((faceMapBase64: any)=>{
                    this.updatingUserFace = true;
                    this.updateUserFaceIdInput(zoomSessionResult,faceMapBase64);
                }) 
                
             });

        });
    }

    updateUserFaceIdInput(zoomSessionResult: ZoomAuthentication.ZoomSessionResult,faceMapBase64 : string){
        this.updatingUserFace = true;
        let model = new UpdateUserFaceIdInput();
        model.userId = this.user.id;
        model.image = zoomSessionResult.faceMetrics.auditTrail[0].toString();
        model.faceMap = faceMapBase64;
        this._userService.updateUserFaceId(model).subscribe((response)=>{
            this.updatingUserFace = false;
            this.message.success(this.l('FaceIdUpdatedSuccessfully'));
            
        })

    }
    // Updates UI when initialization completes
  initializeComplete() {
    // display status
    (document.getElementById('status') as HTMLElement).innerHTML = ZoomSDK.getFriendlyDescriptionForZoomSDKStatus(ZoomSDK.getStatus());
    // enable button
    (document.getElementById('liveness-button') as HTMLButtonElement).disabled = false;
  }

    search(userSearchForm : NgForm){
        this.active = false;
        if(userSearchForm.form.valid){
            this._userService.getUserForFaceId(this.civilId).subscribe((userResult) => {
                this.active = true;
                this.user = userResult;
                this.photoLoaded = true;
                if(this.user.image)
                    this.testSrc =  this._sanitizer.bypassSecurityTrustUrl(this.user.image);
                this.initZoom();
            })
        }
    }


}
