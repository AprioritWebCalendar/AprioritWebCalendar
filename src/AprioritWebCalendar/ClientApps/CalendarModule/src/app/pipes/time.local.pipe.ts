import { Pipe, PipeTransform } from "@angular/core";
import { getTimeAsDate, getTimeAsString } from "../event/services/datetime.functions";

@Pipe({
    name: 'timeLocal'
})
export class TimeLocalPipe implements PipeTransform {
    transform(value: any, ...args: any[]) {
        return getTimeAsString(getTimeAsDate(value));
    }
}