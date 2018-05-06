import { Component } from '@angular/core';
import { EventRequestModel } from '../../models/event.request.model';
import { DialogComponent, DialogService } from 'ng2-bootstrap-modal';
import { NgForm } from '@angular/forms';
import { Calendar } from '../../../calendar/models/calendar';
import { Location } from '../../models/location';
import { Period } from '../../models/period';
import { mergeDateTime, getWithoutTime, getTimeAsString, getTimeAsDate } from '../../services/datetime.functions';
import { EventService } from '../../services/event.service';
import { Event } from '../../models/event';
import { PeriodRequestModel } from '../../models/period.request.model';

export interface IEventCreateParams {
    calendars: Calendar[];
}

@Component({
    selector: 'app-event-create',
    templateUrl: './event-create.component.html'
})
export class EventCreateComponent extends DialogComponent<IEventCreateParams, Event>
                                    implements IEventCreateParams {
    errors: string[] = [];
    model: EventRequestModel = new EventRequestModel();
    calendars: Calendar[];

    startEndDate: Date[];
    startTime?: Date;
    endTime? :Date;

    minDate: Date = new Date();

    constructor(
        public dialogService: DialogService,
        private eventService: EventService
    ){
        super(dialogService);
        this.startEndDate = [new Date(), new Date()];
    }

    // TODO: Localize timepickers.
    // TODO: Localize datepickers.
    // TODO: Validate timepickers.
    // TODO: Validate datepickers.

    private createEvent(createForm: NgForm) {
        this.errors = [];

        let event: EventRequestModel = Object.assign(this.model);

        if (!this.model.IsAllDay) {
            event.StartTime = this.startTime.toTimeString();
            event.EndTime = this.endTime.toTimeString();
            console.log([event.StartTime, event.EndTime]);
        }
        
        console.log(event);

        event.ConvertDateTime([this.startEndDate[0].toDateString(), this.startEndDate[1].toDateString()]);

        console.log(event);

        this.eventService.createEvent(event)
            .subscribe(id => {
                event.Id = id;
                
                this.result = event.ToEvent();
                this.close();
            }, (resp: Response) => {
                var errors = resp.json();

                if (errors instanceof Array) {
                    this.errors = errors;
                }
            });
    }

    private changeLocation(location: Location) {
        this.model.Location = location;
    }

    private changePeriod(period: PeriodRequestModel) {
        this.model.Period = period;
    }
}
