import { Component, OnInit } from '@angular/core';
import { DialogService, DialogComponent } from 'ng2-bootstrap-modal';
import { CalendarService } from '../../services/calendar.service';
import { User } from '../../../authentication/models/user';
import { UserCalendar } from '../../models/user.calendar';

export interface IShareCalendarModel {
    Id: Number;
    Name: string;
    Owner: User;
}

@Component({
    selector: 'app-share-calendar',
    templateUrl: './share-calendar.component.html'
})
export class ShareCalendarComponent 
                        extends DialogComponent<IShareCalendarModel, boolean> 
                        implements IShareCalendarModel, OnInit {  
    Id: Number;
    Name: string;
    Owner: User;

    public sharedUsers: UserCalendar[] = [];

    constructor(
        public dialogService: DialogService,
        private calendarService: CalendarService
    ) {
        super(dialogService);
     }


    ngOnInit(): void {
        this.calendarService.getSharedUsers(this.Id)
            .subscribe((users: UserCalendar[]) => {
                if (users != undefined)
                    this.sharedUsers = users;
            },
            (resp: Response) => {

            });
    }
}
