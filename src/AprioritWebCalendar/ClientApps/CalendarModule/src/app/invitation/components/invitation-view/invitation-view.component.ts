import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { Invitation } from '../../models/invitation';
import { InvitationShortEventModel } from './invitation.short.event.model';

@Component({
    selector: 'app-invitation-view',
    templateUrl: './invitation-view.component.html',
    styleUrls: ['./invitation-view.component.css']
})
export class InvitationViewComponent implements OnInit {
    public eventInfo: InvitationShortEventModel = new InvitationShortEventModel();

    @Input()
    public invitation: Invitation;

    @Output()
    public onAccepted = new EventEmitter<Invitation>();

    @Output()
    public onRejected = new EventEmitter<Invitation>();

    public ngOnInit(): void {
        this.eventInfo = InvitationShortEventModel.FromEvent(this.invitation.Event);
    }

    public accept() : void {
        if (!confirm(`Do you really want to accept invitation on event "${this.invitation.Event.Name}"?`))
            return;

        this.onAccepted.emit(this.invitation);
    }

    public reject() : void {
        if (!confirm(`Do you really want to reject invitation on event "${this.invitation.Event.Name}"?`))
            return;

        this.onRejected.emit(this.invitation);
    }
}
