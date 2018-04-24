import { Component, OnInit } from '@angular/core';
import { MonthViewDay, CalendarEvent } from 'calendar-utils';
import { Event } from '../../../event/models/event';
import { DatesModel } from './dates.model';
import * as moment from 'moment';
import { EventService } from '../../../event/services/event.service';
import { ToastsManager } from 'ng2-toastr';
import { mergeDateTime, getRule, setEndOfDay, getLocalTime, getWithoutTime } from '../../../event/services/datetime.functions';
import { CalendarDateFormatter, CalendarNativeDateFormatter, CalendarEventAction } from 'angular-calendar';
import { DialogService } from 'ng2-bootstrap-modal';
import { EventCreateComponent } from '../../../event/components/event-create/event-create.component';
import { Calendar } from '../../models/calendar';
import { Subject } from 'rxjs/Subject';
import { AuthenticationService } from '../../../authentication/services/authentication.service';
import { isSameMonth, isSameDay } from 'ngx-bootstrap/chronos/utils/date-getters';
import { EventEditComponent } from '../../../event/components/event-edit/event-edit.component';
import { EventRequestModel } from '../../../event/models/event.request.model';
import { User } from '../../../authentication/models/user';
import { EventDeleteComponent, IEventDeleteParams } from '../../../event/components/event-delete/event-delete.component';

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
    currentUser: User;

    activeDayIsOpen: boolean = true;

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
        private dialogService: DialogService,
        private authenticationService: AuthenticationService
    ) { }

    actions: CalendarEventAction[] = [
        {
            label: '<i class="fa fa-fw fa-pencil"></i>',
            onClick: ({ event }: { event: CalendarEvent }): void => {
                this.openEditEventModal(event);
            }
        },
        {
            label: '<i class="fa fa-fw fa-times"></i>',
            onClick: ({ event }: { event: CalendarEvent }): void => {
                this.openDeleteEventModal(event);
            }
        }
    ];
    
    public ngOnInit() : void {
        this.locale = navigator.language.split("-")[0];
        this.localeData =  moment.localeData(this.locale);
        this.weekStartsOn = this.localeData.firstDayOfWeek();
    }

    private setCalendars(calendars: Calendar[]) : void {
        if (this.currentUser == null) {
            this.currentUser = this.authenticationService.getCurrentUser();
        }

        this.calendars = calendars;

        if (calendars == null || calendars.length === 0){
            this.dataEvents = [];
            this.calendarEvents = [];
            return;
        }

        this.loadEvents();
    }

    private changeViewMode(viewMode: string) : void {
        this.viewMode = viewMode;
        this.changeWeekPeriod();
        this.loadEvents();
    }

    private dayClicked({ date, events }: { date: Date; events: CalendarEvent[] }) : void {
        if (isSameMonth(date, this.viewDate)) {
            if (
                (isSameDay(this.viewDate, date) && this.activeDayIsOpen === true) ||
                events.length === 0
            ) {
                this.activeDayIsOpen = false;
            } else {
                this.activeDayIsOpen = true;
                this.viewDate = date;
            }
        }
    }

    private viewDateChanged(date: Date) : void {
        console.log(`View date has been changed ${date}`);

        this.changeWeekPeriod();
        this.loadEvents();
    }

    private openCreateEventModal() : void {
        var calendars = this.calendars.filter(c => c.IsReadOnly != true);

        this.dialogService.addDialog(EventCreateComponent, { calendars: calendars })
            .subscribe(event => {
                if (event == null)
                    return;

                event.IsReadOnly = false;
                event.Owner = this.authenticationService.getCurrentUser();
                event.Color = this.getCalendarsColor(event.CalendarId);
                console.log(event);

                this.dataEvents.push(event);
                this.mapEvent(event);
                
                this.toasts.success("The event has been created successfully");
            }, e => {
                this.toasts.error("Unable to create event. Try again or reload the page.");
            });
    }

    private openEditEventModal(event: CalendarEvent) : void {
        console.log(event.meta);

        let params = { 
            model: EventRequestModel.FromEvent(<Event>event.meta) 
        };

        this.dialogService.addDialog(EventEditComponent, params)
            .subscribe(ev => {
                if (ev == null)
                    return;

                ev.Owner = event.meta.Owner;
                ev.CalendarId = event.meta.CalendarId;
                ev.Color = event.meta.Color;
                ev.IsReadOnly = event.meta.IsReadOnly;

                this.calendarEvents = this.calendarEvents.filter(e => e.meta.Id != ev.Id);
                this.dataEvents = this.dataEvents.filter(e => e.Id != ev.Id);

                this.refresh.next();

                this.dataEvents.push(ev);
                this.mapEvent(ev);

                this.toasts.success("The event has been updated successfully");
            });
    }

    private openDeleteEventModal(event: CalendarEvent) {
        let params: IEventDeleteParams = {
            event: <Event>event.meta,
            currentUser: this.currentUser
        };

        this.dialogService.addDialog(EventDeleteComponent, params)
            .subscribe(id => {
                if (id == null)
                    return;

                this.dataEvents = this.dataEvents.filter(e => e.Id != id);
                this.calendarEvents = this.calendarEvents.filter(e => e.meta.Id != id);
                this.refresh.next();

                this.toasts.success("The event has been deleted successfully?");
            }, e => {
                this.toasts.error("Unable to delete the event. Try again or reload the page.");
            });
    }

    private changeWeekPeriod() : void {
        if (this.viewMode == "week") {
            var start = moment(this.viewDate).locale(this.locale).startOf("week").toDate().toLocaleDateString();
            var end = moment(this.viewDate).locale(this.locale).endOf("week").toDate().toLocaleDateString();
            
            this.weekPeriod = `${start} - ${end}`;
            console.log("Week period: " + this.weekPeriod);
        }
    }

    private loadEvents() : void {
        this.dataEvents = [];
        this.calendarEvents = [];
        var dates = this.getDates();
        
        this.eventService.getEvents(dates.StartDate, dates.EndDate, this.calendars.map(c => c.Id as number))
            .subscribe(events => {
                if (events != null) {
                    console.log(events);

                    this.dataEvents = events;

                    this.mapEventList(this.dataEvents);
                    this.refresh.next();
                }

                console.log(this.calendarEvents);
            }, e => {
                this.toasts.error("Unable to load events. Try again or reload the page!");
            });
    }

    private mapEventList(events: Event[]) : void {
        events.forEach(e => {
            this.mapEvent(e);
        });
    }

    private mapEvent(event: Event) : void {
        if (event.Period == null) {
            this.mapDefaultEvent(event);
        } else {
            this.mapRecurringEvent(event);
        }

        this.refresh.next();
    }

    private mapDefaultEvent(event: Event) : void {
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

    private mapRecurringEvent(event: Event) : void {
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

    private setEventActions(eventCal: CalendarEvent<Event>, event: Event) : void {
        if (!event.IsReadOnly) {
            eventCal.actions.push(this.actions[0]);
        }

        if (event.Owner.Id === this.currentUser.Id) {
            eventCal.actions.push(this.actions[1]);
        }
    }

    private getDates() : DatesModel {
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

    private getCalendarsColor(id: number) : string {
        return this.calendars.filter(c => c.Id == id)
            .map(c => c.Color)[0];
    }
}
