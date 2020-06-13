import { Attribute, Directive, forwardRef } from '@angular/core';
import { AbstractControl, NG_VALIDATORS, Validator } from '@angular/forms';

//Got from: https://scotch.io/tutorials/how-to-implement-a-custom-validator-directive-confirm-password-in-angular-2

@Directive({
    selector: '[validateTime][formControlName],[validateTime][formControl],[validateTime][ngModel]',
    providers: [
        { provide: NG_VALIDATORS, useExisting: forwardRef(() => FromToTimeValidator), multi: true }
    ]
})
export class FromToTimeValidator implements Validator {
    constructor(
        @Attribute('validateTime') public validateTime: string,
        @Attribute('reverse') public reverse: string
        
    ) {
    }

    private get isReverse() {
        if (!this.reverse) {
            return false;
        }

        return this.reverse === 'true';
    }

    validate(control: AbstractControl): { [key: string]: any } {
        debugger
        const pairControl = control.root.get(this.validateTime);
        if (!pairControl) {
            return null;
        }

        if (!control.value) {
            return null;
        }

        const value:Date = control.value;
        const pairValue: Date = pairControl.value;

        const pairControlMinutes =  (pairValue.getHours() * 60) + pairValue.getMinutes();
        const valueMinutes = (value.getHours() * 60) + value.getMinutes();
        if (this.isReverse) {
            if(valueMinutes < pairControlMinutes ||valueMinutes == pairControlMinutes){
                pairControl.setErrors({
                    validateTime: true
                });
               }else{
                this.deleteErrors(pairControl);
               }
        }else{
            if(valueMinutes > pairControlMinutes || valueMinutes == pairControlMinutes){
                pairControl.setErrors({
                    validateTime: true
                });
               }else{
                this.deleteErrors(pairControl);
               }
        }
      

        // if (this.isReverse) {
        //     if (value === pairValue) {
        //         this.deleteErrors(pairControl);
        //     } else {
        //         pairControl.setErrors({
        //             validateEqual: true
        //         });
        //     }

        //     return null;
        // } else {
        //     if (value !== pairValue) {
        //         return {
        //             validateEqual: true
        //         };
        //     }
        // }
    }

    deleteErrors(control: AbstractControl) {
        if (control.errors) {
            delete control.errors['validateTime'];
        }

        if (control.errors && !Object.keys(control.errors).length) {
            control.setErrors(null);
        }
    }
}
