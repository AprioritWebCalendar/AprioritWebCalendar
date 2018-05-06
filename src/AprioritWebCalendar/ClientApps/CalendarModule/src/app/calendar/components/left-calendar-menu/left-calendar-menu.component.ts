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
import { ICalendarExportParams, CalendarExportComponent } from '../calendar-export/calendar-export.component';
import { HotkeysService, Hotkey } from 'angular2-hotkeys';
import { CalendarImportComponent } from '../calendar-import/calendar-import.component';
import { PushNotificationService } from '../../../services/push.notification.service';
import { CalendarListener } from '../../services/calendar.listener';

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
        private toastr: ToastsManager,
        private hotkeysService: HotkeysService,
        private pushNotifService: PushNotificationService,
        private calendarListener: CalendarListener
    ) {
     }

    public UserId: Number;

    ngOnInit() {
        this.configureHotkeys();

        this.calendarService.getCalendars()
            .subscribe((calendars: Calendar[]) => {
                if (calendars == null)
                    return;

                this.UserId = this.authService.getCurrentUser().Id;
                this.model.Calendars = <CalendarCheck[]>calendars;
                this.model.Calendars.forEach(function(value) {
                    value.IsChecked = true;
                });

                this.calendarsChanged();
                this.configureSignalR();
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
                this.calendarsChanged();
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

    showExportModal(calendar: CalendarCheck) {
        let params: ICalendarExportParams = {
            id: calendar.Id as number,
            fileName: calendar.Name
        };

        this.dialogService.addDialog(CalendarExportComponent, params);
    }

    showImportModal() {
        this.dialogService.addDialog(CalendarImportComponent)
            .subscribe((calendar: CalendarCheck) => {
                if (calendar != null) {
                    calendar.Owner = this.authService.getCurrentUser();
                    this.model.Calendars.push(calendar);
                    this.calendarsChanged();
                    this.toastr.success("The calendar has been imported successfully.");
                }
            });
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
    onCalendarsChanged = new EventEmitter<Calendar[]>();

    @Output()
    onCalendarDeleted = new EventEmitter<number>();

    @Output()
    onCalendarUpdated = new EventEmitter<Calendar>();

    calendarsChanged() {
        this.onCalendarsChanged.emit(this.model.Calendars.filter(c => c.IsChecked).map(c => c as Calendar));
    }

    private configureHotkeys() : void {
        this.hotkeysService.add(new Hotkey("alt+c", (e: KeyboardEvent): boolean => {
            e.preventDefault();
            this.showCreateModal();

            return false;
        }));
    }

    private configureSignalR() : void {
        this.calendarListener.OnRemovedFromCalendar((id, name, owner) => {
            this.model.RemoveCalendar(id);
            this.onCalendarDeleted.emit(id);
            this.pushNotifService.PushNotification(`Has removed you from calendar "${name}".`, owner);
        });

        this.calendarListener.OnCalendarShared(calendar => {
            let calCheck = <CalendarCheck>calendar;
            this.model.Calendars.push(calCheck);

            this.pushNotifService.PushNotification(`Has shared calendar "${calendar.Name}" with you.`, 
                calendar.Owner.UserName.toString());
        });

        this.calendarListener.OnCalendarEdited((editor, calendar, oldName) => {
            let message;

            if (oldName == calendar.Name) {
                message = `Has edited your calendar "${calendar.Name}".`;
            } else {
                message = `Has renamed your calendar "${oldName}" to "${calendar.Name}".`;
            }

            this.model.UpdateCalendar(calendar);
            this.onCalendarUpdated.emit(calendar);
            this.pushNotifService.PushNotification(message, editor);
        });

        this.calendarListener.Start();
    }
}
