import { Component, OnInit } from '@angular/core';
import { User } from '../../../authentication/models/user';
import { Event } from '../../models/event';
import { DialogComponent, DialogService } from 'ng2-bootstrap-modal';
import { EventService } from '../../services/event.service';

export interface IEventDeleteParams {
    event: Event;
    currentUser: User;
}

@Component({
    selector: 'app-event-delete',
    templateUrl: './event-delete.component.html'
})
export class EventDeleteComponent extends DialogComponent<IEventDeleteParams, number>
                                    implements OnInit, IEventDeleteParams {
    event: Event;
    currentUser: User;

    isOwner: boolean;

    constructor(
        public dialogService: DialogService,
        private eventService: EventService
    ) {
        super(dialogService);
    }

    public ngOnInit(): void {
        this.isOwner = this.event.Owner.Id == this.currentUser.Id;
    }

    private deleteEvent() : void {
        if (!confirm("Are you sure?"))
            this.close();

        if (this.isOwner) {
            this.eventService.deleteEvent(this.event.Id)
                .subscribe(isOk => this.successCallback(isOk));
        } else {
            this.eventService.deleteInvitedUser(this.event.Id, this.currentUser.Id as number)
                .subscribe(isOk => this.successCallback(isOk));
        }
    }

    private successCallback(isOk: boolean) : void {
        this.result = this.event.Id;
        this.close();
    }
}
