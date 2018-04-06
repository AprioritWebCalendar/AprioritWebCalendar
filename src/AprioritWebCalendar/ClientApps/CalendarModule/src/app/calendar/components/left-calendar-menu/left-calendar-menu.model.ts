import { CalendarCheck } from "./calendar.check.model";

export class LeftCalendarMenuModel {
    public Calendars: CalendarCheck[] = [];
    public ShowOnlyOwn: boolean = false;
    public IsError: boolean = false;

    public getCheckedCalendarsId() : Number[] {
        let ids: Number[] = [];

        this.Calendars.forEach(function(value){
            if (value.IsChecked){
                ids.push(value.Id);
            }
        });

        return ids;
    }
}