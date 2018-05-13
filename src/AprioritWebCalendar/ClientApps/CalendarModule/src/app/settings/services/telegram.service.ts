import { Injectable } from "@angular/core";
import { CustomHttp } from "../../services/custom.http";
import { Observable } from "rxjs/Observable";
import { RequestOptions, Headers } from "@angular/http";

@Injectable()
export class TelegramService {
    private _baseUrl: string = "/api/Settings/Telegram/";

    constructor(private _customHttp: CustomHttp) {
    }

    public verifyTelegramCode(code: string) : Observable<number> {
        var opts = new RequestOptions();
        opts.headers = new Headers();
        opts.headers.set("Content-Type", "application/json");
        this._customHttp.AttachToken(opts);

        return this._customHttp.post(this._baseUrl, code, opts)
            .map(r => r.json().TelegramId)
            .catch(e => Observable.throw(e));
    }

    public setNotificationsEnabled(isEnabled: boolean) : Observable<boolean> {
        var opts = new RequestOptions();
        opts.headers = new Headers();
        opts.headers.set("Content-Type", "application/json");
        this._customHttp.AttachToken(opts);

        return this._customHttp.post(this._baseUrl + "Notifications", isEnabled, opts)
            .map(r => true)
            .catch(e => Observable.throw(e));
    }

    public resetTelegram() : Observable<boolean> {
        return this._customHttp.post(this._baseUrl + "Reset", {})
            .map(r => true)
            .catch(e => Observable.throw(e));
    }
}