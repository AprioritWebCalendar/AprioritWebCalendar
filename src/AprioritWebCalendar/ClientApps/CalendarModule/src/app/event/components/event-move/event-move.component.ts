import { Component, OnInit } from '@angular/core';
import { DialogComponent, DialogService } from 'ng2-bootstrap-modal';
import { EventService } from '../../services/event.service';
import { Event } from '../../models/event';
import { Calendar } from '../../../calendar/models/calendar';
import { NgForm } from '@angular/forms';

export interface IEventMoveParams {
    event: Event;
    calendars: Calendar[];
}

@Component({
    selector: 'app-event-move',
    templateUrl: './event-move.component.html'
})
export class EventMoveComponent extends DialogComponent<IEventMoveParams, Event>
                                implements OnInit, IEventMoveParams {
    constructor(
        public dialogService: DialogService,
        private eventService: EventService
    ) {
        super(dialogService);
    }

    event: Event;
    calendars: Calendar[];

    calendarToMoveId: number;

    errors: string[] = [];

    ngOnInit(): void {
        this.calendarToMoveId = this.event.CalendarId;
    }

    private moveEvent(moveForm: NgForm) : void {
        this.errors = [];

        if (!moveForm.valid)
            return;

        if (this.calendarToMoveId == this.event.CalendarId) {
            this.successCallback(true);
        } else {
            this.eventService.moveEvent(this.event.Id, this.event.CalendarId, this.calendarToMoveId)
                .subscribe(isOk => {
                    this.event.CalendarId = this.calendarToMoveId;
                    this.event.Color = this.calendars.filter(c => c.Id == this.calendarToMoveId)[0].Color;

                    this.successCallback(isOk)   
                },
                (resp: Response) => {
                    var errors = resp.json();

                    if (errors instanceof Array) {
                        this.errors = errors;
                    }
                });
        }
    }

    private successCallback(isOk: boolean) : void {
        this.result = this.event;
        this.close();
    }
}
