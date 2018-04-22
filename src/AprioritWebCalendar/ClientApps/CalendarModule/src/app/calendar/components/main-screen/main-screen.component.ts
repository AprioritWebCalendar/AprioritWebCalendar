import { Component, OnInit } from '@angular/core';
import { MonthViewDay, CalendarEvent } from 'calendar-utils';
import { Event } from '../../../event/models/event';
import { DatesModel } from './dates.model';
import * as moment from 'moment';
import { EventService } from '../../../event/services/event.service';
import { ToastsManager } from 'ng2-toastr';
import { mergeDateTime, getRule, setEndOfDay, getLocalTime } from '../../../event/services/datetime.functions';

@Component({
    selector: 'app-main-screen',
    templateUrl: './main-screen.component.html'
})
export class MainScreenComponent {
    dataEvents: Event[] = [];
    calendarEvents: CalendarEvent<Event>[] = [];
    calendars: number[] = [];

    public viewDate: Date = new Date();
    public viewMode: string = "month";

    constructor(
        private eventService: EventService,
        private toasts: ToastsManager
    ) { }

    setCalendars(ids: number[]) {
        this.calendars = ids;
        this.loadEvents();
    }

    changeViewMode(viewMode: string) {
        this.viewMode = viewMode;
        this.loadEvents();
    }

    dayClicked(day: MonthViewDay) {
        console.log(day);
    }

    viewDateChanged(date: Date) {
        console.log(`View date has been changed ${date}`);
        this.loadEvents();
    }

    openCreateEventModal() {
        alert("There will be a modal window to create an event.");
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
            let eventCal: CalendarEvent<Event>;

            eventCal = {
                title: e.Name,
                color: { primary: e.Color, secondary: '#FDF1BA' },
                start: mergeDateTime(e.StartDate, e.StartTime),
                end: mergeDateTime(e.EndDate, e.EndTime),
                meta: e
            };

            if (!e.IsAllDay) {
                eventCal.start = getLocalTime(eventCal.start);
                eventCal.end = getLocalTime(eventCal.end);
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

        if (this.viewMode == "day") {
            dates.StartDate = moment(this.viewDate).startOf("day").format("YYYY-MM-DD").toString();
            dates.EndDate = moment(this.viewDate).endOf("day").format("YYYY-MM-DD").toString();
        } else if (this.viewMode == "week") {
            dates.StartDate = moment(this.viewDate).startOf("week").format("YYYY-MM-DD").toString();
            dates.EndDate = moment(this.viewDate).endOf("week").format("YYYY-MM-DD").toString();
        }
        else {
            dates.StartDate = moment(this.viewDate).startOf("month").format("YYYY-MM-DD").toString();
            dates.EndDate = moment(this.viewDate).endOf("month").format("YYYY-MM-DD").toString();
        }

        console.log(dates);
        return dates;
    }
}
