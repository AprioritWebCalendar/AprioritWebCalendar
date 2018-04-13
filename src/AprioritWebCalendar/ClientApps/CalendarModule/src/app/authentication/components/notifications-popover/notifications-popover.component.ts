import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from '../../services/authentication.service';

@Component({
    selector: 'app-notifications-popover',
    templateUrl: './notifications-popover.component.html',
    styleUrls: ['./notifications-popover.component.css']
})
export class NotificationsPopoverComponent implements OnInit {
    unreadCount: Number;
    notifications: any[];

    constructor(
        public authService: AuthenticationService
    ) { }

    ngOnInit() {
        // That's some data just to demonstrate the component.

        this.unreadCount = 8;

        this.notifications = [
            { Title: "Meeting with Tomas", Description: "Friday, 13th of April, 10 AM" },
            { Title: "Return money", Description: "Monday, 16th of April. Return 10 UAH to Zheka and Pashka." },
            { Title: "Meeting with the tutor", Description: "Tuesday, 17th of April, 5 PM" }
        ];
    }

}
