import { Component, OnInit } from '@angular/core';
import { EventRequestModel } from '../../models/event.request.model';
import { DialogService, DialogComponent } from 'ng2-bootstrap-modal';
import { EventService } from '../../services/event.service';
import { Location } from '../../models/location';
import { PeriodRequestModel } from '../../models/period.request.model';
import { NgForm } from '@angular/forms';
import { Event } from '../../models/event';
import { getLocalTime, getTimeAsDate, getTimeAsString } from '../../services/datetime.functions';

export interface IEventEditParams {
    model: EventRequestModel;
}

@Component({
    selector: 'app-event-edit',
    templateUrl: './event-edit.component.html',
    styleUrls: ['./event-edit.component.css']
})
export class EventEditComponent extends DialogComponent<IEventEditParams, Event>
                                implements OnInit, IEventEditParams {
    model: EventRequestModel = new EventRequestModel();
    errors: string[] = [];

    startEndDate: Date[];
    startTime?: Date;
    endTime? :Date;

    constructor(
        public dialogService: DialogService,
        private eventService: EventService
    ) {
        super(dialogService);
    }

    ngOnInit(): void {
        this.startEndDate = [
            new Date(this.model.StartDate),
            new Date(this.model.EndDate)
        ];

        if (!this.model.IsAllDay) {
            this.startTime = getLocalTime(getTimeAsDate(this.model.StartTime));
            this.endTime = getLocalTime(getTimeAsDate(this.model.EndTime));
        }

        console.log(this.model);
    }

    editEvent(editForm: NgForm) {
        this.errors = [];

        if (!editForm.valid)
            return;

        let event: EventRequestModel = Object.assign(this.model);

        if (!this.model.IsAllDay) {
            event.StartTime = this.startTime.toTimeString();
            event.EndTime = this.endTime.toTimeString();
            console.log([event.StartTime, event.EndTime]);
        }

        event.ConvertDateTime([this.startEndDate[0].toDateString(), this.startEndDate[1].toDateString()]);

        this.eventService.updateEvent(this.model.Id, event)
            .subscribe(isOk => {
                this.result = event.ToEvent();
                this.close();
            }, (resp: Response) => {
                var errors = resp.json();

                if (errors instanceof Array) {
                    this.errors = errors;
                }
            });
    }

    changeLocation(location: Location) {
        this.model.Location = location;
    }

    changePeriod(period: PeriodRequestModel) {
        this.model.Period = period;
    }
}
