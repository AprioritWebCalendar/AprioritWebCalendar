import { Pipe, PipeTransform } from "@angular/core";
import { getTimeAsDate, getTimeAsString, getLocalTime } from "../event/services/datetime.functions";

@Pipe({
    name: 'timeLocal'
})
export class TimeLocalPipe implements PipeTransform {
    transform(value: any, ...args: any[]) {
        return getTimeAsString(getLocalTime(getTimeAsDate(value)));
    }
}