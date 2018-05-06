import { Component, Output, EventEmitter } from '@angular/core';

@Component({
    selector: 'app-invitations-incoming-button',
    templateUrl: './invitations-incoming-button.component.html',
    styleUrls: ['./invitations-incoming-button.component.css']
})
export class InvitationsIncomingButtonComponent {
    @Output()
    onInvitationsOpenClicked = new EventEmitter();

    constructor() { }

    private openClicked() : void {
        this.onInvitationsOpenClicked.emit();
    }
}
