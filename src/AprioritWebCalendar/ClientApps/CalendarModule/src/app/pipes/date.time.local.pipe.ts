import { Pipe, PipeTransform } from "@angular/core";
import { mergeDateTime, getLocalTime } from "../event/services/datetime.functions";

@Pipe({
    name: 'dateTimeLocal'
})
export class DateTimeLocalPipe implements PipeTransform {
    transform(value: any, ...args: any[]) {
        let time = args[0];
        return getLocalTime(mergeDateTime(value, time)).toLocaleString();
    }
}