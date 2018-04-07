import { Component, OnInit, Input } from '@angular/core';
import { UserCalendar } from '../../models/user.calendar';
import { CalendarService } from '../../services/calendar.service';

@Component({
    selector: 'app-shared-users-list',
    templateUrl: './shared-users-list.component.html'
})
export class SharedUsersListComponent {
    constructor(private calendarService: CalendarService) {}

    @Input()
    sharedUsers: UserCalendar[];

    readOnlyStateChanged(userCalendar: UserCalendar) {
        console.log(userCalendar);
    }
}
