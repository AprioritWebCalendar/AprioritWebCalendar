import { Period } from "./period";
import { Location } from "./location";

export class EventRequestModel {
    public Id: number;

    public Name: string;
    public Description: string;

    public CalendarId: number;

    public StartDate: Date;
    public EndDate: Date;

    public StartTime: Date;
    public EndTime: Date;

    public IsAllDay: boolean;
    public IsPrivate: boolean;
    public RemindBefore?: number = 15;

    public Location: Location;
    public Period: Period;

    //
    public IsRecurrent: boolean;
    public IsLocationAttached: boolean;
    public IsRemindingEnabled: boolean = true;
}