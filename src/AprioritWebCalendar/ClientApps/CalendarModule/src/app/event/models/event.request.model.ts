import { Location } from "./location";
import { mergeDateTime, getWithoutTime, getTimeAsString, getTimeAsDate, getLocalTime } from "../services/datetime.functions";
import { Event } from "./event";
import * as moment from 'moment';
import { PeriodRequestModel } from "./period.request.model";

export class EventRequestModel {
    public Id: number;

    public Name: string;
    public Description: string;

    public CalendarId: number;

    public StartDate?: string;
    public EndDate?: string;

    public StartTime?: string;
    public EndTime?: string;

    public IsAllDay: boolean;
    public IsPrivate: boolean;
    public RemindBefore?: number = 15;

    public Location: Location = new Location();
    public Period: PeriodRequestModel = new PeriodRequestModel();

    //
    public IsRecurrent: boolean;
    public IsLocationAttached: boolean = false;
    public IsRemindingEnabled: boolean = true;

    public ToEvent() : Event {
        var event = new Event();

        event.Id = this.Id
        event.Name = this.Name;
        event.Description = this.Description;

        event.CalendarId = this.CalendarId;

        event.StartDate = new Date(this.StartDate);
        event.EndDate = new Date(this.EndDate);

        event.StartTime = this.StartTime;
        event.EndTime = this.EndTime;

        event.IsAllDay = this.IsAllDay;
        event.IsPrivate = this.IsPrivate;
        event.RemindBefore = this.RemindBefore;

        if (this.Location != null)
            event.Location = this.Location;

        if (this.Period != null)
            event.Period = this.Period.ToPeriod();

        return event;
    }

    public ConvertDateTime(startEndDate: string[]) {
        if (!this.IsRecurrent) {
            this.Period = null;
        } else {
            this.Period.PeriodStart = moment(getWithoutTime(new Date(this.Period.PeriodStart))).format();
            this.Period.PeriodEnd = moment(getWithoutTime(new Date(this.Period.PeriodEnd))).format();
        }

        if (!this.IsLocationAttached)
            this.Location = null;

        if (!this.IsAllDay && !this.IsRecurrent) {
            let start = mergeDateTime(getWithoutTime(new Date(startEndDate[0])), this.StartTime);
            let end = mergeDateTime(getWithoutTime(new Date(startEndDate[1])), this.EndTime);

            this.StartDate = moment(getWithoutTime(start)).format();
            this.EndDate = moment(getWithoutTime(end)).format();

            this.StartTime = getTimeAsString(start);
            this.EndTime = getTimeAsString(end);
        } else if (this.IsAllDay && !this.IsRecurrent) {
            this.StartDate = moment(getWithoutTime(new Date(startEndDate[0]))).format();
            this.EndDate = moment(getWithoutTime(new Date(startEndDate[1]))).format();
        } else {
            this.StartTime = getTimeAsString(mergeDateTime(getWithoutTime(new Date()), this.StartTime));
            this.EndTime = getTimeAsString(mergeDateTime(getWithoutTime(new Date()), this.EndTime));
        }
    }

    public static FromEvent(event: Event) : EventRequestModel {
        let model = new EventRequestModel();

        model.Id = event.Id;
        model.Name = event.Name;
        model.Description = event.Description;
        model.CalendarId = event.CalendarId;

        model.StartDate = getLocalTime(mergeDateTime(event.StartDate, event.StartTime)).toDateString();
        model.EndDate = getLocalTime(mergeDateTime(event.EndDate, event.EndTime)).toDateString();

        model.StartTime = event.StartTime;
        model.EndTime = event.EndTime;

        model.IsAllDay = event.IsAllDay;
        model.IsPrivate = event.IsPrivate;

        if (event.RemindBefore != null) {
            model.RemindBefore = event.RemindBefore;
            model.IsRemindingEnabled = true;
        } else {
            model.IsRemindingEnabled = false;
        }

        if (event.Location != null) {
            model.Location = event.Location;
            model.IsLocationAttached = true;
        }
        
        if (event.Period != null) {
            model.Period = PeriodRequestModel.FromPeriod(event.Period);
            model.IsRecurrent = true;
        }

        return model;
    }
}