import { Component, ViewChild, Injector, Output, EventEmitter, OnInit } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { ShiftsServiceProxy, CreateOrEditShiftDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { NotifyService } from '@abp/notify/notify.service';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { Paginator } from 'primeng/components/paginator/paginator';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import * as moment from 'moment';
import { Router, ActivatedRoute, ParamMap ,Params } from '@angular/router';
import { NgForm } from '@angular/forms';

@Component({
    selector: 'manageShift',
    templateUrl: './manage-shift.component.html',
    styleUrls:['./shift-style.css']
})
export class ManageShiftComponent extends AppComponentBase implements OnInit {

    active = false;
    saving = false;
    value: Date;
    shift: CreateOrEditShiftDto = new CreateOrEditShiftDto();

    meridian = true;
    shiftId: number;

    TimeInObj: Date = new Date();
    TimeOutObj: Date = new Date();
    EarlyInObj: Date = new Date();
    LateInObj: Date = new Date();
    EarlyOutObj: Date = new Date();
    LateOutObj: Date = new Date();
    TimeInRangeFromObj: Date = new Date();
    TimeInRangeToObj: Date = new Date();
    TimeOutRangeFromObj: Date = new Date();
    TimeOutRangeToObj: Date = new Date();



    constructor(
        injector: Injector,
        private _shiftsServiceProxy: ShiftsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _fileDownloadService: FileDownloadService,
        private _route: ActivatedRoute,
        private _router: Router,
    ) {
        super(injector);
    }
    ngOnInit(): void {

        this._route.params.subscribe((params: Params) => {
            this.shiftId = params['id'];
              this.show();
         });

    }

    toggleMeridian() {
        this.meridian = !this.meridian;
    }

    show(): void {

        if (!this.shiftId) {
            this.shift = new CreateOrEditShiftDto();
            this.shift.id = this.shiftId;
            this.shift.deductType = 0 ;
            this.shift.dayRest = 1 ;
            this.shift.dayOff = 7 ;
            this.active = true;

        } else {
            this._shiftsServiceProxy.getShiftForEdit(this.shiftId).subscribe(result => {

                this.shift = result.shift;
                this.TimeInObj.setHours(Math.floor(this.shift.timeIn / 60));
                this.TimeInObj.setMinutes(this.shift.timeIn % 60);



                this.TimeOutObj.setHours( Math.floor(this.shift.timeOut / 60));
                this.TimeOutObj.setMinutes(this.shift.timeOut % 60);


                this.EarlyInObj.setHours(Math.floor(this.shift.earlyIn / 60));
                this.EarlyInObj.setMinutes( this.shift.earlyIn % 60);

                this.LateInObj.setHours(Math.floor(this.shift.lateIn / 60));
                this.LateInObj.setMinutes(this.shift.lateIn % 60);

                this.EarlyOutObj.setHours( Math.floor(this.shift.earlyOut / 60));
                this.EarlyOutObj.setMinutes(this.shift.earlyOut % 60);


                this.LateOutObj.setHours(Math.floor(this.shift.lateOut / 60));
                this.LateOutObj.setMinutes(this.shift.lateOut % 60);


                this.TimeInRangeFromObj.setHours(Math.floor(this.shift.timeInRangeFrom / 60));
                this.TimeInRangeFromObj.setMinutes(this.shift.timeInRangeFrom % 60);

                this.TimeInRangeToObj.setHours(Math.floor(this.shift.timeInRangeTo / 60));
                this.TimeInRangeToObj.setMinutes(this.shift.timeInRangeTo % 60);

                this.TimeOutRangeFromObj.setHours(Math.floor(this.shift.timeOutRangeFrom / 60));
                this.TimeOutRangeFromObj.setMinutes(this.shift.timeOutRangeFrom % 60);

                this.TimeOutRangeToObj.setHours(Math.floor(this.shift.timeOutRangeTo / 60));
                this.TimeOutRangeToObj.setMinutes(this.shift.timeOutRangeTo % 60);


                this.active = true;

            });
        }
    }

    save(shiftForm : NgForm): void {
        if(shiftForm.form.valid){
            this.saving = true;
            this.shift.timeIn = (this.TimeInObj.getHours() * 60) + this.TimeInObj.getMinutes();
            this.shift.timeOut = (this.TimeOutObj.getHours() * 60) + this.TimeOutObj.getMinutes();
            this.shift.earlyIn = (this.EarlyInObj.getHours() * 60) + this.EarlyInObj.getMinutes();
            this.shift.lateIn = (this.LateInObj.getHours() * 60) + this.LateInObj.getMinutes();
            this.shift.earlyOut = (this.EarlyOutObj.getHours() * 60) + this.EarlyOutObj.getMinutes();
            this.shift.lateOut = (this.LateOutObj.getHours() * 60) + this.LateOutObj.getMinutes();

            this.shift.timeInRangeFrom = (this.TimeInRangeFromObj.getHours() * 60) + this.TimeInRangeFromObj.getMinutes();
            this.shift.timeInRangeTo = (this.TimeInRangeToObj.getHours() * 60) + this.TimeInRangeToObj.getMinutes();
            this.shift.timeOutRangeFrom = (this.TimeOutRangeFromObj.getHours() * 60) + this.TimeOutRangeFromObj.getMinutes();
            this.shift.timeOutRangeTo = (this.TimeOutRangeToObj.getHours() * 60) + this.TimeOutRangeToObj.getMinutes();


            this._shiftsServiceProxy.createOrEdit(this.shift)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this._router.navigate(['app/main/setting/shifts']);
             });
        }
           
    }







    close(): void {
        this.active = false;
        this._router.navigate(['app/main/setting/shifts']);

    }
}
