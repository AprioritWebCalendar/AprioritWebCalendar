import { Injectable } from "@angular/core";
import { CustomHttp } from "../../services/custom.http";
import { HubConnection } from "@aspnet/signalr";
import { Event } from "../../event/models/event";
import { Invitation } from "../models/invitation";
import { HubListener } from "../../services/hub.listener";

@Injectable()
export class InvitationListener extends HubListener {
    constructor() {
        super("invitation");
    }

    public OnIncomingInvitationsReceived(callback: (invitations: Invitation[]) => void) : void {
        this._connection.off("incomingInvitations");
        this._connection.on("incomingInvitations", args => callback(args));
    }

    public OnUserInvited(callback: (invitation: Invitation) => void) : void {
        this._connection.off("invited");
        this._connection.on("invited", args => {
            console.log(args);
            callback(args.invitation);
        });
    }

    public OnInvitationDeleted(callback: (eventName: string, eventId: number, invitatorName: string) => void) : void {
        this._connection.off("invitationDeleted");
        this._connection.on("invitationDeleted", args => {
            callback(args.eventName, args.eventId, args.invitatorName);
        });
    }
}