import { Pipe, PipeTransform } from "@angular/core";
import { PeriodType } from "../event/models/period.type";

@Pipe({
    name: 'periodType'
})
export class PeriodTypePipe implements PipeTransform {
    transform(value: any, ...args: any[]) {
        let type: PeriodType = value as PeriodType;

        if (type == PeriodType.Custom)
            return "Daily";
        else if (type == PeriodType.Weekly)
            return "Weekly";
        else if (type == PeriodType.Monthly)
            return "Monthly";
        else if (type == PeriodType.Yearly)
            return "Yearly";
        else
            return undefined;
    }
}