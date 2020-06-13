import * as ZoomAuthentication from "../../../assets/core-sdk/ZoomAuthentication.js/ZoomAuthentication";
import { ZoomCustomization } from "../../../assets/core-sdk/ZoomAuthentication.js/ZoomCustomization";
var ZoomSDK  = ZoomAuthentication.ZoomSDK;
declare var ZoomGlobalState: any;

export var ThemeHelpers = (function() {
    var themeResourceDirectory: string = "../../../../sample-app-resources/images/themes/";

    function setAppTheme(theme: string) {
        ZoomGlobalState.currentCustomization = getCustomizationForTheme(theme);
        var currentLowLightCustomization = getLowLightCustomizationForTheme(theme);

        ZoomSDK.setCustomization(ZoomGlobalState.currentCustomization);
        ZoomSDK.setLowLightCustomization(currentLowLightCustomization);
    }

    function getCustomizationForTheme(theme: string): ZoomCustomization {
        var currentCustomization: ZoomCustomization = new ZoomSDK.ZoomCustomization();

        if(theme === "ZoOm Theme") {
            // using default customizations -- do nothing
        }
        else if(theme === "Well-Rounded") {
            let primaryColor = "rgb(9, 181, 163)"; // green
            let backgroundColor = "white";

            // Overlay Customization
            currentCustomization.overlayCustomization.backgroundColor = "transparent";
            currentCustomization.overlayCustomization.brandingImage = themeResourceDirectory + "blank_image.png";
            currentCustomization.overlayCustomization.foregroundColor = backgroundColor;
           // currentCustomization.overlayCustomization.brightenScreenButtonImage = themeResourceDirectory + "well-rounded/power_button_off_green.png";
            // Guidance Customization
            currentCustomization.guidanceCustomization.backgroundColors = backgroundColor;
            currentCustomization.guidanceCustomization.foregroundColor = primaryColor;
            currentCustomization.guidanceCustomization.buttonTextNormalColor = backgroundColor;
            currentCustomization.guidanceCustomization.buttonBackgroundNormalColor = primaryColor;
            currentCustomization.guidanceCustomization.buttonTextHighlightColor = backgroundColor;
            currentCustomization.guidanceCustomization.buttonBackgroundHighlightColor = primaryColor;
            currentCustomization.guidanceCustomization.buttonBorderColor = "transparent";
            currentCustomization.guidanceCustomization.buttonBorderWidth = "0px";
            currentCustomization.guidanceCustomization.buttonCornerRadius = "30px";
            currentCustomization.guidanceCustomization.readyScreenOvalFillColor = "rgba(9, 181, 163, 0.2)";
            currentCustomization.guidanceCustomization.readyScreenTextBackgroundColor = backgroundColor;
            currentCustomization.guidanceCustomization.readyScreenTextBackgroundCornerRadius = "5px";
            // Guidance Image Customization
            currentCustomization.guidanceCustomization.imageCustomization.cameraPermissionsScreenImage = themeResourceDirectory + "well-rounded/camera_green.png";
            // ID Scan Customization
            currentCustomization.idScanCustomization.selectionScreenBrandingImage = themeResourceDirectory + "blank_image.png";
            currentCustomization.idScanCustomization.showSelectionScreenBrandingImage = false;
            currentCustomization.idScanCustomization.selectionScreenBackgroundColors = backgroundColor;
            currentCustomization.idScanCustomization.reviewScreenBackgroundColors = backgroundColor;
            currentCustomization.idScanCustomization.captureScreenForegroundColor = primaryColor;
            currentCustomization.idScanCustomization.reviewScreenForegroundColor = primaryColor;
            currentCustomization.idScanCustomization.selectionScreenForegroundColor = primaryColor;
            currentCustomization.idScanCustomization.buttonTextNormalColor = backgroundColor;
            currentCustomization.idScanCustomization.buttonBackgroundNormalColor = primaryColor;
            currentCustomization.idScanCustomization.buttonTextHighlightColor = backgroundColor;
            currentCustomization.idScanCustomization.buttonBackgroundHighlightColor = primaryColor;
            currentCustomization.idScanCustomization.buttonBorderColor = "transparent";
            currentCustomization.idScanCustomization.buttonBorderWidth = "0px";
            currentCustomization.idScanCustomization.buttonCornerRadius = "30px";
            currentCustomization.idScanCustomization.captureScreenTextBackgroundColor = backgroundColor;
            currentCustomization.idScanCustomization.captureScreenTextBackgroundBorderColor = primaryColor;
            currentCustomization.idScanCustomization.captureScreenTextBackgroundBorderWidth = "2px";
            currentCustomization.idScanCustomization.captureScreenTextBackgroundCornerRadius = "5px";
            currentCustomization.idScanCustomization.reviewScreenTextBackgroundColor = backgroundColor;
            currentCustomization.idScanCustomization.reviewScreenTextBackgroundBorderColor = primaryColor;
            currentCustomization.idScanCustomization.reviewScreenTextBackgroundBorderWidth = "2px";
            currentCustomization.idScanCustomization.reviewScreenTextBackgroundCornerRadius = "5px";
            currentCustomization.idScanCustomization.captureScreenPassportFrameMobileImage = themeResourceDirectory + "well-rounded/zoom_passport_frame_green_mobile.png";
            currentCustomization.idScanCustomization.captureScreenIDCardFrameMobileImage = themeResourceDirectory + "well-rounded/zoom_id_card_frame_green_mobile.png";
            currentCustomization.idScanCustomization.captureScreenPassportFrameImage = themeResourceDirectory + "well-rounded/zoom_passport_frame_green_desktop.png";
            currentCustomization.idScanCustomization.captureScreenIDCardFrameImage = themeResourceDirectory + "well-rounded/zoom_id_card_frame_green_desktop.png";
            // Result Screen Customization
            currentCustomization.resultScreenCustomization.backgroundColors = backgroundColor;
            currentCustomization.resultScreenCustomization.foregroundColor = primaryColor;
            currentCustomization.resultScreenCustomization.activityIndicatorColor = primaryColor;
            currentCustomization.resultScreenCustomization.customActivityIndicatorImage = themeResourceDirectory + "well-rounded/activity_indicator_green.png";
            currentCustomization.resultScreenCustomization.customActivityIndicatorRotationInterval = "1s";
            currentCustomization.resultScreenCustomization.resultAnimationBackgroundColor = primaryColor;
            currentCustomization.resultScreenCustomization.resultAnimationForegroundColor = backgroundColor;
            currentCustomization.resultScreenCustomization.showUploadProgressBar = false;
            currentCustomization.resultScreenCustomization.uploadProgressTrackColor = "rgba(0, 0, 0, 0.2)";
            currentCustomization.resultScreenCustomization.uploadProgressFillColor = primaryColor;
            // Feedback Customization
            currentCustomization.feedbackCustomization.backgroundColor = primaryColor;
            currentCustomization.feedbackCustomization.textColor = backgroundColor;
            currentCustomization.feedbackCustomization.cornerRadius = "5px";
            // Frame Customization
            currentCustomization.frameCustomization.backgroundColor = backgroundColor
            currentCustomization.frameCustomization.borderColor = primaryColor
            currentCustomization.frameCustomization.borderWidth = "2px";
            currentCustomization.frameCustomization.borderCornerRadius = "20px";
            // Oval Customization
            currentCustomization.ovalCustomization.strokeColor = primaryColor;
            currentCustomization.ovalCustomization.progressColor1 = primaryColor;
            currentCustomization.ovalCustomization.progressColor2 = primaryColor;
            // Cancel Button Customization
            currentCustomization.cancelButtonCustomization.customImage = themeResourceDirectory + "well-rounded/cancel_round_green.png";
        }
        else if(theme === "Dark-Side") {
            let primaryColor = "rgb(0, 174, 237)"; // blue
            let backgroundColor = "black";

            // Overlay Customization
            currentCustomization.overlayCustomization.backgroundColor = backgroundColor;
            currentCustomization.overlayCustomization.brandingImage = themeResourceDirectory + "blank_image.png";
            currentCustomization.overlayCustomization.foregroundColor = primaryColor;
        //    currentCustomization.overlayCustomization.brightenScreenButtonImage = themeResourceDirectory + "dark-side/light_bulb_blue.png";
            // Guidance Customization
            currentCustomization.guidanceCustomization.backgroundColors = backgroundColor;
            currentCustomization.guidanceCustomization.foregroundColor = primaryColor;
            currentCustomization.guidanceCustomization.buttonTextNormalColor = primaryColor;
            currentCustomization.guidanceCustomization.buttonBackgroundNormalColor = "transparent";
            currentCustomization.guidanceCustomization.buttonTextHighlightColor = backgroundColor;
            currentCustomization.guidanceCustomization.buttonBackgroundHighlightColor = primaryColor;
            currentCustomization.guidanceCustomization.buttonBorderColor = primaryColor;
            currentCustomization.guidanceCustomization.buttonBorderWidth = "1px";
            currentCustomization.guidanceCustomization.buttonCornerRadius = "2px";
            currentCustomization.guidanceCustomization.readyScreenOvalFillColor = "rgba(0, 174, 237, 0.2)";
            currentCustomization.guidanceCustomization.readyScreenTextBackgroundColor = backgroundColor;
            currentCustomization.guidanceCustomization.readyScreenTextBackgroundCornerRadius = "5px";
            // Guidance Image Customization
            currentCustomization.guidanceCustomization.imageCustomization.cameraPermissionsScreenImage = themeResourceDirectory + "dark-side/camera_blue.png";
            // ID Scan Customization
            currentCustomization.idScanCustomization.selectionScreenBrandingImage = themeResourceDirectory + "blank_image.png";
            currentCustomization.idScanCustomization.showSelectionScreenBrandingImage = false;
            currentCustomization.idScanCustomization.selectionScreenBackgroundColors = backgroundColor;
            currentCustomization.idScanCustomization.reviewScreenBackgroundColors = backgroundColor;
            currentCustomization.idScanCustomization.captureScreenForegroundColor = primaryColor;
            currentCustomization.idScanCustomization.reviewScreenForegroundColor = primaryColor;
            currentCustomization.idScanCustomization.selectionScreenForegroundColor = primaryColor;
            currentCustomization.idScanCustomization.buttonTextNormalColor = primaryColor;
            currentCustomization.idScanCustomization.buttonBackgroundNormalColor = "transparent";
            currentCustomization.idScanCustomization.buttonTextHighlightColor = backgroundColor
            currentCustomization.idScanCustomization.buttonBackgroundHighlightColor = primaryColor
            currentCustomization.idScanCustomization.buttonBorderColor = primaryColor
            currentCustomization.idScanCustomization.buttonBorderWidth = "1px";
            currentCustomization.idScanCustomization.buttonCornerRadius = "2px";
            currentCustomization.idScanCustomization.captureScreenTextBackgroundColor = "transparent";
            currentCustomization.idScanCustomization.captureScreenTextBackgroundBorderColor = primaryColor;
            currentCustomization.idScanCustomization.captureScreenTextBackgroundBorderWidth = "2px";
            currentCustomization.idScanCustomization.captureScreenTextBackgroundCornerRadius = "8px";
            currentCustomization.idScanCustomization.reviewScreenTextBackgroundColor = "transparent";
            currentCustomization.idScanCustomization.reviewScreenTextBackgroundBorderColor = primaryColor;
            currentCustomization.idScanCustomization.reviewScreenTextBackgroundBorderWidth = "2px";
            currentCustomization.idScanCustomization.reviewScreenTextBackgroundCornerRadius = "8px";
            currentCustomization.idScanCustomization.captureScreenPassportFrameMobileImage = themeResourceDirectory + "dark-side/zoom_passport_frame_blue_mobile.png";
            currentCustomization.idScanCustomization.captureScreenIDCardFrameMobileImage = themeResourceDirectory + "dark-side/zoom_id_card_frame_blue_mobile.png";
            currentCustomization.idScanCustomization.captureScreenPassportFrameImage = themeResourceDirectory + "dark-side/zoom_passport_frame_blue_desktop.png";
            currentCustomization.idScanCustomization.captureScreenIDCardFrameImage = themeResourceDirectory + "dark-side/zoom_id_card_frame_blue_desktop.png";
            // Result Screen Customization
            currentCustomization.resultScreenCustomization.backgroundColors = backgroundColor;
            currentCustomization.resultScreenCustomization.foregroundColor = primaryColor;
            currentCustomization.resultScreenCustomization.activityIndicatorColor = primaryColor;
            currentCustomization.resultScreenCustomization.customActivityIndicatorImage = themeResourceDirectory + "dark-side/activity_indicator_blue.png";
            currentCustomization.resultScreenCustomization.customActivityIndicatorRotationInterval = "2s";
            currentCustomization.resultScreenCustomization.resultAnimationBackgroundColor = primaryColor;
            currentCustomization.resultScreenCustomization.resultAnimationForegroundColor = backgroundColor;
            currentCustomization.resultScreenCustomization.showUploadProgressBar = false;
            currentCustomization.resultScreenCustomization.uploadProgressTrackColor = "rgba(255, 255, 255, 0.2)";
            currentCustomization.resultScreenCustomization.uploadProgressFillColor = primaryColor;
            // Feedback Customization
            currentCustomization.feedbackCustomization.backgroundColor = primaryColor;
            currentCustomization.feedbackCustomization.textColor = backgroundColor;
            currentCustomization.feedbackCustomization.cornerRadius = "8px";
            // Frame Customization
            currentCustomization.frameCustomization.backgroundColor = backgroundColor;
            currentCustomization.frameCustomization.borderColor = primaryColor;
            currentCustomization.frameCustomization.borderWidth = "2px";
            currentCustomization.frameCustomization.borderCornerRadius = "5px";
            // Oval Customization
            currentCustomization.ovalCustomization.strokeColor = primaryColor;
            currentCustomization.ovalCustomization.progressColor1 = "rgba(0, 174, 237, 0.7)";
            currentCustomization.ovalCustomization.progressColor2 = "rgba(0, 174, 237, 0.7)";
            // Cancel Button Customization
            currentCustomization.cancelButtonCustomization.customImage = themeResourceDirectory + "dark-side/double_chevron_left_blue.png";
        }
        else if(theme === "Bitcoin Exchange") {
            let primaryColor = "rgb(247, 150, 52)"; // orange
            let secondaryColor = "rgb(255, 255, 30)"; // yellow
            let backgroundColor = "rgb(66, 66, 66)"; // dark grey

            // Overlay Customization
            currentCustomization.overlayCustomization.backgroundColor = "white";
            currentCustomization.overlayCustomization.brandingImage = themeResourceDirectory + "bitcoin-exchange/bitcoin_exchange_logo.png";
            currentCustomization.overlayCustomization.foregroundColor = primaryColor;
         //   currentCustomization.overlayCustomization.brightenScreenButtonImage = "";
            // Guidance Customization
            currentCustomization.guidanceCustomization.backgroundColors = backgroundColor;
            currentCustomization.guidanceCustomization.foregroundColor = primaryColor;
            currentCustomization.guidanceCustomization.buttonTextNormalColor = backgroundColor;
            currentCustomization.guidanceCustomization.buttonBackgroundNormalColor = primaryColor;
            currentCustomization.guidanceCustomization.buttonTextHighlightColor = backgroundColor;
            currentCustomization.guidanceCustomization.buttonBackgroundHighlightColor = primaryColor;
            currentCustomization.guidanceCustomization.buttonBorderColor = "transparent";
            currentCustomization.guidanceCustomization.buttonBorderWidth = "0px";
            currentCustomization.guidanceCustomization.buttonCornerRadius = "5px";
            currentCustomization.guidanceCustomization.readyScreenOvalFillColor = "rgba(247, 150, 52, 0.2)";
            currentCustomization.guidanceCustomization.readyScreenTextBackgroundColor = backgroundColor;
            currentCustomization.guidanceCustomization.readyScreenTextBackgroundCornerRadius = "5px";
            // Guidance Image Customization
            currentCustomization.guidanceCustomization.imageCustomization.cameraPermissionsScreenImage = themeResourceDirectory + "bitcoin-exchange/camera_orange.png"
            // ID Scan Customization
            currentCustomization.idScanCustomization.selectionScreenBrandingImage = themeResourceDirectory + "blank_image.png";
            currentCustomization.idScanCustomization.showSelectionScreenBrandingImage = false;
            currentCustomization.idScanCustomization.selectionScreenBackgroundColors = backgroundColor;
            currentCustomization.idScanCustomization.reviewScreenBackgroundColors = backgroundColor;
            currentCustomization.idScanCustomization.captureScreenForegroundColor = primaryColor;
            currentCustomization.idScanCustomization.reviewScreenForegroundColor = primaryColor;
            currentCustomization.idScanCustomization.selectionScreenForegroundColor = primaryColor;
            currentCustomization.idScanCustomization.buttonTextNormalColor = backgroundColor;
            currentCustomization.idScanCustomization.buttonBackgroundNormalColor = primaryColor;
            currentCustomization.idScanCustomization.buttonTextHighlightColor = backgroundColor;
            currentCustomization.idScanCustomization.buttonBackgroundHighlightColor = primaryColor;
            currentCustomization.idScanCustomization.buttonBorderColor = "transparent";
            currentCustomization.idScanCustomization.buttonBorderWidth = "0px";
            currentCustomization.idScanCustomization.buttonCornerRadius = "5px";
            currentCustomization.idScanCustomization.captureScreenTextBackgroundColor = backgroundColor;
            currentCustomization.idScanCustomization.captureScreenTextBackgroundBorderColor = primaryColor;
            currentCustomization.idScanCustomization.captureScreenTextBackgroundBorderWidth = "0px";
            currentCustomization.idScanCustomization.captureScreenTextBackgroundCornerRadius = "8px";
            currentCustomization.idScanCustomization.reviewScreenTextBackgroundColor = backgroundColor;
            currentCustomization.idScanCustomization.reviewScreenTextBackgroundBorderColor = primaryColor;
            currentCustomization.idScanCustomization.reviewScreenTextBackgroundBorderWidth = "0px";
            currentCustomization.idScanCustomization.reviewScreenTextBackgroundCornerRadius = "8px";
            currentCustomization.idScanCustomization.captureScreenPassportFrameMobileImage = themeResourceDirectory + "bitcoin-exchange/zoom_passport_frame_orange_mobile.png";
            currentCustomization.idScanCustomization.captureScreenIDCardFrameMobileImage = themeResourceDirectory + "bitcoin-exchange/zoom_id_card_frame_orange_mobile.png";
            currentCustomization.idScanCustomization.captureScreenPassportFrameImage = themeResourceDirectory + "bitcoin-exchange/zoom_passport_frame_orange_desktop.png";
            currentCustomization.idScanCustomization.captureScreenIDCardFrameImage = themeResourceDirectory + "bitcoin-exchange/zoom_id_card_frame_orange_desktop.png";
            // Result Screen Customization
            currentCustomization.resultScreenCustomization.backgroundColors = backgroundColor;
            currentCustomization.resultScreenCustomization.foregroundColor = primaryColor;
            currentCustomization.resultScreenCustomization.activityIndicatorColor = primaryColor;
            currentCustomization.resultScreenCustomization.customActivityIndicatorImage = themeResourceDirectory + "bitcoin-exchange/activity_indicator_orange.png";
            currentCustomization.resultScreenCustomization.customActivityIndicatorRotationInterval = "1.5s";
            currentCustomization.resultScreenCustomization.resultAnimationBackgroundColor = primaryColor;
            currentCustomization.resultScreenCustomization.resultAnimationForegroundColor = "white";
            currentCustomization.resultScreenCustomization.showUploadProgressBar = true;
            currentCustomization.resultScreenCustomization.uploadProgressTrackColor = "rgba(0, 0, 0, 0.2)";
            currentCustomization.resultScreenCustomization.uploadProgressFillColor = primaryColor;
            // Feedback Customization
            currentCustomization.feedbackCustomization.backgroundColor = primaryColor;
            currentCustomization.feedbackCustomization.textColor = backgroundColor;
            currentCustomization.feedbackCustomization.cornerRadius = "5px";
            // Frame Customization
            currentCustomization.frameCustomization.backgroundColor = backgroundColor;
            currentCustomization.frameCustomization.borderColor = secondaryColor;
            currentCustomization.frameCustomization.borderWidth = "3px";
            currentCustomization.frameCustomization.borderCornerRadius = "5px";
            // Oval Customization
            currentCustomization.ovalCustomization.strokeColor = primaryColor;
            currentCustomization.ovalCustomization.progressColor1 = secondaryColor;
            currentCustomization.ovalCustomization.progressColor2 = secondaryColor;
            // Cancel Button Customization
            currentCustomization.cancelButtonCustomization.customImage = themeResourceDirectory + "bitcoin-exchange/back_orange.png";
        }
        else if(theme === "eKYC") {
            let primaryColor = "rgb(237, 28, 36)"; // red
            let secondaryColor = "black";
            let backgroundColor = "white";

            // Overlay Customization
            currentCustomization.overlayCustomization.backgroundColor = "white";
            currentCustomization.overlayCustomization.brandingImage = themeResourceDirectory + "ekyc/ekyc_logo.png";
            currentCustomization.overlayCustomization.foregroundColor = primaryColor;
           // currentCustomization.overlayCustomization.brightenScreenButtonImage = "";
            // Guidance Customization
            currentCustomization.guidanceCustomization.backgroundColors = backgroundColor;
            currentCustomization.guidanceCustomization.foregroundColor = secondaryColor;
            currentCustomization.guidanceCustomization.buttonTextNormalColor = primaryColor;
            currentCustomization.guidanceCustomization.buttonBackgroundNormalColor = "transparent";
            currentCustomization.guidanceCustomization.buttonTextHighlightColor = backgroundColor;
            currentCustomization.guidanceCustomization.buttonBackgroundHighlightColor = primaryColor;
            currentCustomization.guidanceCustomization.buttonBorderColor = primaryColor;
            currentCustomization.guidanceCustomization.buttonBorderWidth = "2px";
            currentCustomization.guidanceCustomization.buttonCornerRadius = "8px";
            currentCustomization.guidanceCustomization.readyScreenOvalFillColor = "rgba(237, 28, 36, 0.2)";
            currentCustomization.guidanceCustomization.readyScreenTextBackgroundColor = backgroundColor;
            currentCustomization.guidanceCustomization.readyScreenTextBackgroundCornerRadius = "3px";
            // Guidance Image Customization
            currentCustomization.guidanceCustomization.imageCustomization.cameraPermissionsScreenImage = themeResourceDirectory + "ekyc/camera_red.png"
            // ID Scan Customization
            currentCustomization.idScanCustomization.selectionScreenBrandingImage = themeResourceDirectory + "ekyc/ekyc_logo.png";
            currentCustomization.idScanCustomization.showSelectionScreenBrandingImage = true;
            currentCustomization.idScanCustomization.selectionScreenBackgroundColors = backgroundColor;
            currentCustomization.idScanCustomization.reviewScreenBackgroundColors = backgroundColor;
            currentCustomization.idScanCustomization.captureScreenForegroundColor = backgroundColor;
            currentCustomization.idScanCustomization.reviewScreenForegroundColor = backgroundColor;
            currentCustomization.idScanCustomization.selectionScreenForegroundColor = primaryColor;
            currentCustomization.idScanCustomization.buttonTextNormalColor = primaryColor;
            currentCustomization.idScanCustomization.buttonBackgroundNormalColor = "transparent";
            currentCustomization.idScanCustomization.buttonTextHighlightColor = backgroundColor;
            currentCustomization.idScanCustomization.buttonBackgroundHighlightColor = primaryColor;
            currentCustomization.idScanCustomization.buttonBorderColor = primaryColor;
            currentCustomization.idScanCustomization.buttonBorderWidth = "2px";
            currentCustomization.idScanCustomization.buttonCornerRadius = "8px";
            currentCustomization.idScanCustomization.captureScreenTextBackgroundColor = primaryColor;
            currentCustomization.idScanCustomization.captureScreenTextBackgroundBorderColor = primaryColor;
            currentCustomization.idScanCustomization.captureScreenTextBackgroundBorderWidth = "0px";
            currentCustomization.idScanCustomization.captureScreenTextBackgroundCornerRadius = "2px";
            currentCustomization.idScanCustomization.reviewScreenTextBackgroundColor = primaryColor;
            currentCustomization.idScanCustomization.reviewScreenTextBackgroundBorderColor = primaryColor;
            currentCustomization.idScanCustomization.reviewScreenTextBackgroundBorderWidth = "0px";
            currentCustomization.idScanCustomization.reviewScreenTextBackgroundCornerRadius = "2px";
            currentCustomization.idScanCustomization.captureScreenPassportFrameMobileImage = themeResourceDirectory + "ekyc/zoom_passport_frame_red_mobile.png";
            currentCustomization.idScanCustomization.captureScreenIDCardFrameMobileImage = themeResourceDirectory + "ekyc/zoom_id_card_frame_red_mobile.png";
            currentCustomization.idScanCustomization.captureScreenPassportFrameImage = themeResourceDirectory + "ekyc/zoom_passport_frame_red_desktop.png";
            currentCustomization.idScanCustomization.captureScreenIDCardFrameImage = themeResourceDirectory + "ekyc/zoom_id_card_frame_red_desktop.png";
            // Result Screen Customization
            currentCustomization.resultScreenCustomization.backgroundColors = backgroundColor;
            currentCustomization.resultScreenCustomization.foregroundColor = secondaryColor;
            currentCustomization.resultScreenCustomization.activityIndicatorColor = primaryColor;
            currentCustomization.resultScreenCustomization.customActivityIndicatorImage = themeResourceDirectory + "ekyc/activity_indicator_red.png";
            currentCustomization.resultScreenCustomization.customActivityIndicatorRotationInterval = "1.5s";
            currentCustomization.resultScreenCustomization.resultAnimationBackgroundColor = primaryColor;
            currentCustomization.resultScreenCustomization.resultAnimationForegroundColor = backgroundColor;
            currentCustomization.resultScreenCustomization.showUploadProgressBar = false;
            currentCustomization.resultScreenCustomization.uploadProgressTrackColor = "rgba(0, 0, 0, 0.2)";
            currentCustomization.resultScreenCustomization.uploadProgressFillColor = primaryColor;
            // Feedback Customization
            currentCustomization.feedbackCustomization.backgroundColor = secondaryColor;
            currentCustomization.feedbackCustomization.textColor = backgroundColor;
            currentCustomization.feedbackCustomization.cornerRadius = "3px";
            // Frame Customization
            currentCustomization.frameCustomization.backgroundColor = backgroundColor;
            currentCustomization.frameCustomization.borderColor = primaryColor;
            currentCustomization.frameCustomization.borderWidth = "2px";
            currentCustomization.frameCustomization.borderCornerRadius = "8px";
            // Oval Customization
            currentCustomization.ovalCustomization.strokeColor = primaryColor;
            currentCustomization.ovalCustomization.progressColor1 = "rgba(237, 28, 36, 0.7)";
            currentCustomization.ovalCustomization.progressColor2 = "rgba(237, 28, 36, 0.7)";
            // Cancel Button Customization
            currentCustomization.cancelButtonCustomization.customImage = themeResourceDirectory + "ekyc/cancel_box_red.png";
        }
        else if(theme == "Sample Bank") {
            let primaryColor = "white";
            let backgroundColor = "rgb(29, 23, 79)"; // navy

            // Overlay Customization
            currentCustomization.overlayCustomization.backgroundColor = primaryColor;
            currentCustomization.overlayCustomization.brandingImage = themeResourceDirectory + "sample-bank/sample_bank_logo.png";
            currentCustomization.overlayCustomization.foregroundColor = backgroundColor;
           // currentCustomization.overlayCustomization.brightenScreenButtonImage = "";
            // Guidance Customization
            currentCustomization.guidanceCustomization.backgroundColors = backgroundColor;
            currentCustomization.guidanceCustomization.foregroundColor = primaryColor;
            currentCustomization.guidanceCustomization.buttonTextNormalColor = backgroundColor;
            currentCustomization.guidanceCustomization.buttonBackgroundNormalColor = primaryColor;
            currentCustomization.guidanceCustomization.buttonTextHighlightColor = backgroundColor;
            currentCustomization.guidanceCustomization.buttonBackgroundHighlightColor = "rgba(255, 255, 255, 0.8)";
            currentCustomization.guidanceCustomization.buttonBorderColor = backgroundColor;
            currentCustomization.guidanceCustomization.buttonBorderWidth = "2px";
            currentCustomization.guidanceCustomization.buttonCornerRadius = "2px";
            currentCustomization.guidanceCustomization.readyScreenOvalFillColor = "rgba(255, 255, 255, 0.2)";
            currentCustomization.guidanceCustomization.readyScreenTextBackgroundColor = backgroundColor;
            currentCustomization.guidanceCustomization.readyScreenTextBackgroundCornerRadius = "2px";
            // Guidance Image Customization
            currentCustomization.guidanceCustomization.imageCustomization.cameraPermissionsScreenImage = themeResourceDirectory + "sample-bank/camera_white_navy.png"
            // ID Scan Customization
            currentCustomization.idScanCustomization.selectionScreenBrandingImage = themeResourceDirectory + "blank_image.png";
            currentCustomization.idScanCustomization.showSelectionScreenBrandingImage = false;
            currentCustomization.idScanCustomization.selectionScreenBackgroundColors = backgroundColor;
            currentCustomization.idScanCustomization.reviewScreenBackgroundColors = backgroundColor;
            currentCustomization.idScanCustomization.captureScreenForegroundColor = backgroundColor;
            currentCustomization.idScanCustomization.reviewScreenForegroundColor = backgroundColor;
            currentCustomization.idScanCustomization.selectionScreenForegroundColor = primaryColor;
            currentCustomization.idScanCustomization.buttonTextNormalColor = backgroundColor;
            currentCustomization.idScanCustomization.buttonBackgroundNormalColor = primaryColor;
            currentCustomization.idScanCustomization.buttonTextHighlightColor = backgroundColor;
            currentCustomization.idScanCustomization.buttonBackgroundHighlightColor = "rgba(255, 255, 255, 0.8)";
            currentCustomization.idScanCustomization.buttonBorderColor = backgroundColor;
            currentCustomization.idScanCustomization.buttonBorderWidth = "2px";
            currentCustomization.idScanCustomization.buttonCornerRadius = "2px";
            currentCustomization.idScanCustomization.captureScreenTextBackgroundColor = primaryColor;
            currentCustomization.idScanCustomization.captureScreenTextBackgroundBorderColor = backgroundColor;
            currentCustomization.idScanCustomization.captureScreenTextBackgroundBorderWidth = "2px";
            currentCustomization.idScanCustomization.captureScreenTextBackgroundCornerRadius = "2px";
            currentCustomization.idScanCustomization.reviewScreenTextBackgroundColor = primaryColor;
            currentCustomization.idScanCustomization.reviewScreenTextBackgroundBorderColor = backgroundColor;
            currentCustomization.idScanCustomization.reviewScreenTextBackgroundBorderWidth = "2px";
            currentCustomization.idScanCustomization.reviewScreenTextBackgroundCornerRadius = "2px";
            currentCustomization.idScanCustomization.captureScreenPassportFrameMobileImage = themeResourceDirectory + "sample-bank/zoom_passport_frame_navy_mobile.png";
            currentCustomization.idScanCustomization.captureScreenIDCardFrameMobileImage = themeResourceDirectory + "sample-bank/zoom_id_card_frame_navy_mobile.png";
            currentCustomization.idScanCustomization.captureScreenPassportFrameImage = themeResourceDirectory + "sample-bank/zoom_passport_frame_navy_desktop.png";
            currentCustomization.idScanCustomization.captureScreenIDCardFrameImage = themeResourceDirectory + "sample-bank/zoom_id_card_frame_navy_desktop.png";
            // Result Screen Customization
            currentCustomization.resultScreenCustomization.backgroundColors = backgroundColor;
            currentCustomization.resultScreenCustomization.foregroundColor = primaryColor;
            currentCustomization.resultScreenCustomization.activityIndicatorColor = primaryColor;
            currentCustomization.resultScreenCustomization.customActivityIndicatorImage = themeResourceDirectory + "sample-bank/activity_indicator_white.png";
            currentCustomization.resultScreenCustomization.customActivityIndicatorRotationInterval = "1s";
            currentCustomization.resultScreenCustomization.resultAnimationBackgroundColor = primaryColor;
            currentCustomization.resultScreenCustomization.resultAnimationForegroundColor = backgroundColor;
            currentCustomization.resultScreenCustomization.showUploadProgressBar = true;
            currentCustomization.resultScreenCustomization.uploadProgressTrackColor = "rgba(255, 255, 255, 0.2)";
            currentCustomization.resultScreenCustomization.uploadProgressFillColor = primaryColor;
            // Feedback Customization
            currentCustomization.feedbackCustomization.backgroundColor = primaryColor;
            currentCustomization.feedbackCustomization.textColor = backgroundColor;
            currentCustomization.feedbackCustomization.cornerRadius = "2px";
            // Frame Customization
            currentCustomization.frameCustomization.backgroundColor = backgroundColor;
            currentCustomization.frameCustomization.borderColor = backgroundColor;
            currentCustomization.frameCustomization.borderWidth = "2px";
            currentCustomization.frameCustomization.borderCornerRadius = "2px";
            // Oval Customization
            currentCustomization.ovalCustomization.strokeColor = primaryColor;
            currentCustomization.ovalCustomization.progressColor1 = "rgba(255, 255, 255, 0.8)";
            currentCustomization.ovalCustomization.progressColor2 = "rgba(255, 255, 255, 0.8)";
            // Cancel Button Customization
            currentCustomization.cancelButtonCustomization.customImage = themeResourceDirectory + "sample-bank/cancel_white.png";
        }

        return currentCustomization;
    }

    function getLowLightCustomizationForTheme(theme: string): ZoomCustomization|null {
        var currentLowLightCustomization: ZoomCustomization|null = getCustomizationForTheme(theme);

        if(theme === "ZoOm Theme") {
            currentLowLightCustomization = null;
        }
        else if(theme === "Well-Rounded") {
            currentLowLightCustomization = null;
        }
        else if(theme === "Dark-Side") {
            let primaryColor = "rgb(0, 174, 237)"; // blue

            // Overlay Customization
            currentLowLightCustomization.overlayCustomization.brandingImage = themeResourceDirectory + "blank_image.png";
            currentLowLightCustomization.overlayCustomization.foregroundColor = primaryColor;
            // Guidance Customization
            currentLowLightCustomization.guidanceCustomization.foregroundColor = primaryColor;
            currentLowLightCustomization.guidanceCustomization.buttonTextNormalColor = primaryColor;
            currentLowLightCustomization.guidanceCustomization.buttonBackgroundNormalColor = "transparent";
            currentLowLightCustomization.guidanceCustomization.buttonTextHighlightColor = "white";
            currentLowLightCustomization.guidanceCustomization.buttonBackgroundHighlightColor = primaryColor;
            currentLowLightCustomization.guidanceCustomization.buttonBorderColor = primaryColor;
            currentLowLightCustomization.guidanceCustomization.readyScreenOvalFillColor = "rgba(0, 174, 237, 0.2)";
            currentLowLightCustomization.guidanceCustomization.readyScreenTextBackgroundColor = "white";
            // ID Scan Customization
            currentLowLightCustomization.idScanCustomization.selectionScreenBrandingImage = themeResourceDirectory + "blank_image.png";
            currentLowLightCustomization.idScanCustomization.captureScreenForegroundColor = primaryColor;
            currentLowLightCustomization.idScanCustomization.reviewScreenForegroundColor = primaryColor;
            currentLowLightCustomization.idScanCustomization.selectionScreenForegroundColor = primaryColor;
            currentLowLightCustomization.idScanCustomization.buttonTextNormalColor = primaryColor;
            currentLowLightCustomization.idScanCustomization.buttonBackgroundNormalColor = "transparent";
            currentLowLightCustomization.idScanCustomization.buttonTextHighlightColor = "white";
            currentLowLightCustomization.idScanCustomization.buttonBackgroundHighlightColor = primaryColor
            currentLowLightCustomization.idScanCustomization.buttonBorderColor = primaryColor
            currentLowLightCustomization.idScanCustomization.captureScreenTextBackgroundColor = "transparent";
            currentLowLightCustomization.idScanCustomization.captureScreenTextBackgroundBorderColor = primaryColor;
            currentLowLightCustomization.idScanCustomization.reviewScreenTextBackgroundColor = "transparent";
            currentLowLightCustomization.idScanCustomization.reviewScreenTextBackgroundBorderColor = primaryColor;
            currentLowLightCustomization.idScanCustomization.captureScreenPassportFrameMobileImage = themeResourceDirectory + "dark-side/zoom_passport_frame_blue_mobile.png";
            currentLowLightCustomization.idScanCustomization.captureScreenIDCardFrameMobileImage = themeResourceDirectory + "dark-side/zoom_id_card_frame_blue_mobile.png";
            currentLowLightCustomization.idScanCustomization.captureScreenPassportFrameImage = themeResourceDirectory + "dark-side/zoom_passport_frame_blue_desktop.png";
            currentLowLightCustomization.idScanCustomization.captureScreenIDCardFrameImage = themeResourceDirectory + "dark-side/zoom_id_card_frame_blue_desktop.png";
            // Result Screen Customization
            currentLowLightCustomization.resultScreenCustomization.foregroundColor = primaryColor;
            currentLowLightCustomization.resultScreenCustomization.activityIndicatorColor = primaryColor;
            currentLowLightCustomization.resultScreenCustomization.customActivityIndicatorImage = themeResourceDirectory + "dark-side/activity_indicator_blue.png";
            currentLowLightCustomization.resultScreenCustomization.resultAnimationBackgroundColor = primaryColor;
            currentLowLightCustomization.resultScreenCustomization.resultAnimationForegroundColor = "white";
            currentLowLightCustomization.resultScreenCustomization.uploadProgressTrackColor = "rgba(0, 0, 0, 0.2)";
            currentLowLightCustomization.resultScreenCustomization.uploadProgressFillColor = primaryColor;
            // Feedback Customization
            currentLowLightCustomization.feedbackCustomization.backgroundColor = primaryColor;
            currentLowLightCustomization.feedbackCustomization.textColor = "white";
            // Frame Customization
            currentLowLightCustomization.frameCustomization.borderColor = primaryColor;
            // Oval Customization
            currentLowLightCustomization.ovalCustomization.strokeColor = primaryColor;
            currentLowLightCustomization.ovalCustomization.progressColor1 = "rgba(0, 174, 237, 0.7)";
            currentLowLightCustomization.ovalCustomization.progressColor2 = "rgba(0, 174, 237, 0.7)";
            // Cancel Button Customization
            currentLowLightCustomization.cancelButtonCustomization.customImage = themeResourceDirectory + "dark-side/double_chevron_left_blue.png";
        }
        else if(theme === "Bitcoin Exchange") {
            let primaryColor = "rgb(247, 150, 52)"; // orange
            let secondaryColor = "rgb(255, 255, 30)"; // yellow
            let backgroundColor = "rgb(66, 66, 66)"; // dark grey

            // Overlay Customization
            currentLowLightCustomization.overlayCustomization.brandingImage = themeResourceDirectory + "bitcoin-exchange/bitcoin_exchange_logo.png";
            currentLowLightCustomization.overlayCustomization.foregroundColor = primaryColor;
            // Guidance Customization
            currentLowLightCustomization.guidanceCustomization.foregroundColor = backgroundColor;
            currentLowLightCustomization.guidanceCustomization.buttonTextNormalColor = "white";
            currentLowLightCustomization.guidanceCustomization.buttonBackgroundNormalColor = primaryColor;
            currentLowLightCustomization.guidanceCustomization.buttonTextHighlightColor = "white";
            currentLowLightCustomization.guidanceCustomization.buttonBackgroundHighlightColor = "rgba(247, 150, 52, 0.8)";
            currentLowLightCustomization.guidanceCustomization.buttonBorderColor = "transparent";
            currentLowLightCustomization.guidanceCustomization.readyScreenOvalFillColor = "rgba(247, 150, 52, 0.2)";
            currentLowLightCustomization.guidanceCustomization.readyScreenTextBackgroundColor = "white";
            // ID Scan Customization
            currentLowLightCustomization.idScanCustomization.selectionScreenBrandingImage = themeResourceDirectory + "blank_image.png";
            currentLowLightCustomization.idScanCustomization.captureScreenForegroundColor = primaryColor;
            currentLowLightCustomization.idScanCustomization.reviewScreenForegroundColor = primaryColor;
            currentLowLightCustomization.idScanCustomization.selectionScreenForegroundColor = primaryColor;
            currentLowLightCustomization.idScanCustomization.buttonTextNormalColor = "white";
            currentLowLightCustomization.idScanCustomization.buttonBackgroundNormalColor = primaryColor;
            currentLowLightCustomization.idScanCustomization.buttonTextHighlightColor = "white";
            currentLowLightCustomization.idScanCustomization.buttonBackgroundHighlightColor = "rgba(247, 150, 52, 0.8)";
            currentLowLightCustomization.idScanCustomization.buttonBorderColor = "transparent";
            currentLowLightCustomization.idScanCustomization.captureScreenTextBackgroundColor = backgroundColor;
            currentLowLightCustomization.idScanCustomization.captureScreenTextBackgroundBorderColor = "transparent";
            currentLowLightCustomization.idScanCustomization.reviewScreenTextBackgroundColor = backgroundColor;
            currentLowLightCustomization.idScanCustomization.reviewScreenTextBackgroundBorderColor = "transparent";
            currentLowLightCustomization.idScanCustomization.captureScreenPassportFrameMobileImage = themeResourceDirectory + "bitcoin-exchange/zoom_passport_frame_orange_mobile.png";
            currentLowLightCustomization.idScanCustomization.captureScreenIDCardFrameMobileImage = themeResourceDirectory + "bitcoin-exchange/zoom_id_card_frame_orange_mobile.png";
            currentLowLightCustomization.idScanCustomization.captureScreenPassportFrameImage = themeResourceDirectory + "bitcoin-exchange/zoom_passport_frame_orange_desktop.png";
            currentLowLightCustomization.idScanCustomization.captureScreenIDCardFrameImage = themeResourceDirectory + "bitcoin-exchange/zoom_id_card_frame_orange_desktop.png";
            // Result Screen Customization
            currentLowLightCustomization.resultScreenCustomization.foregroundColor = backgroundColor;
            currentLowLightCustomization.resultScreenCustomization.activityIndicatorColor = primaryColor;
            currentLowLightCustomization.resultScreenCustomization.customActivityIndicatorImage = themeResourceDirectory + "bitcoin-exchange/activity_indicator_orange.png";
            currentLowLightCustomization.resultScreenCustomization.resultAnimationBackgroundColor = primaryColor;
            currentLowLightCustomization.resultScreenCustomization.resultAnimationForegroundColor = "white";
            currentLowLightCustomization.resultScreenCustomization.uploadProgressTrackColor = "rgba(0, 0, 0, 0.2)";
            currentLowLightCustomization.resultScreenCustomization.uploadProgressFillColor = primaryColor;
            // Feedback Customization
            currentLowLightCustomization.feedbackCustomization.backgroundColor = backgroundColor;
            currentLowLightCustomization.feedbackCustomization.textColor = "white";
            // Frame Customization
            currentLowLightCustomization.frameCustomization.borderColor = backgroundColor;
            // Oval Customization
            currentLowLightCustomization.ovalCustomization.strokeColor = primaryColor;
            currentLowLightCustomization.ovalCustomization.progressColor1 = secondaryColor;
            currentLowLightCustomization.ovalCustomization.progressColor2 = secondaryColor;
            // Cancel Button Customization
            currentLowLightCustomization.cancelButtonCustomization.customImage = themeResourceDirectory + "bitcoin-exchange/back_orange.png";
        }
        else if(theme === "eKYC") {
            currentLowLightCustomization = null;
        }
        else if(theme == "Sample Bank") {
            let primaryColor = "white";
            let backgroundColor = "rgb(29, 23, 79)"; // navy

            // Overlay Customization
            currentLowLightCustomization.overlayCustomization.brandingImage = themeResourceDirectory + "sample-bank/sample_bank_logo.png";
            currentLowLightCustomization.overlayCustomization.foregroundColor = backgroundColor;
            // Guidance Customization
            currentLowLightCustomization.guidanceCustomization.foregroundColor = backgroundColor;
            currentLowLightCustomization.guidanceCustomization.buttonTextNormalColor = primaryColor;
            currentLowLightCustomization.guidanceCustomization.buttonBackgroundNormalColor = backgroundColor;
            currentLowLightCustomization.guidanceCustomization.buttonTextHighlightColor = primaryColor;
            currentLowLightCustomization.guidanceCustomization.buttonBackgroundHighlightColor = "rgba(29, 23, 79, 0.8)";
            currentLowLightCustomization.guidanceCustomization.buttonBorderColor = backgroundColor;
            currentLowLightCustomization.guidanceCustomization.readyScreenOvalFillColor = "rgba(255, 255, 255, 0.2)";
            currentLowLightCustomization.guidanceCustomization.readyScreenTextBackgroundColor = primaryColor;
            // ID Scan Customization
            currentLowLightCustomization.idScanCustomization.selectionScreenBrandingImage = themeResourceDirectory + "blank_image.png";
            currentLowLightCustomization.idScanCustomization.captureScreenForegroundColor = backgroundColor;
            currentLowLightCustomization.idScanCustomization.reviewScreenForegroundColor = backgroundColor;
            currentLowLightCustomization.idScanCustomization.selectionScreenForegroundColor = backgroundColor;
            currentLowLightCustomization.idScanCustomization.buttonTextNormalColor = primaryColor;
            currentLowLightCustomization.idScanCustomization.buttonBackgroundNormalColor = backgroundColor;
            currentLowLightCustomization.idScanCustomization.buttonTextHighlightColor = primaryColor;
            currentLowLightCustomization.idScanCustomization.buttonBackgroundHighlightColor = "rgba(29, 23, 79, 0.8)";
            currentLowLightCustomization.idScanCustomization.buttonBorderColor = backgroundColor;
            currentLowLightCustomization.idScanCustomization.captureScreenTextBackgroundColor = primaryColor;
            currentLowLightCustomization.idScanCustomization.captureScreenTextBackgroundBorderColor = backgroundColor;
            currentLowLightCustomization.idScanCustomization.reviewScreenTextBackgroundColor = primaryColor;
            currentLowLightCustomization.idScanCustomization.reviewScreenTextBackgroundBorderColor = backgroundColor;
            currentLowLightCustomization.idScanCustomization.captureScreenPassportFrameMobileImage = themeResourceDirectory + "sample-bank/zoom_passport_frame_navy_mobile.png";
            currentLowLightCustomization.idScanCustomization.captureScreenIDCardFrameMobileImage = themeResourceDirectory + "sample-bank/zoom_id_card_frame_navy_mobile.png";
            currentLowLightCustomization.idScanCustomization.captureScreenPassportFrameImage = themeResourceDirectory + "sample-bank/zoom_passport_frame_navy_desktop.png";
            currentLowLightCustomization.idScanCustomization.captureScreenIDCardFrameImage = themeResourceDirectory + "sample-bank/zoom_id_card_frame_navy_desktop.png";
            // Result Screen Customization
            currentLowLightCustomization.resultScreenCustomization.foregroundColor = backgroundColor;
            currentLowLightCustomization.resultScreenCustomization.activityIndicatorColor = backgroundColor;
            currentLowLightCustomization.resultScreenCustomization.customActivityIndicatorImage = themeResourceDirectory + "sample-bank/activity_indicator_navy.png";
            currentLowLightCustomization.resultScreenCustomization.resultAnimationBackgroundColor = backgroundColor;
            currentLowLightCustomization.resultScreenCustomization.resultAnimationForegroundColor = primaryColor;
            currentLowLightCustomization.resultScreenCustomization.uploadProgressTrackColor = "rgba(0, 0, 0, 0.2)";
            currentLowLightCustomization.resultScreenCustomization.uploadProgressFillColor = backgroundColor;
            // Feedback Customization
            currentLowLightCustomization.feedbackCustomization.backgroundColor = backgroundColor;
            currentLowLightCustomization.feedbackCustomization.textColor = primaryColor;
            // Frame Customization
            currentLowLightCustomization.frameCustomization.borderColor = backgroundColor;
            // Oval Customization
            currentLowLightCustomization.ovalCustomization.strokeColor = backgroundColor;
            currentLowLightCustomization.ovalCustomization.progressColor1 = "rgba(29, 23, 79, 0.8)";
            currentLowLightCustomization.ovalCustomization.progressColor2 = "rgba(29, 23, 79, 0.8)";
            // Cancel Button Customization
            currentLowLightCustomization.cancelButtonCustomization.customImage = themeResourceDirectory + "sample-bank/cancel_navy.png";
        }

        return currentLowLightCustomization;
    }

    return {
        themeResourceDirectory,
        setAppTheme: function(theme:string) {
            setAppTheme(theme);
        }
    }
})()
