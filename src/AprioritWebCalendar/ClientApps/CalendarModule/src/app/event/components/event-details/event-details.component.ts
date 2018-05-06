import { Component } from '@angular/core';
import { DialogComponent, DialogService } from 'ng2-bootstrap-modal';
import { Event } from '../../models/event';
import { PeriodType } from '../../models/period.type';

export interface IEventDetailsParams {
    event: Event;
}

@Component({
    selector: 'app-event-details',
    templateUrl: './event-details.component.html'
})
export class EventDetailsComponent extends DialogComponent<IEventDetailsParams, boolean>
                                    implements IEventDetailsParams {

    constructor(public _dialogService: DialogService) {
        super(_dialogService);
    }

    public event: Event;

    public GetPeriodType() : string {
        switch (this.event.Period.Type) {
            case PeriodType.Custom:
                return "Daily";

            case PeriodType.Monthly:
                return "Monthly";

            case PeriodType.Weekly:
                return "Weekly";

            case PeriodType.Yearly:
                return "Yearly";
        }
    }
}
