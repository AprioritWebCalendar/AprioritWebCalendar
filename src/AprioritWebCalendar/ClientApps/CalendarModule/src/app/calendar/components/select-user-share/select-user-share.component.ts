import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { User } from '../../../authentication/models/user';
import { UserService } from '../../../services/user.service';
import { Observable } from 'rxjs/Observable';
import { TypeaheadMatch } from 'ngx-bootstrap/typeahead';
import { Observer } from 'rxjs/Observer';
import { CalendarService } from '../../services/calendar.service';
import { UserCalendar } from '../../models/user.calendar';
import { ToastsManager } from 'ng2-toastr';

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

    isReadOnly: boolean = true;

    constructor(
        private userService: UserService,
        private calendarService: CalendarService,
        private toastr: ToastsManager
    ) {
        this.users = Observable.create((observer: any) => {
            userService.findUsersByEmailOrUserName(this.emailOrUserName)
                .subscribe(users => {
                    if (users == null)
                        return;

                    observer.next(users)
                });
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
                this.toastr.success("The calendar has been shared successfully.");
                this.selectedUser = new User();
            },
            (e: Response) => {
                this.toastr.error("Unable to share the calendar.");
            });
    }

    typeaheadOnSelect(e: TypeaheadMatch): void {
        this.selectedUser = e.item;
    }

    @Output()
    onCalendarShared = new EventEmitter<UserCalendar>();
}
