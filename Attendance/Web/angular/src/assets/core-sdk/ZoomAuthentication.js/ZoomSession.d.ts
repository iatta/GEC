import { ZoomSessionResult, ZoomFaceMapProcessor, ZoomIDScanProcessor, ZoomIDScanResult } from "./ZoomPublicApi";
/**
 * Types describing the ZoomSession constructor parameters
*/
declare type ZoomSessionParameter1Type = ZoomSessionCompleteFunction;
declare type ZoomSessionParameter2Type = string | ZoomFaceMapProcessor;
declare type ZoomSessionParameter3Type = ZoomIDScanProcessor | boolean | string | undefined;
declare type ZoomSessionParameter4Type = string | boolean | undefined;
/**
 * Zoom IDScan callback function
 */
export interface ZoomIDScanCompleteFunction {
    (zoomIDScanResult?: ZoomIDScanResult): void;
}
/**
 * Zoom Session Complete Function
 */
export interface ZoomSessionCompleteFunction {
    (zoomSessionResult?: ZoomSessionResult): void;
}
/**
 * ZoomSession class
 */
export declare class ZoomSession {
    onZoomSessionCaptureComplete: ZoomSessionCompleteFunction;
    start: () => void;
    constructor(param1: ZoomSessionParameter1Type, param2: ZoomSessionParameter2Type, param3?: ZoomSessionParameter3Type, param4?: ZoomSessionParameter4Type);
}
export {};
