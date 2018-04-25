import { Component, Output, EventEmitter, OnInit } from '@angular/core';
import { User } from '../../../authentication/models/user';
import { Observable } from 'rxjs/Observable';
import { Observer } from 'rxjs/Observer';
import { UserService } from '../../../services/user.service';
import { TypeaheadMatch } from 'ngx-bootstrap';
import { InviteRequestModel } from '../../models/invite.request.model';

@Component({
    selector: 'app-event-select-user-share',
    templateUrl: './event-select-user-share.component.html'
})
export class EventSelectUserShareComponent implements OnInit {
    public selectedUser: User = new User();
    public users: Observable<User[]>;

    public emailOrUserName: string;
    public isReadOnly: boolean = true;

    @Output()
    onUserInvited = new EventEmitter<InviteRequestModel>();

    constructor(
        private userService: UserService
    ) {
    }

    public ngOnInit(): void {
        this.users = Observable.create((observer: any) => {
            this.userService.findUsersByEmailOrUserName(this.emailOrUserName)
                .subscribe(users => {
                    console.log(users);

                    if (users != null && users.length > 0)
                        observer.next(users);
                });
        });
    }

    private inviteUser() {
        if (this.selectedUser.Id == null)
            return;

        let invitationRequest = new InviteRequestModel();
        invitationRequest.IsReadOnly = this.isReadOnly;
        invitationRequest.User = this.selectedUser;

        this.onUserInvited.emit(invitationRequest);
    }

    private cancelInvite() {
        this.emailOrUserName = null;
        this.selectedUser = null;
        this.isReadOnly = true;
    }
    
    private typeaheadOnSelect(e: TypeaheadMatch): void {
        this.selectedUser = e.item;
    }
}
