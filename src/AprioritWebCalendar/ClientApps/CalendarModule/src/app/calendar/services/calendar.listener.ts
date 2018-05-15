import { Injectable } from "@angular/core";
import { CustomHttp } from "../../services/custom.http";
import { HubConnection } from "@aspnet/signalr";
import { Calendar } from "../models/calendar";
import { HubListener } from "../../services/hub.listener";

@Injectable()
export class CalendarListener extends HubListener {
    constructor() {
        super("calendar");
    }

    public OnCalendarShared(callback: (calendar: Calendar) => void) : void {
        this._connection.off("calendarShared");
        this._connection.on("calendarShared", args => callback(args.calendar));
    }

    public OnRemovedFromCalendar(callback: (id: number, name: string, owner: string) => void) : void {
        this._connection.off("removedFromCalendar");
        this._connection.on("removedFromCalendar", args => callback(args.Calendar.Id, args.Calendar.Name, args.Calendar.Owner));
    }

    public OnCalendarEdited(callback: (editedByUser: string, calendar: Calendar, oldCalendarName: string) => void) : void {
        this._connection.off("calendarEdited");
        this._connection.on("calendarEdited", args => {
            callback(args.editedByUser, args.calendar, args.oldCalendarName);
        });
    }

    public OnCalendarReadOnlyChanged(callback: (id, name, owner, isReadOnly) => void) : void {
        this._connection.off("calendarReadOnlyChanged");
        this._connection.on("calendarReadOnlyChanged", args => {
            callback(args.Calendar.Id, args.Calendar.Name, args.Calendar.Owner, args.IsReadOnly);
        });
    }

    public OnCalendarDeleted(callback: (id: number, name: string, owner: string) => void) : void {
        this._connection.off("calendarDeleted");
        this._connection.on("calendarDeleted", args => callback(args.Calendar.Id, args.Calendar.Name, args.Calendar.Owner));
    }
}