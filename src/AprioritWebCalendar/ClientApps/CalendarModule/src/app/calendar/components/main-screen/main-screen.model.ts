import { Calendar } from "../../models/calendar";
import { CalendarEvent, CalendarEventAction } from "angular-calendar";
import * as moment from 'moment';
import { Event } from "../../../event/models/event";
import { Subject } from "rxjs/Subject";
import { getWithoutTime, setEndOfDay, getLocalTime, mergeDateTime, getRule } from "../../../event/services/datetime.functions";
import { User } from "../../../authentication/models/user";
import { DatesModel } from "./dates.model";

export class MainScreenModel {
    public calendars: Calendar[] = [];
    public dataEvents: Event[] = [];
    public calendarEvents: CalendarEvent<Event>[] = [];
    public actions: CalendarEventAction[];

    public viewDate: Date = new Date();
    public viewMode: string = "month";

    public activeDayIsOpen: boolean = true;

    public localeData: moment.Locale;
    public locale: string;
    public weekStartsOn: number;
    public weekPeriod: string;

    public currentUser: User;

    public refresh: Subject<any> = new Subject();

    public mapEventList(events: Event[]) : void {
        events.forEach(e => {
            this.mapEvent(e);
        });
    }

    public mapEvent(event: Event) : void {
        if (event.Period == null) {
            this.mapDefaultEvent(event);
        } else {
            this.mapRecurringEvent(event);
        }

        this.refresh.next();
    }

    public mapDefaultEvent(event: Event) : void {
        let eventCal;

        eventCal = {
            title: event.Name,
            color: { primary: event.Color },
            meta: event,
            actions: []
        };

        eventCal.color.secondary = this.viewMode == "month" ? '#FDF1BA' : event.Color;

        if (event.IsAllDay) {
            eventCal.start = getWithoutTime(new Date(event.StartDate));
            eventCal.end = setEndOfDay(new Date(event.EndDate));
        } else {
            eventCal.start = getLocalTime(mergeDateTime(event.StartDate, event.StartTime));
            eventCal.end = getLocalTime(mergeDateTime(event.EndDate, event.EndTime));
        }

        this.setEventActions(eventCal, event);
        this.calendarEvents.push(eventCal);
    }

    public mapRecurringEvent(event: Event) : void {
        var rule = getRule(event.Period);

        rule.all().forEach(date => {
            let eventCal;

            eventCal = {
                title: event.Name,
                color: { primary: event.Color },
                meta: event,
                actions: []
            };

            eventCal.color.secondary = this.viewMode == "month" ? '#FDF1BA' : event.Color;

            if (event.IsAllDay) {
                eventCal.start = date;
                eventCal.end = setEndOfDay(date);
            } else {
                eventCal.start = getLocalTime(mergeDateTime(date, event.StartTime));
                eventCal.end = getLocalTime(mergeDateTime(date, event.EndTime));
            }

            this.setEventActions(eventCal, event);
            this.calendarEvents.push(eventCal);
        });
    }

    public setEventActions(eventCal: CalendarEvent<Event>, event: Event) : void {
        if (!event.IsReadOnly) {
            eventCal.actions.push(this.actions[0]);
        }

        if (event.Owner.Id === this.currentUser.Id) {
            eventCal.actions.push(this.actions[1]);
        }

        eventCal.actions.push(this.actions[2]);
    }

    public refreshCalendar() : void {
        this.refresh.next();
    }

    public getOwnCalendars() : Calendar[] {
        return this.calendars.filter(c => c.Owner.Id == this.currentUser.Id);
    }

    public getEditableCalendars() : Calendar[] {
        return this.calendars.filter(c => !c.IsReadOnly);
    }

    public getCalendarColor(id: number) : string {
        return this.calendars.filter(c => c.Id == id)[0].Color;
    }

    public getDefaultCalendar() : Calendar {
        return this.calendars.filter(c => c.IsDefault)[0];
    }

    public clearAllEvents() : void {
        this.dataEvents = [];
        this.calendarEvents = [];
    }

    public pushEvent(event: Event) : void {
        this.dataEvents.push(event);
        this.mapEvent(event);
    }

    public removeEvent(id: number) : void {
        this.dataEvents = this.dataEvents.filter(e => e.Id != id);
        this.calendarEvents = this.calendarEvents.filter(e => e.meta.Id != id);
    }

    public getDates() : DatesModel {
        var dates = new DatesModel();
        var curMoment = moment(this.viewDate).locale(this.locale);

        if (this.viewMode == "day") {
            dates.StartDate = curMoment.startOf("day").format("YYYY-MM-DD").toString();
            dates.EndDate = curMoment.endOf("day").format("YYYY-MM-DD").toString();
        } else if (this.viewMode == "week") {
            dates.StartDate = curMoment.startOf("week").format("YYYY-MM-DD").toString();
            dates.EndDate = curMoment.endOf("week").format("YYYY-MM-DD").toString();
        }
        else {
            dates.StartDate = curMoment.startOf("month").format("YYYY-MM-DD").toString();
            dates.EndDate = curMoment.endOf("month").format("YYYY-MM-DD").toString();
        }

        return dates;
    }
}