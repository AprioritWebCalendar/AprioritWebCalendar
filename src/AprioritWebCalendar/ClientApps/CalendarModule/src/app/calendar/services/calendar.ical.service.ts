import { Injectable } from "@angular/core";
import { CustomHttp } from "../../services/custom.http";
import { Observable } from "rxjs/Observable";
import { Response } from "@angular/http";
import { CalendarImportModel } from "../models/calendar-import.model";
import { CalendarImportPreviewModel } from "../models/calendar-import.preview.model";

@Injectable()
export class CalendarIcalService {
    private _baseUrl: string = "/api/iCal/";

    constructor(private customHttp: CustomHttp){ }

    public exportCalendar(id: number) : Observable<Response> {
        return this.customHttp.get(this._baseUrl + id);
    }

    public importCalendar(model: CalendarImportModel) : Observable<CalendarImportPreviewModel> {
        let data = new FormData();
        data.append("File", model.File, model.File.name);
        data.append("Name", model.Name);
        data.append("Color", model.Color);

        if (model.Description != undefined)
            data.append("Description", model.Description);

        return this.customHttp.post(this._baseUrl, data)
            .map(r => r.json())
            .catch(e => Observable.throw(e));
    }

    public saveCalendar(key: string) : Observable<number> {
        return this.customHttp.put(this._baseUrl + key, {})
            .map(r => r.json().Id)
            .catch(e => Observable.throw(e));
    }
}