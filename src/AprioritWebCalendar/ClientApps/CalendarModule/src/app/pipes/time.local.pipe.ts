import { Pipe, PipeTransform } from "@angular/core";
import { getTimeAsDate, getLocalTime, getTimeAsLocalString } from "../event/services/datetime.functions";

@Pipe({
    name: 'timeLocal'
})
export class TimeLocalPipe implements PipeTransform {
    transform(value: any, ...args: any[]) {
        return getTimeAsLocalString(getLocalTime(getTimeAsDate(value)));
    }
}