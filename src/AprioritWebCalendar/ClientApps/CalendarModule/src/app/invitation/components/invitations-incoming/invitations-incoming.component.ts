import { Component, OnInit, Input } from '@angular/core';

@Component({
    selector: 'app-invitations-incoming',
    templateUrl: './invitations-incoming.component.html',
    styleUrls: ['./invitations-incoming.component.css']
})
export class InvitationsIncomingComponent implements OnInit {
    @Input()
    isSidebarOpened: boolean;

    constructor() { }

    ngOnInit() {
    }

}
