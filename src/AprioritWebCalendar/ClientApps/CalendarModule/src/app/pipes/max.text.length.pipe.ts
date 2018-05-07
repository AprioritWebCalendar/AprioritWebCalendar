import { Pipe, PipeTransform } from "@angular/core";

@Pipe({
    name: 'maxTextLength'
})
export class MaxTextLengthPipe implements PipeTransform {
    transform(value: any, ...args: any[]) {
        let maxLength = args[0] as number;

        if (maxLength == undefined || isNaN(maxLength) || !isFinite(maxLength)) {
            throw new Error("MaxTextLengthPipe: maxLength is set wrong.");
        }

        let str = value as string;

        if (str == undefined) {
            throw new Error("The value is undefined");
        }

        if (str.length <= maxLength) {
            return str;
        }

        return str.substr(0, maxLength) + "...";
    }
}