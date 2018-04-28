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
import { EventMoveComponent, IEventMoveParams } from '../../../event/components/event-move/event-move.component';
import { IEventShareParams, EventShareComponent } from '../../../event/components/event-share/event-share.component';
import { Invitation } from '../../../invitation/models/invitation';
import { MainScreenModel } from './main-screen.model';

@Component({
    selector: 'app-main-screen',
    templateUrl: './main-screen.component.html',
    providers: [{
        provide: CalendarDateFormatter,
        useClass: CalendarNativeDateFormatter
    }]
})
export class MainScreenComponent implements OnInit {
    public model: MainScreenModel = new MainScreenModel();

    public isSidebarOpened: boolean;

    constructor(
        private eventService: EventService,
        private toasts: ToastsManager,
        private dialogService: DialogService,
        private authenticationService: AuthenticationService
    ) {
        this.model.actions = this.actions;
     }

    private actions: CalendarEventAction[] = [
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
        },
        {
            label: '<i class="fas fa-arrow-right"></i>',
            onClick: ({ event }: { event: CalendarEvent }): void => {
                this.openMoveEventModal(event);
            }
        }
    ];
    
    public ngOnInit() : void {
        this.model.locale = navigator.language.split("-")[0];
        this.model.localeData =  moment.localeData(this.model.locale);
        this.model.weekStartsOn = this.model.localeData.firstDayOfWeek();
    }

    private setCalendars(calendars: Calendar[]) : void {
        if (this.model.currentUser == null) {
            this.model.currentUser = this.authenticationService.getCurrentUser();
        }

        this.model.calendars = calendars;

        if (calendars == null || calendars.length === 0){
            this.model.clearAllEvents();
            return;
        }

        this.loadEvents();
    }

    private changeViewMode(viewMode: string) : void {
        this.model.viewMode = viewMode;
        this.changeWeekPeriod();
        this.loadEvents();
    }

    private dayClicked({ date, events }: { date: Date; events: CalendarEvent[] }) : void {
        if (isSameMonth(date, this.model.viewDate)) {
            if (
                (isSameDay(this.model.viewDate, date) && this.model.activeDayIsOpen === true) ||
                events.length === 0
            ) {
                this.model.activeDayIsOpen = false;
            } else {
                this.model.activeDayIsOpen = true;
                this.model.viewDate = date;
            }
        }
    }

    private openEventShareModal(event: CalendarEvent) {
        let params: IEventShareParams = {
            event: event.meta
        };

        this.dialogService.addDialog(EventShareComponent, params);
    }

    private viewDateChanged(date: Date) : void {
        console.log(`View date has been changed ${date}`);

        this.changeWeekPeriod();
        this.loadEvents();
    }

    private openCreateEventModal() : void {
        var calendars = this.model.getEditableCalendars();

        this.dialogService.addDialog(EventCreateComponent, { calendars: calendars })
            .subscribe(event => {
                if (event == null)
                    return;

                event.IsReadOnly = false;
                event.Owner = this.authenticationService.getCurrentUser();
                event.Color = this.model.getCalendarColor(event.CalendarId);
                console.log(event);

                this.model.pushEvent(event);
                
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

                this.model.removeEvent(ev.Id);
                this.model.pushEvent(ev);

                this.toasts.success("The event has been updated successfully");
            });
    }

    private openDeleteEventModal(event: CalendarEvent) : void {
        let params: IEventDeleteParams = {
            event: <Event>event.meta,
            currentUser: this.model.currentUser
        };

        this.dialogService.addDialog(EventDeleteComponent, params)
            .subscribe(id => {
                if (id == null)
                    return;

                this.model.removeEvent(id);
                this.model.refreshCalendar();

                this.toasts.success("The event has been deleted successfully?");
            }, e => {
                this.toasts.error("Unable to delete the event. Try again or reload the page.");
            });
    }

    private openMoveEventModal(event: CalendarEvent) : void {
        let params: IEventMoveParams = {
            event: event.meta,
            calendars: this.model.getOwnCalendars()
        };

        this.dialogService.addDialog(EventMoveComponent, params)
            .subscribe(ev => {
                this.model.removeEvent(ev.Id);
                this.model.pushEvent(ev);

                this.toasts.success("The event has been moved successfully");
            }, e => {
                this.toasts.error("Unable to move the event. Try again or reload the page.");
            });
    }

    private changeSidebarOpened() {
        this.isSidebarOpened = !this.isSidebarOpened;
    }

    private closeOpenedSidebar(e: MouseEvent) {
        if (e.toElement.id != "incoming-invitations-button")
            this.isSidebarOpened = false;
    }

    private changeWeekPeriod() : void {
        if (this.model.viewMode == "week") {
            var start = moment(this.model.viewDate).locale(this.model.locale).startOf("week").toDate().toLocaleDateString();
            var end = moment(this.model.viewDate).locale(this.model.locale).endOf("week").toDate().toLocaleDateString();
            
            this.model.weekPeriod = `${start} - ${end}`;
            console.log("Week period: " + this.model.weekPeriod);
        }
    }

    private loadEvents() : void {
        this.model.dataEvents = [];
        this.model.calendarEvents = [];
        var dates = this.model.getDates();
        
        this.eventService.getEvents(dates.StartDate, dates.EndDate, this.model.calendars.map(c => c.Id as number))
            .subscribe(events => {
                if (events != null) {
                    this.model.dataEvents = events;
                    this.model.mapEventList(this.model.dataEvents);
                }
            }, e => {
                this.toasts.error("Unable to load events. Try again or reload the page!");
            });
    }
}
