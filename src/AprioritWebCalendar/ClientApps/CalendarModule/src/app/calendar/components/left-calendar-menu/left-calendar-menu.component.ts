import { Component, OnInit, Output, EventEmitter, ViewContainerRef } from '@angular/core';
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
import { ToastsManager } from 'ng2-toastr';

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
        private dialogService: DialogService,
        private toastr: ToastsManager
    ) {
     }

    public UserId: Number;

    ngOnInit() {
        this.calendarService.getCalendars()
            .subscribe((calendars: Calendar[]) => {
                this.UserId = this.authService.getCurrentUser().Id;

                if (calendars == null)
                    return;

                this.model.Calendars = <CalendarCheck[]>calendars;
                this.model.Calendars.forEach(function(value) {
                    value.IsChecked = true;
                });

                this.calendarsChanged();
            },
            (response: Response) => {
                this.model.IsError = true;
                this.toastr.error("It seems we have some problems. Try to reload the page.");
            });
    }

    showCreateModal() {
        this.dialogService.addDialog(CalendarCreateComponent)
            .subscribe((calendar: CalendarCheck) => {
                if (calendar != null) {
                    calendar.Owner = this.authService.getCurrentUser();
                    this.model.Calendars.push(calendar);

                    console.log(calendar);
                    console.log(`UserId: ${this.UserId}`);

                    this.toastr.success("The calendar has been created successfully.");
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

                this.toastr.success("The calendar has been updated successfully.");
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
                this.toastr.success("The calendar has been deleted successfully.");
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
                this.toastr.success("You have subscribed to the calendar.");
            },
            (resp: Response) => {
                this.toastr.error("Unable to subscribe to the calendar. Try to reload the page.");
            });
    }

    unsubscribeCalendar(calendar: CalendarCheck) {
        if (!confirm(`Unsubscribe calendar "${calendar.Name}"?`))
            return;

        this.calendarService.unsubscribeCalendar(calendar.Id)
            .subscribe((isOk: boolean) => {
                calendar.IsSubscribed = false;
                this.toastr.success("You have unsubscribed from the calendar.");
            },
            (resp: Response) => {
                this.toastr.error("Unable to unsubscribe from the calendar. Try to reload the page.");
            });
    }

    @Output()
    onCalendarsChanged = new EventEmitter<Number[]>();

    calendarsChanged() {
        this.onCalendarsChanged.emit(this.model.getCheckedCalendarsId());
    }
}
