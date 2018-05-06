import { CalendarCheck } from "./calendar.check.model";
import { Calendar } from "../../models/calendar";

export class LeftCalendarMenuModel {
    public Calendars: CalendarCheck[] = [];
    public ShowOnlyOwn: boolean = false;
    public IsError: boolean = false;

    public RemoveCalendar(id: number) : void {
        this.Calendars = this.Calendars.filter(c => c.Id != id);
    }

    public UpdateCalendar(calendar: Calendar) : void {
        let calToUpdate = this.Calendars.filter(c => c.Id == calendar.Id)[0];
        calToUpdate.Name = calendar.Name;
        calToUpdate.Description = calendar.Description;
        calToUpdate.Color = calendar.Color;
        
        let index = this.Calendars.indexOf(calToUpdate);
        this.Calendars.splice(index, 1, calToUpdate);
    }
}