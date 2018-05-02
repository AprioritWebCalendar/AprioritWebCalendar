import { Injectable } from "@angular/core";
import { CustomHttp } from "../../services/custom.http";

@Injectable()
export class CalendarIcalService {
    private _baseUrl: string = "/api/iCal/";

    constructor(private customHttp: CustomHttp){ }

    public exportCalendar(id: number) {
        return this.customHttp.get(this._baseUrl + id);
    }
}