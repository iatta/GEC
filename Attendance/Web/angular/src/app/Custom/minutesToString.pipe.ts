import { Pipe, PipeTransform } from '@angular/core';

@Pipe({name: 'minutesToString'})
export class MinutesToStringPipe implements PipeTransform {
  transform(value: number): string {

      let hours   = Math.floor(value / 60);
      let minuts   = value % 60;
      return hours + ' H ' + minuts + ' M' ;
  }
}
