import { Component, OnInit, Input } from '@angular/core';
import { UserCalendar } from '../../models/user.calendar';
import { CalendarService } from '../../services/calendar.service';
import { UserService } from '../../../services/user.service';

@Component({
    selector: 'app-shared-users-list',
    templateUrl: './shared-users-list.component.html'
})
export class SharedUsersListComponent {
    constructor(
        private calendarService: CalendarService,
        private userService: UserService
    ) {}

    @Input()
    id: Number;

    @Input()
    sharedUsers: UserCalendar[];

    readOnlyStateChanged(userCalendar: UserCalendar) {
        this.calendarService.setReadOnly(this.id, userCalendar.User.Id, userCalendar.IsReadOnly)
            .subscribe((isOk: boolean) => {
                if (!isOk)
                    return;
            },
            (e: Response) => {
                // TODO: Notification.
            });
    }

    removeSharing(userCalendar: UserCalendar) {
        if (!confirm(`Do you really want to remove "${userCalendar.User.UserName}" from shared users?`))
            return;

        if (!confirm("Are you sure?"))
            return;

        this.calendarService.removeSharingCalendar(this.id, userCalendar.User.Id)
            .subscribe((isOk: boolean) => {
                if (!isOk)
                    return;

                this.sharedUsers.splice(this.sharedUsers.indexOf(userCalendar), 1);

                // TODO: Notification.
            },
            (e: Response) => {
                // TODO: Notification.
            });
    }
}
