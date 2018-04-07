import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { CalendarService } from '../../services/calendar.service';
import { LeftCalendarMenuModel } from './left-calendar-menu.model';
import { Calendar } from '../../models/calendar';
import { AuthenticationService } from '../../../authentication/services/authentication.service';
import { CalendarCheck } from './calendar.check.model';
import { DialogService } from 'ng2-bootstrap-modal';
import { CalendarCreateComponent } from '../calendar-create/calendar-create.component';

@Component({
    selector: 'app-left-calendar-menu',
    templateUrl: './left-calendar-menu.component.html',
    styleUrls: ['./left.menu.css', './checkbox.css']
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

    @Output()
    onCalendarsChanged = new EventEmitter<Number[]>();

    calendarsChanged() {
        this.onCalendarsChanged.emit(this.model.getCheckedCalendarsId());
    }
}
