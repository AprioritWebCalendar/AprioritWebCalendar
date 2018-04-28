import { Injectable } from "@angular/core";
import { CustomHttp } from "../../services/custom.http";
import { Observable } from "rxjs/Observable";
import { RequestOptions, Headers } from "@angular/http";
import { UserInvited } from "../../event/models/user.invited";
import { Invitation } from "../models/invitation";

@Injectable()
export class InvitationService {
    private baseUrl: string = "/api/Event/";

    constructor(private customHttp: CustomHttp) {
    }

    public getIncomingInvitations() : Observable<Invitation[]> {
        return this.customHttp.get(`${this.baseUrl}Invitation/Incoming`)
            .map(r => r.json())
            .catch(e => Observable.throw(e));
    }

    public getOutcomingInvitations() : Observable<Invitation[]> {
        return this.customHttp.get(`${this.baseUrl}Invitation/Outcoming`)
            .map(r => r.json())
            .catch(e => Observable.throw(e));
    }

    public getUsers(eventId: number) : Observable<UserInvited[]> {
        return this.customHttp.get(`${this.baseUrl}${eventId}/Users`)
            .map(r => r.json())
            .catch(e => Observable.throw(e));
    }

    public inviteUser(eventId: number, userId: number, isReadOnly: boolean) : Observable<boolean> {
        var data = {
            UserId: userId,
            IsReadOnly: isReadOnly
        };

        return this.customHttp.put(`${this.baseUrl}${eventId}/Invite`, data)
            .map(r => true)
            .catch(e => Observable.throw(e));
    }

    public acceptInvitation(eventId: number) : Observable<boolean> {
        return this.customHttp.put(`${this.baseUrl}${eventId}/Accept`, {})
            .map(r => true)
            .catch(e => Observable.throw(e));
    }

    public rejectInvitation(eventId: number) : Observable<boolean> {
        return this.customHttp.put(`${this.baseUrl}${eventId}/Reject`, {})
            .map(r => true)
            .catch(e => Observable.throw(e));
    }

    public setInvitationReadOnlyState(eventId: number, userId: number, isReadOnly: boolean) : Observable<boolean> {
        var opts = new RequestOptions();
        opts.headers = new Headers();
        opts.headers.set("Content-Type", "application/json");
        this.customHttp.attachToken(opts);

        return this.customHttp.put(`${this.baseUrl}${eventId}/Invitation/ReadOnly/${userId}`, isReadOnly, opts)
            .map(r => true)
            .catch(e => Observable.throw(e));
    }

    public deleteInvitation(eventId: number, userId: number) : Observable<boolean> {
        return this.customHttp.delete(`${this.baseUrl}${eventId}/Invitation/${userId}`)
            .map(r => true)
            .catch(e => Observable.throw(e));
    }
}