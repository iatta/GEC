import * as ZoomAuthentication from  '../../../../src/assets/core-sdk/ZoomAuthentication.js/ZoomAuthentication';
const ZoomSDK  = ZoomAuthentication.ZoomSDK;

export const SampleAppUtilities = (() => {

    function displayStatus(message: string) {
      (document.getElementById('status') as HTMLElement).innerHTML = message;
    }

    function showResultStatusAndMainUI(isSuccess: boolean, zoomSessionResult: ZoomAuthentication.ZoomSessionResult ) {
        debugger
      if (isSuccess) {
        displayStatus('Success');
      } else {
        let statusString = 'Unsuccessful. ';
        if (zoomSessionResult.status === ZoomSDK.ZoomSessionStatus.SessionCompletedSuccessfully) {
          statusString += 'Session was completed but an unexpected issue occurred during the network request.';
        } else {
          statusString += ZoomSDK.getFriendlyDescriptionForZoomSessionStatus(zoomSessionResult.status);
        }
        displayStatus(statusString);
      }
    }

    return {
      displayStatus,
      showResultStatusAndMainUI
    };
})();
