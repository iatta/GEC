// Demonstrates calling the FaceTec Managed Testing API and/or ZoOm Server.

import * as ZoomAuthentication from "../../core-sdk/ZoomAuthentication.js/ZoomAuthentication";
var ZoomSDK  = ZoomAuthentication.ZoomSDK;
declare var ZoomGlobalState: any
// Possible directives after parsing the result from ZoOm Server.
export enum UXNextStep {
    Succeed,
    Retry,
    Cancel
};

export var NetworkingHelpers = (function (){
    var latestXHR: XMLHttpRequest = new XMLHttpRequest();

    // Create and send the request.  Parse the results and send the caller what the next step should be (Succeed, Retry, or Cancel).
    function getLivenessCheckResponseFromZoomServer(
        zoomSessionResult: ZoomAuthentication.ZoomSessionResult,
        zoomFaceMapResultCallback: ZoomAuthentication.ZoomFaceMapResultCallback,
        resultCallback: (responseJSON: any) => void
    ){
        zoomSessionResult.faceMetrics.getFaceMapBase64(function(faceMapBase64: any) {
            var parameters = {
                sessionId: zoomSessionResult.sessionId,
                faceMap: faceMapBase64,
                lowQualityAuditTrailImage: zoomSessionResult.faceMetrics.lowQualityAuditTrailCompressedBase64()[0],
                auditTrailImage: zoomSessionResult.faceMetrics.getAuditTrailBase64JPG()[0]
            }
            callToZoomServerForResult("/liveness", parameters, zoomSessionResult.sessionId, zoomFaceMapResultCallback, function(responseJSON: any){
                var nextStep: UXNextStep = ServerResultHelpers.getLivenessNextStep(responseJSON);
                resultCallback(nextStep);
            })
        });
    }

    // Create and send the request.  Parse the results and send the caller what the next step should be (Succeed, Retry, or Cancel).
    function getEnrollmentResponseFromZoomServer(
        zoomSessionResult: ZoomAuthentication.ZoomSessionResult,
        zoomFaceMapResultCallback: ZoomAuthentication.ZoomFaceMapResultCallback,
        resultCallback: (responseJSON: any) => void
    ){
        zoomSessionResult.faceMetrics.getFaceMapBase64(function(faceMapBase64: any) {
            
            var parameters = {
                sessionId: zoomSessionResult.sessionId,
                faceMap: faceMapBase64,
                enrollmentIdentifier: ZoomGlobalState.userIdentifier,
                lowQualityAuditTrailImage: zoomSessionResult.faceMetrics.lowQualityAuditTrailCompressedBase64()[0],
                auditTrailImage: zoomSessionResult.faceMetrics.getAuditTrailBase64JPG()[0]
            }
            callToZoomServerForResult("/enrollment", parameters, zoomSessionResult.sessionId, zoomFaceMapResultCallback, function(responseJSON: any){
                
                var nextStep: UXNextStep = ServerResultHelpers.getEnrollmentNextStep(responseJSON);
                resultCallback(nextStep);
            })
        });

    }

    // Create and send the request.  Parse the results and send the caller what the next step should be (Succeed, Retry, or Cancel).
    function getAuthenticateResponseFromZoomServer(
        zoomSessionResult: ZoomAuthentication.ZoomSessionResult,
        zoomFaceMapResultCallback: ZoomAuthentication.ZoomFaceMapResultCallback,
        resultCallback: (responseJSON: any) => void
    ){
        zoomSessionResult.faceMetrics.getFaceMapBase64(function(faceMapBase64: any) {
            var parameters = {
                ssessionId: zoomSessionResult.sessionId,
                source: {enrollmentIdentifier:  ZoomGlobalState.randomUsername},
                target: {faceMap: faceMapBase64},
                lowQualityAuditTrailImage: zoomSessionResult.faceMetrics.lowQualityAuditTrailCompressedBase64()[0],
                auditTrailImage: zoomSessionResult.faceMetrics.getAuditTrailBase64JPG()[0]
            }
            callToZoomServerForResult("/match-3d-3d", parameters, zoomSessionResult.sessionId, zoomFaceMapResultCallback, function(responseJSON: any){
                var nextStep: UXNextStep = ServerResultHelpers.getAuthenticateNextStep(responseJSON);
                resultCallback(nextStep);
            })
        });
    }

    // Create and send the request.  Parse the results and send the caller what the next step should be (Succeed, Retry, or Cancel).
    function getPhotoIDMatchResponseFromServer(
        zoomIDScanResult: ZoomAuthentication.ZoomIDScanResult,
        zoomIDScanResultCallback: ZoomAuthentication.ZoomIDScanResultCallback,
        resultCallback: (responseJSON: any) => void
        ){
        zoomIDScanResult.idScanMetrics.getIDScanBase64(function(idScanBase64) {
            var parameters: any = {
              sessionId: zoomIDScanResult.sessionId,
              enrollmentIdentifier: ZoomGlobalState.randomUsername,
              idScan: idScanBase64,
              idScanFrontImage: zoomIDScanResult.idScanMetrics.frontImages[0].split(",")[1] || ""
            }
            if(zoomIDScanResult.idScanMetrics.backImages[0]) {
              parameters.idScanBackImage = zoomIDScanResult.idScanMetrics.backImages[0];
            }
            callToZoomServerForResult("/id-check", parameters, zoomIDScanResult.sessionId, zoomIDScanResultCallback, function(responseJSON: any){
                var nextStep: UXNextStep = ServerResultHelpers.getPhotoIDMatchNextStep(responseJSON);
                resultCallback(nextStep);
            })
        })
    }

    // Makes the actual call to the API.
    // Note that for initial integration this sends to the FaceTec Managed Testing API.
    // After deployment of your own instance of ZoOm Server, this will be your own configurable endpoint.
    function callToZoomServerForResult(endpoint: String, parameters: any, sessionId: String | null, zoomResultCallback: ZoomAuthentication.ZoomFaceMapResultCallback | ZoomAuthentication.ZoomIDScanResultCallback, resultCallback: any){
        latestXHR = new XMLHttpRequest();
        latestXHR.open("POST", ZoomGlobalState.ZoomServerBaseURL + endpoint);
        latestXHR.setRequestHeader("X-Device-License-Key", ZoomGlobalState.DeviceLicenseKeyIdentifier);
        latestXHR.setRequestHeader("X-User-Agent", ZoomSDK.createZoomAPIUserAgentString(sessionId as string));
        latestXHR.setRequestHeader("Content-Type", "application/json");
        latestXHR.onreadystatechange = function () {
            if (this.readyState === 4) {
                try {
                    var responseJSON = JSON.parse(this.responseText)
                    resultCallback(responseJSON);
                }
                catch {
                  zoomResultCallback.cancel();
                  return;
                }
            }
        };

        latestXHR.onerror = function() {
            zoomResultCallback.cancel();
        }

        latestXHR.upload.onprogress = function name(event) {
            var progress = event.loaded / event.total;
            zoomResultCallback.uploadProgress(progress)
        }

        var jsonUpload = JSON.stringify(parameters);
        latestXHR.send(jsonUpload);
    }

    function cancelInFlightRequests() {
        latestXHR.abort();
        latestXHR = new XMLHttpRequest();
    }

    return {
        cancelInFlightRequests,
        latestXHR,
        UXNextStep,
        getLivenessCheckResponseFromZoomServer,
        getEnrollmentResponseFromZoomServer,
        getAuthenticateResponseFromZoomServer,
        getPhotoIDMatchResponseFromServer
    }
})()

// Helpers for parsing API response to determine if result was a success vs. user needs retry vs. unexpected (cancel out).
// Developers are encouraged to change API call parameters and results to fit their own.
class ServerResultHelpers {

    // If livenessStatus is Liveness Proven, succeed.  Otherwise fail.  Unexpected responses cancel.
    static getLivenessNextStep(responseJSON: any) {
        if (responseJSON["data"] && responseJSON["data"]["livenessStatus"] == 0) {
            return UXNextStep.Succeed
        }
        else if (responseJSON["data"] && responseJSON["data"]["livenessStatus"] == 1) {
            return UXNextStep.Retry
        }
        else {
            return UXNextStep.Cancel
        }
    }

    // If isEnrolled and livenessStatus is Liveness Proven, succeed.  Otherwise retry.  Unexpected responses cancel.
    static getEnrollmentNextStep(responseJSON: any) {
        if(responseJSON && responseJSON.meta && responseJSON.meta.code === 200 && responseJSON.data && responseJSON.data.isEnrolled && responseJSON.data.livenessStatus === 0) {
            return UXNextStep.Succeed
        }
        else if(responseJSON && responseJSON.meta && responseJSON.meta.code === 200 && responseJSON.data && responseJSON.data.isEnrolled === false) {
            return UXNextStep.Retry
        }
        else {
            return UXNextStep.Cancel
        }
    }

    // If isEnrolled and livenessStatus is Liveness Proven, succeed.  Otherwise retry.  Unexpected responses cancel.
    static getAuthenticateNextStep(responseJSON: any) {
        // If both FaceMaps have Liveness Proven, and Match Level is 10 (1 in 4.2 million), then succeed.  Otherwise retry.  Unexpected responses cancel.
        if(responseJSON && responseJSON.data && responseJSON.data.matchLevel != null && responseJSON.data.matchLevel === 10
            && responseJSON.data.sourceFaceMap && responseJSON.data.sourceFaceMap.livenessStatus === 0
            && responseJSON.data.targetFaceMap && responseJSON.data.targetFaceMap.livenessStatus === 0
         ) {
            return UXNextStep.Succeed
         }
         else if(responseJSON && responseJSON.data
                 && responseJSON.data.sourceFaceMap
                 && responseJSON.data.targetFaceMap
                 && responseJSON.data.matchLevel != null
                 && (responseJSON.data.sourceFaceMap.livenessStatus !== 0 || responseJSON.data.targetFaceMap.livenessStatus !== 0 || responseJSON.data.matchLevel !== 10))
         {
            return UXNextStep.Retry
         }
         else {
           return UXNextStep.Cancel
         }

        }

    // If Liveness Proven on FaceMap, and Match Level between FaceMap and ID Photo is non-zero, then succeed.  Otherwise retry.  Unexpected responses cancel.
    static getPhotoIDMatchNextStep(responseJSON: any){
        // If Liveness Proven on FaceMap, and Match Level between FaceMap and ID Photo is non-zero, then succeed.  Otherwise retry.  Unexpected responses cancel.
        if(responseJSON && responseJSON.meta && responseJSON.meta.ok && responseJSON.meta.ok === true && responseJSON.data
            && responseJSON.data.livenessStatus === 0 && responseJSON.data.matchLevel != null && responseJSON.data.matchLevel !== 0)
          {
            return UXNextStep.Succeed;
          }
          else if(responseJSON && responseJSON.data
            && responseJSON.data.matchLevel != null
            && (responseJSON.data.livenessStatus !== 0 || responseJSON.data.matchLevel === 0))
          {
            return UXNextStep.Retry;
          }
          else {
            return UXNextStep.Cancel;
          }
    }
}
