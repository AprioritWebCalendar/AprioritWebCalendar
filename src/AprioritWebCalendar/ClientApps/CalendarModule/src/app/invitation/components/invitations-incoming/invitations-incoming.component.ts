import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Invitation } from '../../models/invitation';
import { InvitationService } from '../../services/invitation.service';
import { ToastsManager } from 'ng2-toastr';
import { InvitationListener } from '../../services/invitation.listener';
import { PushNotificationService } from '../../../services/push.notification.service';
import { Event } from '../../../event/models/event';

@Component({
    selector: 'app-invitations-incoming',
    templateUrl: './invitations-incoming.component.html',
    styleUrls: ['./invitations-incoming.component.css']
})
export class InvitationsIncomingComponent implements OnInit {
    @Input()
    public isSidebarOpened: boolean;

    @Output()
    public onInvitationAccepted = new EventEmitter<Invitation>();

    public invitations: Invitation[] = [];
    public isError: boolean = false;

    constructor(
        private invitationService: InvitationService,
        private toastr: ToastsManager,
        private invitationListener: InvitationListener,
        private pushNotifService: PushNotificationService
    ) { }

    public ngOnInit() : void {
        this.configureSignalR();
    }

    public acceptInvitation(invitation: Invitation): void {
        this.invitationService.acceptInvitation(invitation.Event.Id)
            .subscribe(isOk => {
                this.removeFromList(invitation);
                this.onInvitationAccepted.emit(invitation);

                this.toastr.success("The invitation has been accepted successfully.");
            }, e => {
                this.toastr.error("Unable to accept invitation. Try to reload the page.")
            });
    }

    public rejectInvitation(invitation: Invitation) : void {
        this.invitationService.rejectInvitation(invitation.Event.Id)
            .subscribe(isOk => {
                this.removeFromList(invitation);
            }, e => {
                this.toastr.error("Unable to reject invitation. Try to reload the page.");
            });
    }

    private removeFromList(invitation: Invitation) : void {
        this.invitations.splice(this.invitations.indexOf(invitation), 1);
    }

    private configureSignalR() : void {
        this.invitationListener.OnIncomingInvitationsReceived((invitations: Invitation[]) => {
            if (invitations != null && invitations.length > 0) {
                console.log(invitations);

                this.invitations = invitations;
            }
        });

        this.invitationListener.OnUserInvited((i: Invitation) => {
            console.log(i);

            if (i != null) {
                this.pushNotifService.PushNotification(`Has invited you to go to event "${i.Event.Name}".`, 
                    i.Invitator.UserName as string);

                this.invitations.push(i);
            }
        });

        this.invitationListener.OnInvitationDeleted((name, id, invitator) => {
            this.invitations = this.invitations.filter(i => i.Event.Id != id);
            this.pushNotifService.PushNotification(`Has deleted invitation on event "${name}".`, invitator);
        });
        
        this.invitationListener.Start();
    }
}
