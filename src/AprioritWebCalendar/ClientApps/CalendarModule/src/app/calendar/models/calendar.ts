import { User } from "../../authentication/models/user";
import { EventCalendar } from "./event.calendar";

export class Calendar {
    public Id: Number;
    public Name: string;
    public Description: string;
    public Color: string;
    public Owner: User;

    public IsReadOnly?: boolean;
    public IsSubscribed?: boolean;
    public IsDefault: boolean;

    public Events: EventCalendar[];
}