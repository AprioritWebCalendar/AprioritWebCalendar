import { Injectable } from "@angular/core";
import { CustomHttp } from "../../services/custom.http";
import { HubConnection } from "@aspnet/signalr";
import { Calendar } from "../models/calendar";

@Injectable()
export class CalendarListener {
    constructor(private _customHttp: CustomHttp) {
        this._connection = new HubConnection(`/hub/calendar?token=${this._customHttp.getTokenString()}`);
    }

    private _connection: HubConnection;

    public Start() : void {
        this._connection.start();
        console.log("The CalendarListener is running...");
    }

    public OnCalendarShared(callback: (calendar: Calendar) => void) : void {
        this._connection.on("calendarShared", args => callback(args.calendar));
    }

    public OnRemovedFromCalendar(callback: (id: number, name: string, owner: string) => void) : void {
        this._connection.on("removedFromCalendar", args => callback(args.Calendar.Id, args.Calendar.Name, args.Calendar.Owner));
    }

    public OnCalendarEdited(callback: (editedByUser: string, calendar: Calendar, oldCalendarName: string) => void) : void {
        this._connection.on("calendarEdited", args => {
            callback(args.editedByUser, args.calendar, args.oldCalendarName);
        });
    }
}