import { Pipe, PipeTransform } from '@angular/core';
/*
 * Raise the value exponentially
 * Takes an exponent argument that defaults to 1.
 * Usage:
 *   value | exponentialStrength:exponent
 * Example:
 *   {{ 2 | exponentialStrength:10 }}
 *   formats to: 1024
*/
@Pipe({name: 'minutesToTime'})
export class MinutesToTimePipe implements PipeTransform {
  transform(value: number): string {

      let hours   = Math.floor(value / 60);
      let minuts   = value % 60;
      let type = hours > 12 ? 'PM' : 'AM';
      let deductedHours  = hours > 12 ? (hours - 12) : hours;
        return deductedHours + ':' + minuts + ' ' + type;
  }
}
