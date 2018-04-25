import { Component, Input, Output, EventEmitter } from '@angular/core';
import { UserInvited } from '../../models/user.invited';

@Component({
    selector: 'app-event-share-users',
    templateUrl: './event-share-users.component.html'
})
export class EventShareUsersComponent {
    @Input()
    users: UserInvited[];

    @Output()
    onUserRemoved = new EventEmitter<UserInvited>();

    @Output()
    onReadOnlyStateChanged = new EventEmitter<UserInvited>();

    private removeUser(user: UserInvited) : void {
        console.log("Removing user.");
        this.onUserRemoved.emit(user);
    }

    private readOnlyStateChanged(user: UserInvited) : void {
        console.log("Changing readonly state.");
        this.onReadOnlyStateChanged.emit(user);
    }
}
