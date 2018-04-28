import { Injectable } from "@angular/core";
import { CustomHttp } from "../../services/custom.http";
import { Observable } from "rxjs/Observable";
import { RequestOptions, Headers } from "@angular/http";
import { UserInvited } from "../../event/models/user.invited";

@Injectable()
export class InvitationService {
    private baseUrl: string = "/api/Event/";

    constructor(private customHttp: CustomHttp) {
    }

    public getUsers(id: number) : Observable<UserInvited[]> {
        return this.customHttp.get(`${this.baseUrl}${id}/Users`)
            .map(r => r.json())
            .catch(e => Observable.throw(e));
    }

    public inviteUser(id: number, userId: number, isReadOnly: boolean) : Observable<boolean> {
        var data = {
            UserId: userId,
            IsReadOnly: isReadOnly
        };

        return this.customHttp.put(`${this.baseUrl}${id}/Invite`, data)
            .map(r => true)
            .catch(e => Observable.throw(e));
    }

    public acceptInvitation(id: number) : Observable<boolean> {
        return this.customHttp.put(`${this.baseUrl}${id}/Accept`, {})
            .map(r => true)
            .catch(e => Observable.throw(e));
    }

    public rejectInvitation(id: number) : Observable<boolean> {
        return this.customHttp.put(`${this.baseUrl}${id}/Reject`, {})
            .map(r => true)
            .catch(e => Observable.throw(e));
    }

    public setInvitationReadOnlyState(id: number, userId: number, isReadOnly: boolean) : Observable<boolean> {
        var opts = new RequestOptions();
        opts.headers = new Headers();
        opts.headers.set("Content-Type", "application/json");
        this.customHttp.attachToken(opts);

        return this.customHttp.put(`${this.baseUrl}${id}/Invitation/ReadOnly/${userId}`, isReadOnly, opts)
            .map(r => true)
            .catch(e => Observable.throw(e));
    }

    public deleteInvitation(id: number, userId: number) : Observable<boolean> {
        return this.customHttp.delete(`${this.baseUrl}${id}/Invitation/${userId}`)
            .map(r => true)
            .catch(e => Observable.throw(e));
    }
}