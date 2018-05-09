import { Pipe, PipeTransform } from "@angular/core";
import { getWithoutTime } from "../event/services/datetime.functions";

@Pipe({
    name: 'dateLocal'
})
export class DateLocalPipe implements PipeTransform {
    transform(value: any, ...args: any[]) {
        return getWithoutTime(value).toLocaleDateString();
    }
}