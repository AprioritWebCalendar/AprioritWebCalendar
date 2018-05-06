import { Injectable } from "@angular/core";
import { Response, RequestOptions, Headers } from "@angular/http";
import { CustomHttp } from "../../services/custom.http";
import { Observable } from "rxjs/Observable";
import { Calendar } from "../models/calendar";
import { UserCalendar } from "../models/user.calendar";

@Injectable()
export class CalendarService {
    private baseUrl: string = "/api/Calendar/";

    constructor(private customHttp: CustomHttp) { }

    public getCalendars(onlyOwn: boolean = false) : Observable<Calendar[]> {
        return this.customHttp.get(this.baseUrl, { params: { onlyOwn: onlyOwn } })
            .map((response: Response) => response.json())
            .catch (e => Observable.throw(e));
    }

    public getById(id: Number) : Observable<Calendar> {
        return this.customHttp.get(`${this.baseUrl}${id}`)
            .map((response: Response) => response.json())
            .catch (e => Observable.throw(e));
    }

    public getSharedUsers(id: Number) : Observable<UserCalendar[]> {
        return this.customHttp.get(`${this.baseUrl}${id}/SharedUsers`)
            .map((response: Response) => response.json())
            .catch (e => Observable.throw(e));
    }

    public createCalendar(calendar: Calendar) : Observable<Number> {
        return this.customHttp.post(this.baseUrl, calendar)
            .map((response: Response) => response.json().Id)
            .catch (e => Observable.throw(e));
    }

    public updateCalendar(id: Number, calendar: Calendar) : Observable<boolean> {
        return this.customHttp.put(`${this.baseUrl}${id}`, calendar)
            .map(response => true)
            .catch (e => Observable.throw(e));
    }

    public deleteCalendar(id: Number) : Observable<boolean> {
        return this.customHttp.delete(`${this.baseUrl}${id}`)
            .map(response => true)
            .catch (e => Observable.throw(e));
    }

    public shareCalendar(id: Number, userId: Number, isReadOnly: boolean = true) : Observable<boolean> {
        var body = {
            UserId: userId,
            IsReadOnly: isReadOnly
        };

        return this.customHttp.put(`${this.baseUrl}${id}/Share`, body)
            .map(response => true)
            .catch (e => Observable.throw(e));
    }

    public removeSharingCalendar(id: Number, userId: Number) : Observable<boolean> {
        var opts = new RequestOptions();
        opts.headers = new Headers();
        opts.headers.set("Content-Type", "application/json");
        this.customHttp.AttachToken(opts);

        return this.customHttp.put(`${this.baseUrl}${id}/RemoveSharing`, userId, opts)
            .map(response => true)
            .catch (e => Observable.throw(e));
    }

    public setReadOnly(id: Number, userId: Number, isReadOnly: boolean) : Observable<boolean> {
        var opts = new RequestOptions();
        opts.headers = new Headers();
        opts.headers.set("Content-Type", "application/json");
        this.customHttp.AttachToken(opts);

        return this.customHttp.put(`${this.baseUrl}${id}/ReadOnly/${userId}`, isReadOnly, opts)
            .map(response => true)
            .catch (e => Observable.throw(e));
    }

    public subscribeCalendar(id: Number) : Observable<boolean> {
        return this.customHttp.put(`${this.baseUrl}${id}/Subscribe`, {})
            .map(response => true)
            .catch (e => Observable.throw(e));
    }

    public unsubscribeCalendar(id: Number) : Observable<boolean> {
        return this.customHttp.put(`${this.baseUrl}${id}/Unsubscribe`, {})
            .map(response => true)
            .catch (e => Observable.throw(e));
    }
}