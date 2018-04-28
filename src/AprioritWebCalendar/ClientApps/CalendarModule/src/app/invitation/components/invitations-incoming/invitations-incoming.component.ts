import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Invitation } from '../../models/invitation';
import { InvitationService } from '../../services/invitation.service';
import { ToastsManager } from 'ng2-toastr';

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
    public isLoading: boolean = false;

    constructor(
        private invitationService: InvitationService,
        private toastr: ToastsManager
    ) { }

    public ngOnInit() : void {
        this.isLoading = true;

        this.invitationService.getIncomingInvitations()
            .subscribe(i => {
                if (i != null) {
                    this.invitations = i;
                }
                this.isLoading = false;
            }, e => {
                this.isLoading = false;
                this.isError = true;
            });
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
}
