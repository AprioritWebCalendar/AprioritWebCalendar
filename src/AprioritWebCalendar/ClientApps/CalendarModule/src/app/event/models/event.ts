import { Period } from "./period";
import { User } from "../../authentication/models/user";
import { Location } from "./location";

export class Event  {
    public Id: number;

    public Name: string;
    public Description: string;

    public StartDate?: Date;
    public EndDate?: Date;

    public StartTime?: string;
    public EndTime?: string;

    public IsAllDay: boolean;
    public IsPrivate: boolean;
    public RemindBefore?: number;

    public IsReadOnly: boolean;
    public CalendarId: number;
    public Color: string;

    public Location: Location;
    public Period: Period;
    public Owner: User;
}