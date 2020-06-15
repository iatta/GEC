var ZoomGlobalState = (function () {
    // Replace string below with your license key from https://dev.zoomlogin.com/zoomsdk/#/account
    var DeviceLicenseKeyIdentifier = "dM0IXuRAuSpfvyZxo6hJNOdgQmlOjqE6";
    // "https://api.zoomauth.com/api/v2/biometrics" for FaceTec Managed Testing API.
    // "http://localhost:8080" if running ZoOm Server SDK (Dockerized) locally.
    // Otherwise, your webservice URL.
    var ZoomServerBaseURL = "https://api.zoomauth.com/api/v2/biometrics";
    // The customer-controlled public key used during encryption of FaceMap data.
    // Please see https://dev.zoomlogin.com/zoomsdk/#/licensing-and-encryption-keys for more information.
    var PublicFaceMapEncryptionKey = "-----BEGIN PUBLIC KEY-----\n" +
        "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA5PxZ3DLj+zP6T6HFgzzk\n" +
        "M77LdzP3fojBoLasw7EfzvLMnJNUlyRb5m8e5QyyJxI+wRjsALHvFgLzGwxM8ehz\n" +
        "DqqBZed+f4w33GgQXFZOS4AOvyPbALgCYoLehigLAbbCNTkeY5RDcmmSI/sbp+s6\n" +
        "mAiAKKvCdIqe17bltZ/rfEoL3gPKEfLXeN549LTj3XBp0hvG4loQ6eC1E1tRzSkf\n" +
        "GJD4GIVvR+j12gXAaftj3ahfYxioBH7F7HQxzmWkwDyn3bqU54eaiB7f0ftsPpWM\n" +
        "ceUaqkL2DZUvgN0efEJjnWy5y1/Gkq5GGWCROI9XG/SwXJ30BbVUehTbVcD70+ZF\n" +
        "8QIDAQAB\n" +
        "-----END PUBLIC KEY-----";
    // You can paste the license text here, while preserving line breaks.
    // If SFTPLicenseText is null, it will attempt to initialize via FaceTec https
    var SFTPLicenseText = null

    // Used for bookkeeping around demonstrating enrollment/authentication functionality of ZoOm.
    var randomUsername = "";
    var userIdentifier ="PADA";
    var isUserEnrolled = false;
    var isRandomUsernameEnrolled = false;

    var currentCustomization;
    return {
        DeviceLicenseKeyIdentifier: DeviceLicenseKeyIdentifier,
        ZoomServerBaseURL: ZoomServerBaseURL,
        PublicFaceMapEncryptionKey: PublicFaceMapEncryptionKey,
        randomUsername: randomUsername,
        userIdentifier : userIdentifier,
        isUserEnrolled :isUserEnrolled,
        isRandomUsernameEnrolled: isRandomUsernameEnrolled,
        SFTPLicenseText: SFTPLicenseText,
        currentCustomization
    };
})();
