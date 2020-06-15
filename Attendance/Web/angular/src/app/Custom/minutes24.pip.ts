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
@Pipe({name: 'minutesToTime24'})
export class MinutesToTimePipe24 implements PipeTransform {
  transform(value: number): string {

      let hours   = Math.floor(value / 60);
      let minuts   = value % 60;
      return hours + ':' + minuts;
  }
}
