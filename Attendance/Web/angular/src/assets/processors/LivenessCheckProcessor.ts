// Demonstrates performing a Liveness Check.

import * as ZoomAuthentication from "../core-sdk/ZoomAuthentication.js/ZoomAuthentication";
import { NetworkingHelpers, UXNextStep } from "./helpers/NetworkingHelpers"

var ZoomSDK = ZoomAuthentication.ZoomSDK;

export class LivenessCheckProcessor {
    public zoomFaceMapResultCallback: ZoomAuthentication.ZoomFaceMapResultCallback | null;
    public latestZoomSessionResult: ZoomAuthentication.ZoomSessionResult | null;
    public onComplete: (isSuccess: boolean, zoomSessionResult: ZoomAuthentication.ZoomSessionResult) => void
    public isSuccess: boolean = false;

    constructor(
        onComplete: ( isSuccess: boolean, zoomSessionResult: ZoomAuthentication.ZoomSessionResult) => void
    ){
        this.zoomFaceMapResultCallback = null;
        this.latestZoomSessionResult = null;
        this.onComplete = onComplete;

        var _this = this;
        var onZoomSessionComplete = function() {
            _this.onComplete(_this.isSuccess, _this.latestZoomSessionResult!);
        }

        // Launch the ZoOm Session.
        new ZoomSDK.ZoomSession(onZoomSessionComplete, this);
    }

    // Required function that handles calling ZoOm Server to get result and decides how to continue.
    public processZoomSessionResultWhileZoomWaits(zoomSessionResult: ZoomAuthentication.ZoomSessionResult, zoomFaceMapResultCallback: ZoomAuthentication.ZoomFaceMapResultCallback ){
        var _this = this;

        _this.latestZoomSessionResult = zoomSessionResult;
        _this.zoomFaceMapResultCallback = zoomFaceMapResultCallback;

        // cancellation, timeout, etc.
        if(zoomSessionResult.status !== ZoomSDK.ZoomSessionStatus.SessionCompletedSuccessfully || !zoomSessionResult.faceMetrics.faceMap || !zoomSessionResult.faceMetrics.faceMap.size) {
            NetworkingHelpers.cancelInFlightRequests(); // if upload is taking place and user cancels/context switches, abort the in-flight request
            zoomFaceMapResultCallback.cancel();
            return;
        }

        // Create and parse request to ZoOm Server.
        NetworkingHelpers.getLivenessCheckResponseFromZoomServer(
            zoomSessionResult,
            zoomFaceMapResultCallback,
            function(nextStep: UXNextStep){
                _this.latestZoomSessionResult = zoomSessionResult;
                _this.zoomFaceMapResultCallback = zoomFaceMapResultCallback;

                if (nextStep == UXNextStep.Succeed) {
                    // Dynamically set the success message.
                    ZoomSDK.ZoomCustomization.setOverrideResultScreenSuccessMessage("Liveness\r\nConfirmed");
                    zoomFaceMapResultCallback.succeed();
                    _this.isSuccess = true;
                }
                else if (nextStep == UXNextStep.Retry) {
                    zoomFaceMapResultCallback.retry();
                }
                else {
                    zoomFaceMapResultCallback.cancel();
                }
            }
        )
    }
}
