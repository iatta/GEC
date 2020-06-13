
import { mergeMap as _observableMergeMap, catchError as _observableCatch } from 'rxjs/operators';
import { Observable, throwError as _observableThrow, of as _observableOf } from 'rxjs';
import { Injectable, Inject, Optional, InjectionToken } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse, HttpResponseBase } from '@angular/common/http';
import {API_BASE_URL as BASE,ApiException,TimeProfilesServiceProxy,CreateOrEditEmployeeWarningDto,CreateTimeProfileFromExcelDto,CreateOrEditEmployeeVacationDto,UserReportDto,InOutReportOutput,EmployeeReportOutput,PermitReportOutput,FingerReportOutput,ReportInput} from './service-proxies';

import * as moment from 'moment';

export const API_BASE_URL = BASE;

@Injectable()
export class CustomServiceProxy{
    private http: HttpClient;
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(@Inject(HttpClient) http: HttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        this.http = http;
        this.baseUrl = baseUrl ? baseUrl : "";
    }

    createTimeProfileFromExcel(body: CreateTimeProfileFromExcelDto[] | undefined): Observable<void> {
        let url_ = this.baseUrl + "/api/services/app/TimeProfiles/CreateTimeProfileFromExcel";
        url_ = url_.replace(/[?&]$/, "");

        const content_ = JSON.stringify(body);

        let options_ : any = {
            body: content_,
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Content-Type": "application/json-patch+json",
            })
        };

        return this.http.request("post", url_, options_).pipe(_observableMergeMap((response_ : any) => {
            return this.processTimeProfileFromExcel(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processTimeProfileFromExcel(<any>response_);
                } catch (e) {
                    return <Observable<void>><any>_observableThrow(e);
                }
            } else
                return <Observable<void>><any>_observableThrow(response_);
        }));
    }

    protected processTimeProfileFromExcel(response: HttpResponseBase): Observable<void> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
            (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }};
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return _observableOf<void>(<any>null);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<void>(<any>null);
    }


    createEmployeeVacationFromExcel(body: CreateOrEditEmployeeVacationDto[] | undefined): Observable<void> {
        let url_ = this.baseUrl + "/api/services/app/EmployeeVacations/CreateFromExcel";
        url_ = url_.replace(/[?&]$/, "");

        const content_ = JSON.stringify(body);

        let options_ : any = {
            body: content_,
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Content-Type": "application/json-patch+json",
            })
        };

        return this.http.request("post", url_, options_).pipe(_observableMergeMap((response_ : any) => {
            return this.processCreateFromExcel(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processCreateFromExcel(<any>response_);
                } catch (e) {
                    return <Observable<void>><any>_observableThrow(e);
                }
            } else
                return <Observable<void>><any>_observableThrow(response_);
        }));
    }

    protected processCreateFromExcel(response: HttpResponseBase): Observable<void> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
            (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }};
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return _observableOf<void>(<any>null);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<void>(<any>null);
    }


    getUsersByShiftIdReport(shiftId: number): Observable<UserReportDto[]>{
        let url_ = this.baseUrl + "/api/services/app/User/GetUsersByShiftId?";
        if (shiftId === null)
            throw new Error("The parameter 'shiftId' cannot be null.");
        else if (shiftId !== undefined)
            url_ += "shiftId=" + encodeURIComponent("" + shiftId) + "&";

        url_ = url_.replace(/[?&]$/, "");

        let options_ : any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "text/plain"
            })
        };

        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_ : any) => {
            return this.processGetUsersByShiftIdReport(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetUsersByShiftIdReport(<any>response_);
                } catch (e) {
                    return <Observable<UserReportDto[]>><any>_observableThrow(e);
                }
            } else
                return <Observable<UserReportDto[]>><any>_observableThrow(response_);
        }));
    }
    protected processGetUsersByShiftIdReport(response: HttpResponseBase): Observable<UserReportDto[]> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
            (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }};
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            if (Array.isArray(resultData200)) {
                result200 = [] as any;
                for (let item of resultData200)
                    result200!.push(UserReportDto.fromJS(item));
            }
            return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<UserReportDto[]>(<any>null);
    }


    getPdf():Observable<any>{
        let url_ = this.baseUrl + "/report";
        let options_ : any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "text/plain"
            })
        };

        return this.http.get<any>(url_, options_);

    }
    calculateDaysReport(input: ReportInput | undefined): Observable<EmployeeReportOutput[]> {
        debugger
        let url_ = this.baseUrl + "/api/services/app/User/CalculateDaysReport";
        if (input === null)
            throw new Error("The parameter 'filter' cannot be null.");


        url_ = url_.replace(/[?&]$/, "");
        const content_ = JSON.stringify(input);
        let options_ : any = {
            body: content_,
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Content-Type": "application/json-patch+json",
            })
        };

        return this.http.request("post", url_, options_).pipe(_observableMergeMap((response_ : any) => {
            return this.processCalculateDaysReport(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processCalculateDaysReport(<any>response_);
                } catch (e) {
                    return <Observable<EmployeeReportOutput[]>><any>_observableThrow(e);
                }
            } else
                return <Observable<EmployeeReportOutput[]>><any>_observableThrow(response_);
        }));
    }

    protected processCalculateDaysReport(response: HttpResponseBase): Observable<EmployeeReportOutput[]> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
            (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }};
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            if (Array.isArray(resultData200)) {
                result200 = [] as any;
                for (let item of resultData200)
                    result200!.push(EmployeeReportOutput.fromJS(item));
            }

            return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<EmployeeReportOutput[]>(<any>null);
    }

    generatePermitReport(input: ReportInput | undefined): Observable<PermitReportOutput[]> {
        debugger
        let url_ = this.baseUrl + "/api/services/app/User/GeneratePermitReport";
        if (input === null)
            throw new Error("The parameter 'filter' cannot be null.");


        url_ = url_.replace(/[?&]$/, "");
        const content_ = JSON.stringify(input);
        let options_ : any = {
            body: content_,
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Content-Type": "application/json-patch+json",
            })
        };

        return this.http.request("post", url_, options_).pipe(_observableMergeMap((response_ : any) => {
            return this.processGeneratePermitReport(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGeneratePermitReport(<any>response_);
                } catch (e) {
                    return <Observable<PermitReportOutput[]>><any>_observableThrow(e);
                }
            } else
                return <Observable<PermitReportOutput[]>><any>_observableThrow(response_);
        }));
    }

    protected processGeneratePermitReport(response: HttpResponseBase): Observable<PermitReportOutput[]> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
            (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }};
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            if (Array.isArray(resultData200)) {
                result200 = [] as any;
                for (let item of resultData200)
                    result200!.push(PermitReportOutput.fromJS(item));
            }

            return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<PermitReportOutput[]>(<any>null);
    }

    generateWarningeport(input: CreateOrEditEmployeeWarningDto | undefined): Observable<any> {
        let url_ = this.baseUrl + "/Report/GenerateWarningeport";
        if (input === null)
            throw new Error("The parameter 'filter' cannot be null.");


        url_ = url_.replace(/[?&]$/, "");
        const content_ = JSON.stringify(input);
        let options_ : any = {
            body: content_,
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Content-Type": "application/json-patch+json",
            })
        };

        return this.http.request("post", url_, options_);
    }
    generateFingerReport(input: ReportInput | undefined): Observable<any> {
        let url_ = this.baseUrl + "/Report/GenerateFingerReport";
        if (input === null)
            throw new Error("The parameter 'filter' cannot be null.");


        url_ = url_.replace(/[?&]$/, "");
        const content_ = JSON.stringify(input);
        let options_ : any = {
            body: content_,
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Content-Type": "application/json-patch+json",
            })
        };

        return this.http.request("post", url_, options_);
    }

    protected processGenerateFingerReport(response: HttpResponseBase): Observable<FingerReportOutput[]> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
            (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }};
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            if (Array.isArray(resultData200)) {
                result200 = [] as any;
                for (let item of resultData200)
                    result200!.push(FingerReportOutput.fromJS(item));
            }

            return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<FingerReportOutput[]>(<any>null);
    }



    generateInOutReport(input: ReportInput | undefined): Observable<any> {
        debugger
        let url_ = this.baseUrl + "/Report/GenerateInOutReport";
        if (input === null)
            throw new Error("The parameter 'filter' cannot be null.");


        url_ = url_.replace(/[?&]$/, "");
        const content_ = JSON.stringify(input);
        let options_ : any = {
            body: content_,
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Content-Type": "application/json-patch+json",
            })
        };

        return this.http.request("post", url_, options_);
    }

    generateForgetInOutReport(input: ReportInput | undefined): Observable<any> {
        debugger
        let url_ = this.baseUrl + "/Report/GenerateForgetInOutReport";
        if (input === null)
            throw new Error("The parameter 'filter' cannot be null.");


        url_ = url_.replace(/[?&]$/, "");
        const content_ = JSON.stringify(input);
        let options_ : any = {
            body: content_,
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Content-Type": "application/json-patch+json",
            })
        };

        return this.http.request("post", url_, options_);
    }

    protected processGenerateInOutReport(response: HttpResponseBase): Observable<InOutReportOutput[]> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
            (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }};
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            if (Array.isArray(resultData200)) {
                result200 = [] as any;
                for (let item of resultData200)
                    result200!.push(InOutReportOutput.fromJS(item));
            }

            return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<InOutReportOutput[]>(<any>null);
    }



}

function throwException(message: string, status: number, response: string, headers: { [key: string]: any; }, result?: any): Observable<any> {
    if (result !== null && result !== undefined)
        return _observableThrow(result);
    else
        return _observableThrow(new ApiException(message, status, response, headers, null));
}

function blobToText(blob: any): Observable<string> {
    return new Observable<string>((observer: any) => {
        if (!blob) {
            observer.next("");
            observer.complete();
        } else {
            let reader = new FileReader();
            reader.onload = event => {
                observer.next((<any>event.target).result);
                observer.complete();
            };
            reader.readAsText(blob);
        }
    });
}
