import { Injectable } from "@angular/core";
import { CustomHttp } from "../../services/custom.http";
import { Observable } from "rxjs/Observable";
import { RequestOptions, Headers } from "@angular/http";

@Injectable()
export class SettingsService {
    private _baseUrl: string = "/api/Settings/";

    constructor(private _customHttp: CustomHttp) {

    }

    public getTimeZone() : Observable<string> {
        return this._customHttp.get(this._baseUrl + "TimeZone")
            .map(r => r.json().TimeZone)
            .catch(e => Observable.throw(e));
    }

    public saveTimeZone(timeZone: string) : Observable<boolean> {
        var opts = new RequestOptions();
        opts.headers = new Headers();
        opts.headers.set("Content-Type", "application/json");
        this._customHttp.AttachToken(opts);

        return this._customHttp.post(this._baseUrl + "TimeZone", timeZone, opts)
            .map(r => true)
            .catch(e => Observable.throw(e));
    }
}