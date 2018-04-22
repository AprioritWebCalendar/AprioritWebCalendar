import { CalendarCheck } from "./calendar.check.model";

export class LeftCalendarMenuModel {
    public Calendars: CalendarCheck[] = [];
    public ShowOnlyOwn: boolean = false;
    public IsError: boolean = false;

    public getCheckedCalendarsId() : Number[] {
        return this.Calendars
            .filter(c => c.IsChecked)
            .map(c => c.Id);
    }
}