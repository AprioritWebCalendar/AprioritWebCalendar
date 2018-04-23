import { Component, OnInit } from '@angular/core';
import { MonthViewDay, CalendarEvent } from 'calendar-utils';
import { Event } from '../../../event/models/event';
import { DatesModel } from './dates.model';
import * as moment from 'moment';
import { EventService } from '../../../event/services/event.service';
import { ToastsManager } from 'ng2-toastr';
import { mergeDateTime, getRule, setEndOfDay, getLocalTime, getWithoutTime } from '../../../event/services/datetime.functions';
import { CalendarDateFormatter, CalendarNativeDateFormatter } from 'angular-calendar';
import { DialogService } from 'ng2-bootstrap-modal';
import { EventCreateComponent } from '../../../event/components/event-create/event-create.component';
import { Calendar } from '../../models/calendar';
import { Subject } from 'rxjs/Subject';

@Component({
    selector: 'app-main-screen',
    templateUrl: './main-screen.component.html',
    providers: [{
        provide: CalendarDateFormatter,
        useClass: CalendarNativeDateFormatter
    }]
})
export class MainScreenComponent implements OnInit {
    dataEvents: Event[] = [];
    calendarEvents: CalendarEvent<Event>[] = [];
    calendars: Calendar[] = [];

    refresh: Subject<any> = new Subject();

    public viewDate: Date = new Date();
    public viewMode: string = "month";

    public localeData: moment.Locale;
    public locale: string;
    public weekStartsOn: number;
    public weekPeriod: string;

    constructor(
        private eventService: EventService,
        private toasts: ToastsManager,
        private dialogService: DialogService
    ) { }

    ngOnInit() {
        this.locale = navigator.language.split("-")[0];
        this.localeData =  moment.localeData(this.locale);
        this.weekStartsOn = this.localeData.firstDayOfWeek();
    }

    setCalendars(calendars: Calendar[]) {
        this.calendars = calendars;

        if (calendars == null || calendars.length === 0){
            this.dataEvents = [];
            this.calendarEvents = [];
            return;
        }

        this.loadEvents();
    }

    changeViewMode(viewMode: string) {
        this.viewMode = viewMode;
        this.changeWeekPeriod();
        this.loadEvents();
    }

    dayClicked(day: MonthViewDay) {
        console.log(day);
    }

    viewDateChanged(date: Date) {
        console.log(`View date has been changed ${date}`);

        this.changeWeekPeriod();
        this.loadEvents();
    }

    openCreateEventModal() {
        var calendars = this.calendars.filter(c => c.IsReadOnly != true);

        this.dialogService.addDialog(EventCreateComponent, { calendars: calendars })
            .subscribe(event => {
                if (event == null)
                    return;

                event.Color = this.getCalendarsColor(event.CalendarId);
                console.log(event);

                this.dataEvents.push(event);

                if (event.Period == null) {
                    this.calendarEvents.push(this.fromDefaultEvent(event));
                } else {
                    this.fromRecurringEvent(event).forEach(e => this.calendarEvents.push(e));
                }

                this.refresh.next();
                this.toasts.success("The event has been created successfully");
            }, e => {
                this.toasts.error("Unable to create event. Try again or reload the page.");
            });
    }

    changeWeekPeriod() {
        if (this.viewMode == "week") {
            var start = moment(this.viewDate).locale(this.locale).startOf("week").toDate().toLocaleDateString();
            var end = moment(this.viewDate).locale(this.locale).endOf("week").toDate().toLocaleDateString();
            
            this.weekPeriod = `${start} - ${end}`;
            console.log("Week period: " + this.weekPeriod);
        }
    }

    loadEvents() {
        var dates = this.getDates();
        
        this.eventService.getEvents(dates.StartDate, dates.EndDate, this.calendars.map(c => c.Id as number))
            .subscribe(events => {
                if (events != null) {
                    console.log(events);

                    this.dataEvents = events;

                    this.mapEvents();

                    console.log(this.calendarEvents);
                } else {
                    this.calendarEvents = [];
                }
            }, e => {
                this.toasts.error("Unable to load events. Try again or reload the page!");
            });
    }

    mapEvents() {
        let events: CalendarEvent<Event>[] = [];

        this.dataEvents.forEach(e => {
            if (e.Period == null) {
                events.push(this.fromDefaultEvent(e));
            } else {
                let list = this.fromRecurringEvent(e);

                list.forEach(rEvent => events.push(rEvent));
            }
        });

        this.calendarEvents = events;
    }

    fromDefaultEvent(event: Event): CalendarEvent<Event> {
        let eventCal;

        eventCal = {
            title: event.Name,
            color: { primary: event.Color },
            meta: event
        };

        eventCal.color.secondary = this.viewMode == "month" ? '#FDF1BA' : event.Color;

        if (event.IsAllDay) {
            eventCal.start = getWithoutTime(new Date(event.StartDate));
            eventCal.end = setEndOfDay(new Date(event.EndDate));
        } else {
            eventCal.start = getLocalTime(mergeDateTime(event.StartDate, event.StartTime));
            eventCal.end = getLocalTime(mergeDateTime(event.EndDate, event.EndTime));
        }

        return eventCal;
    }

    fromRecurringEvent(event: Event) : CalendarEvent<Event>[] {
        let list: CalendarEvent<Event>[] = [];
        var rule = getRule(event.Period);

        rule.all().forEach(date => {
            let eventCal;

            eventCal = {
                title: event.Name,
                color: { primary: event.Color },
                meta: event
            };

            eventCal.color.secondary = this.viewMode == "month" ? '#FDF1BA' : event.Color;

            if (event.IsAllDay) {
                eventCal.start = date;
                eventCal.end = setEndOfDay(date);
            } else {
                eventCal.start = getLocalTime(mergeDateTime(date, event.StartTime));
                eventCal.end = getLocalTime(mergeDateTime(date, event.EndTime));
            }

            list.push(eventCal);
        });

        return list;
    }

    getDates() : DatesModel {
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

        console.log(dates);
        return dates;
    }

    getCalendarsColor(id: number) {
        return this.calendars.filter(c => c.Id == id)
            .map(c => c.Color)[0];
    }
}
