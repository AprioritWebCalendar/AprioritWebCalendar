import { Component, OnInit } from '@angular/core';
import { Event } from '../../models/event';
import { DialogComponent, DialogService } from 'ng2-bootstrap-modal';
import { EventService } from '../../services/event.service';
import { UserInvited } from '../../models/user.invited';
import { ToastsManager } from 'ng2-toastr';
import { InviteRequestModel } from '../../models/invite.request.model';

export interface IEventShareParams {
    event: Event;
}

@Component({
    selector: 'app-event-share',
    templateUrl: './event-share.component.html'
})
export class EventShareComponent extends DialogComponent<IEventShareParams, boolean> 
                                 implements OnInit, IEventShareParams {

    constructor(
        public dialogService: DialogService,
        private eventService: EventService,
        private toastr: ToastsManager
    ) {
        super(dialogService);
    }

    event: Event;
    users: UserInvited[] = [];
    isError: boolean = false;

    public ngOnInit() : void {
        this.eventService.getUsers(this.event.Id)
            .subscribe(u => {
                if (u == null)
                    return;
                    
                this.users = u;
            }, e => this.isError = true);
    }

    private inviteUser(invitation: InviteRequestModel) {
        let errorMessage = "Unable to invite user. Try to reload the page";

        this.eventService.inviteUser(this.event.Id, invitation.User.Id as number, invitation.IsReadOnly)
            .subscribe(isOk => {
                if (!isOk) {
                    this.errorCallback(errorMessage);
                    return;
                } else {
                    let user = new UserInvited();
                    user.User = invitation.User;
                    user.IsReadOnly = invitation.IsReadOnly;
                    user.IsAccepted = false;

                    this.users.push(user);
                    this.toastr.success("The user has been invited successfully.");
                }
            }, e => this.errorCallback(errorMessage));
    }

    private removeUser(user: UserInvited) : void {
        let message = "The user has been removed";
        let errorMessage = "Unable to remove user. Try to reload the page.";

        if (user.IsAccepted) {
            this.eventService.deleteInvitedUser(this.event.Id, user.User.Id as number)
                .subscribe(isOk => {
                    if (isOk) {
                        this.users = this.users.filter(u => u.User.Id != user.User.Id);
                        this.successCallback(message);
                    }
                    else {
                        this.errorCallback(errorMessage);
                    }
                }, e => this.errorCallback(errorMessage));
        } else {
            this.eventService.deleteInvitation(this.event.Id, user.User.Id as number)
                .subscribe(isOk => {
                    if (isOk)
                        this.successCallback(message);
                    else
                        this.errorCallback(errorMessage);
                }, e => this.errorCallback(errorMessage));
        }
    }

    private changeReadOnlyState(user: UserInvited) : void {
        let message = "ReadOnly state has been changed.";
        let errorMessage = "Unable to change ReadOnly state. Try to reload the page.";

        console.log("Changing read-only state for: " + user.User.UserName);

        if (user.IsAccepted) {
            this.eventService.setEventReadOnlyState(this.event.Id, user.User.Id as number, user.IsReadOnly)
            .subscribe(isOk => {
                if (isOk)
                    this.successCallback(message);
                else
                    this.errorCallback(errorMessage);
            }, e => this.errorCallback(errorMessage));
        } else {
            this.eventService.setInvitationReadOnlyState(this.event.Id, user.User.Id as number, user.IsReadOnly).subscribe(isOk => {
                if (isOk)
                    this.successCallback(message);
                else
                    this.errorCallback(errorMessage);
            }, e => this.errorCallback(errorMessage));
        }
    }

    private successCallback(message: string) {
        this.toastr.success(message);
    }

    private errorCallback(message: string) {
        this.toastr.error(message);
    }
}
