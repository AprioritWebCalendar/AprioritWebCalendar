import { DatePipe } from "@angular/common";
import { PipeTransform, Pipe } from "@angular/core";
import * as moment from 'moment';

// Doesn't work well.

@Pipe({
    name: 'dateFormat'
})
export class DateFormatPipe extends DatePipe implements PipeTransform {
    transform(value: any, args?: any): any {
        return super.transform(value, moment.localeData(navigator.language).longDateFormat("LL"));
    }
}