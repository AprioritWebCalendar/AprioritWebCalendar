import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { CalendarService } from '../../services/calendar.service';
import { LeftCalendarMenuModel } from './left-calendar-menu.model';
import { Calendar } from '../../models/calendar';
import { AuthenticationService } from '../../../authentication/services/authentication.service';
import { CalendarCheck } from './calendar.check.model';
import { DialogService } from 'ng2-bootstrap-modal';
import { CalendarCreateComponent } from '../calendar-create/calendar-create.component';
import { CalendarEditComponent, ICalendarEditModel } from '../calendar-edit/calendar-edit.component';
import { CalendarDeleteComponent } from '../calendar-delete/calendar-delete.component';
import { ShareCalendarComponent } from '../share-calendar/share-calendar.component';

@Component({
    selector: 'app-left-calendar-menu',
    templateUrl: './left-calendar-menu.component.html',
    styleUrls: ['./left.menu.css']
})
export class LeftCalendarMenuComponent implements OnInit {
    public model: LeftCalendarMenuModel = new LeftCalendarMenuModel();

    constructor(
        private calendarService: CalendarService,
        private authService: AuthenticationService,
        private dialogService: DialogService
    ) { }

    public UserId: Number;

    ngOnInit() {
        this.calendarService.getCalendars()
            .subscribe((calendars: Calendar[]) => {
                this.model.Calendars = <CalendarCheck[]>calendars;
                this.model.Calendars.forEach(function(value) {
                    value.IsChecked = true;
                });

                this.UserId = this.authService.getCurrentUser().Id;
            },
            (response: Response) => {
                this.model.IsError = true;
            });
    }

    showCreateModal() {
        this.dialogService.addDialog(CalendarCreateComponent)
            .subscribe((calendar: CalendarCheck) => {
                if (calendar != null) {
                    calendar.Owner = this.authService.getCurrentUser();
                    this.model.Calendars.push(calendar);
                }
            });
    }

    showEditModal(calendar: CalendarCheck) {
        var model = {
            Id: calendar.Id,
            Name: calendar.Name,
            Description: calendar.Description,
            Color: calendar.Color
        };

        this.dialogService.addDialog(CalendarEditComponent, model)
            .subscribe((editModel: ICalendarEditModel) => {
                calendar.Name = editModel.Name;
                calendar.Description = editModel.Description;
                calendar.Color = editModel.Color;
            });
    }

    showDeleteModal(calendar: CalendarCheck) {
        var model = {
            Id: calendar.Id,
            Name: calendar.Name
        };

        this.dialogService.addDialog(CalendarDeleteComponent, model)
            .subscribe((isOk: boolean) => {
                if (!isOk)
                    return;

                this.model.Calendars.splice(this.model.Calendars.indexOf(calendar), 1);
            });
    }

    showShareModal(calendar: CalendarCheck) {
        var model = { 
            Id: calendar.Id,
            Name: calendar.Name,
            Owner: calendar.Owner
        };

        this.dialogService.addDialog(ShareCalendarComponent, model);
    }

    subscribeCalendar(calendar: CalendarCheck) {
        if (!confirm(`Subscribe calendar "${calendar.Name}"?`))
            return;

        this.calendarService.subscribeCalendar(calendar.Id)
            .subscribe((isOk: boolean) => {
                calendar.IsSubscribed = true;
            },
            (resp: Response) => {
                // TODO: Notifications.
            });
    }

    unsubscribeCalendar(calendar: CalendarCheck) {
        if (!confirm(`Unsubscribe calendar "${calendar.Name}"?`))
            return;

        this.calendarService.unsubscribeCalendar(calendar.Id)
            .subscribe((isOk: boolean) => {
                calendar.IsSubscribed = false;
            },
            (resp: Response) => {
                // TODO: Notifications.
            });
    }

    @Output()
    onCalendarsChanged = new EventEmitter<Number[]>();

    calendarsChanged() {
        this.onCalendarsChanged.emit(this.model.getCheckedCalendarsId());
    }
}
