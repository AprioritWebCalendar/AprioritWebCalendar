import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { User } from '../../../authentication/models/user';
import { UserService } from '../../../services/user.service';
import { Observable } from 'rxjs/Observable';
import { TypeaheadMatch } from 'ngx-bootstrap/typeahead';
import { Observer } from 'rxjs/Observer';
import { CalendarService } from '../../services/calendar.service';
import { UserCalendar } from '../../models/user.calendar';

@Component({
    selector: 'app-select-user-share',
    templateUrl: './select-user-share.component.html'
})
export class SelectUserShareComponent {
    public emailOrUserName: string;
    public selectedUser: User = new User();
    public users: Observable<User[]>;

    @Input()
    id: Number;

    @Input()
    sharedUsers: UserCalendar[];

    isReadOnly: boolean = true;

    constructor(
        private userService: UserService,
        private calendarService: CalendarService
    ) {
        this.users = Observable.create((observer: any) => {
            userService.findUsersByEmailOrUserName(this.emailOrUserName)
                .subscribe(users => observer.next(users));
        });
     }

    shareCalendar() {
        let userCalendar = new UserCalendar();
        userCalendar.User = this.selectedUser;
        userCalendar.IsReadOnly = this.isReadOnly;
        
        console.log(userCalendar);

        this.calendarService.shareCalendar(this.id, this.selectedUser.Id, this.isReadOnly)
            .subscribe(isOk => {
                this.onCalendarShared.emit(userCalendar);
            },
            (e: Response) => {
                // TODO: Show notification.
            });
    }

    typeaheadOnSelect(e: TypeaheadMatch): void {
        this.selectedUser = e.item;
    }

    @Output()
    onCalendarShared = new EventEmitter<UserCalendar>();
}
