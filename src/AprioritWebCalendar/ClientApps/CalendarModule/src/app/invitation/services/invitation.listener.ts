import { Injectable } from "@angular/core";
import { CustomHttp } from "../../services/custom.http";
import { HubConnection } from "@aspnet/signalr";
import { Event } from "../../event/models/event";
import { Invitation } from "../models/invitation";

@Injectable()
export class InvitationListener {
    constructor(private _customHttp: CustomHttp) {
        this._connection = new HubConnection(`/hub/invitation?token=${this._customHttp.getTokenString()}`);
    }

    private _connection: HubConnection;

    public Start() : void {
        this._connection.start();
        console.log("The InvitationListener is running...");
    }

    public OnIncomingInvitationsReceived(callback: (invitations: Invitation[]) => void) : void {
        this._connection.on("incomingInvitations", args => callback(args));
    }

    public OnUserInvited(callback: (invitation: Invitation) => void) : void {
        this._connection.on("invited", args => {
            console.log(args);
            callback(args.invitation);
        });
    }

    public OnInvitationDeleted(callback: (eventName: string, eventId: number, invitatorName: string) => void) : void {
        this._connection.on("invitationDeleted", args => {
            callback(args.eventName, args.eventId, args.invitatorName);
        });
    }
}