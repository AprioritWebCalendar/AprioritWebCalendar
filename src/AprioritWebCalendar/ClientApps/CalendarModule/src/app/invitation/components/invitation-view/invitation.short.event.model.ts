import { Event } from "../../../event/models/event";
import { getWithoutTime, getLocalTime, mergeDateTime, getTimeAsDate } from "../../../event/services/datetime.functions";
import * as moment from 'moment';

export class InvitationShortEventModel {
    public Name: string;
    public Description: string;

    public Start: string;
    public End: string;

    public IsRecurring: boolean;
    public IsAllDay: boolean;

    public PeriodStart: string;
    public PeriodEnd: string;

    public static FromEvent(event: Event) {
        let model = new InvitationShortEventModel();

        model.Name = event.Name;
        model.Description = event.Description;
        model.IsAllDay = event.IsAllDay;

        if (event.Period == null) {
            if (event.IsAllDay) {
                model.Start = getWithoutTime(new Date(event.StartDate)).toLocaleDateString();
                model.End = getWithoutTime(new Date(event.EndDate)).toLocaleDateString();
            } else {
                model.Start = getLocalTime(mergeDateTime(event.StartDate, event.StartTime)).toLocaleString();
                model.End = getLocalTime(mergeDateTime(event.EndDate, event.EndTime)).toLocaleString();
            }
        } else {
            model.PeriodStart = getWithoutTime(new Date(event.Period.PeriodStart)).toLocaleDateString();
            model.PeriodEnd = getWithoutTime(new Date(event.Period.PeriodEnd)).toLocaleDateString();

            model.IsRecurring = true;

            if (event.IsAllDay) {
                model.Start = model.PeriodStart;
                model.End = model.PeriodEnd;
            } else {
                model.Start = moment(getTimeAsDate(event.StartTime)).format("HH:mm").toString();
                model.End = moment(getTimeAsDate(event.EndTime)).format("HH:mm").toString();
            }
        }

        return model;
    }
}