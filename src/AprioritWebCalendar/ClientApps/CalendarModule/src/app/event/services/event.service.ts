import { Injectable } from "@angular/core";
import { Observable } from "rxjs/Observable";
import { RequestOptions, Headers } from "@angular/http";
import { Event } from "../models/event";
import { CustomHttp } from "../../services/custom.http";
import { UserInvited } from "../models/user.invited";
import { EventRequestModel } from "../models/event.request.model";

@Injectable()
export class EventService {
    private baseUrl: string = "/api/Event/";

    constructor(private customHttp: CustomHttp) {

    }

    public getEvents(startDate: string, endDate: string, calendars: number[]) : Observable<Event[]> {
        var params = {
            StartDate: startDate,
            EndDate: endDate,
            CalendarsIds: calendars
        };
        
        return this.customHttp.get(this.baseUrl, { params: params })
            .map(r => r.json())
            .catch(e => Observable.throw(e));
    }

    public getById(id: number) : Observable<Event> {
        return this.customHttp.get(this.baseUrl + id)
            .map(r => r.json())
            .catch(e => Observable.throw(e));
    }

    public getUsers(id: number) : Observable<UserInvited[]> {
        return this.customHttp.get(`${this.baseUrl}${id}/Users`)
            .map(r => r.json())
            .catch(e => Observable.throw(e));
    }

    public createEvent(event: EventRequestModel) : Observable<number> {
        return this.customHttp.post(this.baseUrl, event)
            .map(r => r.json().Id)
            .catch(e => Observable.throw(e));
    }

    public updateEvent(id: number, event: EventRequestModel) : Observable<boolean> {
        return this.customHttp.put(this.baseUrl + id, event)
            .map(r => true)
            .catch(e => Observable.throw(e));
    }

    public moveEvent(id: number, oldCalendar: number, calendar: number) : Observable<boolean> {
        var data = {
            OldCalendar: oldCalendar,
            NewCalendar: calendar
        };

        return this.customHttp.put(`${this.baseUrl}${id}/Move`, data)
            .map(r => true)
            .catch(e => Observable.throw(e));
    }

    public inviteUser(id: number, userId: number, isReadOnly: boolean) : Observable<boolean> {
        var data = {
            UserId: userId,
            IsReadOnly: isReadOnly
        };

        return this.customHttp.put(`${this.baseUrl}${id}/InviteUser`, data)
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

    public setEventReadOnlyState(id: number, userId: number, isReadOnly: boolean) : Observable<boolean> {
        var opts = new RequestOptions();
        opts.headers = new Headers();
        opts.headers.set("Content-Type", "application/json");
        this.customHttp.attachToken(opts);

        return this.customHttp.put(`${this.baseUrl}${id}/ReadOnly/${userId}`, isReadOnly)
            .map(r => true)
            .catch(e => Observable.throw(e));
    }

    public setInvitationReadOnlyState(id: number, userId: number, isReadOnly: boolean) : Observable<boolean> {
        var opts = new RequestOptions();
        opts.headers = new Headers();
        opts.headers.set("Content-Type", "application/json");
        this.customHttp.attachToken(opts);

        return this.customHttp.put(`${this.baseUrl}${id}/Invitation/ReadOnly/${userId}`, isReadOnly)
            .map(r => true)
            .catch(e => Observable.throw(e));
    }

    public deleteEvent(id: number) : Observable<boolean> {
        return this.customHttp.delete(`${this.baseUrl}${id}`)
            .map(r => true)
            .catch(e => Observable.throw(e));
    }

    public deleteInvitedUser(id: number, userId: number) : Observable<boolean> {
        return this.customHttp.delete(`${this.baseUrl}${id}/Invited/${userId}`)
            .map(r => true)
            .catch(e => Observable.throw(e));
    }

    public deleteInvitation(id: number, userId: number) : Observable<boolean> {
        return this.customHttp.delete(`${this.baseUrl}${id}/Invitation/${userId}`)
            .map(r => true)
            .catch(e => Observable.throw(e));
    }
}
