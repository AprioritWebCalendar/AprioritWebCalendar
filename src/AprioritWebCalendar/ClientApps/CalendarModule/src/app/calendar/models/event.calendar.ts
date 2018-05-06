import { Calendar } from "./calendar";
import { Event } from "../../event/models/event";

export class EventCalendar {
    public Calendar: Calendar;
    public Event: Event;
    public IsReadOnly: boolean;
}