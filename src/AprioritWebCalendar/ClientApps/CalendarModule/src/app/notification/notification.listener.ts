import { CustomHttp } from "../services/custom.http";
import { Injectable } from "@angular/core";
import { HubConnection, IHubConnectionOptions } from '@aspnet/signalr';

@Injectable()
export class NotificationListener {
    constructor(
        private customHttp: CustomHttp
    ) {

    }

    private _connection: HubConnection;

    public Start() : void {
        this._connection = new HubConnection(`/notification-hub?token=${this.customHttp.GetTokenString()}`);
        this._connection.start();
        console.log("The NotificationListener is running...");
    }

    public OnCalendarShared(callback: (calendarName: string, calendarOwner: string) => void) : void {
        this._connection.on("calendarShared", args => {
            callback(args.calendarName, args.calendarOwner);
        });
    }

    public OnCalendarEdited(callback: (editedByUser: string, calendarName: string, newCalendarName: string) => void) : void {
        this._connection.on("calendarEdited", args => {
            callback(args.editedByUser, args.calendarName, args.newCalendarName);
        });
    }

    public OnEventInCalendarCreated(callback: (createdByUser: string, eventName: string, calendarName: string) => void) : void {
        this._connection.on("eventInCalendarCreated", args => {
            callback(args.createdByUser, args.eventName, args.calendarName);
        });
    }

    public OnEventEdited(callback: (editedByUser: string, eventName: string, newEventName: string) => void) : void {
        this._connection.on("eventEdited", args => {
            callback(args.editedByUser, args.eventName, args.newEventName);
        });
    }

    public OnUserInvited(callback: (eventName: string, invitatorName: string) => void) : void {
        this._connection.on("invited", args => {
            callback(args.eventName, args.invitatorName);
        });
    }

    public OnInvitationAccepted(callback: (eventName: string, userName: string) => void) : void {
        this._connection.on("invitationAccepted", args => {
            callback(args.eventName, args.userName);
        });
    }

    public OnInvitationRejected(callback: (eventName: string, userName: string) => void) : void {
        this._connection.on("invitationRejected", args => {
            callback(args.eventName, args.userName);
        });
    }

    public OnInvitationDeleted(callback: (eventName: string, invitatorName: string) => void) : void {
        this._connection.on("invitationDeleted", args => {
            callback(args.eventName, args.invitatorName);
        });
    }

    public OnRemovedFromCalendar(callback: (calendarName: string, calendarOwner: string) => void) : void {
        this._connection.on("removedFromCalendar", args => {
            callback(args.calendarName, args.calendarOwner);
        });
    }
 
    public OnRemovedFromEvent(callback: (eventName: string, eventOwner: string) => void) : void {
        this._connection.on("removedFromEvent", args => {
            callback(args.eventName, args.eventOwner);
        });
    }

    public OnCalendarReadOnlyChanged(callback: (calendarName: string, calendarOwner: string, isReadOnly: boolean) => void) : void {
        this._connection.on("calendarReadOnlyChanged", args => {
            callback(args.calendarName, args.calendarOwner, args.isReadOnly);
        });
    }

    public OnEventReadOnlyChanged(callback: (eventName: string, eventOwner: string, isReadOnly: boolean) => void) : void {
        this._connection.on("eventReadOnlyChanged", args => {
            callback(args.eventName, args.eventOwner, args.isReadOnly);
        });
    }
}