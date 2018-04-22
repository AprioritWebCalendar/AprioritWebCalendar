import { Component, OnInit } from '@angular/core';
import { MonthViewDay, CalendarEvent } from 'calendar-utils';
import { Event } from '../../../event/models/event';
import { DatesModel } from './dates.model';
import * as moment from 'moment';
import { EventService } from '../../../event/services/event.service';
import { ToastsManager } from 'ng2-toastr';
import { mergeDateTime, getRule, setEndOfDay, getLocalTime } from '../../../event/services/datetime.functions';
import { CalendarDateFormatter, CalendarNativeDateFormatter } from 'angular-calendar';

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
    calendars: number[] = [];

    public viewDate: Date = new Date();
    public viewMode: string = "month";

    public localeData: moment.Locale;
    public locale: string;
    public weekStartsOn: number;
    public weekPeriod: string;

    constructor(
        private eventService: EventService,
        private toasts: ToastsManager
    ) { }

    ngOnInit() {
        this.locale = navigator.language.split("-")[0];
        this.localeData =  moment.localeData(this.locale);
        this.weekStartsOn = this.localeData.firstDayOfWeek();
        
    }

    setCalendars(ids: number[]) {
        this.calendars = ids;
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
        alert("There will be a modal window to create an event.");
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
        
        this.eventService.getEvents(dates.StartDate, dates.EndDate, this.calendars)
            .subscribe(events => {
                if (events != null) {
                    console.log(events);

                    this.dataEvents = events;
                    this.mapEvents();

                    console.log(this.calendarEvents);
                }
            }, e => {
                this.toasts.error("Unable to load events. Try again or reload the page!");
            });
    }

    mapEvents() {
        this.calendarEvents = this.dataEvents.filter(e => e.Period == null).map(e => {
            let eventCal;

            eventCal = {
                title: e.Name,
                color: { primary: e.Color, secondary: '#FDF1BA' },
                meta: e
            };

            eventCal.color.secondary = this.viewMode == "month" ? '#FDF1BA' : e.Color;

            if (e.IsAllDay) {
                eventCal.start = new Date(e.StartDate);
                eventCal.end = setEndOfDay(new Date(e.EndDate));
            } else {
                eventCal.start = getLocalTime(mergeDateTime(e.StartDate, e.StartTime));
                eventCal.end = getLocalTime(mergeDateTime(e.EndDate, e.EndTime));
            }

            return eventCal;
        });

        this.mapRecurringEvents();
    }

    mapRecurringEvents() {
        this.dataEvents.filter(e => e.Period != null)
            .forEach(e => {
                var rule = getRule(e.Period);

                rule.all().forEach(d => {
                    let eventCal;

                    eventCal = {
                        title: e.Name,
                        color: { primary: e.Color, secondary: '#FDF1BA' },
                        meta: e
                    };

                    if (e.IsAllDay) {
                        eventCal.start = d;
                        eventCal.end = setEndOfDay(d);
                    } else {
                        eventCal.start = getLocalTime(mergeDateTime(d, e.StartTime));
                        eventCal.end = getLocalTime(mergeDateTime(d, e.EndTime));
                    }
                    
                    this.calendarEvents.push(eventCal);
                });
            });
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
}
