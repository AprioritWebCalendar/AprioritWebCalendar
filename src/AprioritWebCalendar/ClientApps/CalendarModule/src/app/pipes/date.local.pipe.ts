import { Pipe, PipeTransform } from "@angular/core";

@Pipe({
    name: 'dateLocal'
})
export class DateLocalPipe implements PipeTransform {
    transform(value: any, ...args: any[]) {
        return new Date(value).toLocaleDateString();
    }
}