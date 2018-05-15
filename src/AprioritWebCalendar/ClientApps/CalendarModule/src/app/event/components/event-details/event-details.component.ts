import { Component } from '@angular/core';
import { DialogService } from 'ng2-bootstrap-modal';
import { Event } from '../../models/event';
import { PeriodType } from '../../models/period.type';
import { CustomDialogComponent } from '../../../services/custom.dialog.component';

export interface IEventDetailsParams {
    event: Event;
}

@Component({
    selector: 'app-event-details',
    templateUrl: './event-details.component.html'
})
export class EventDetailsComponent extends CustomDialogComponent<IEventDetailsParams, boolean>
                                    implements IEventDetailsParams {

    constructor(public _dialogService: DialogService) {
        super(_dialogService);
    }

    public event: Event;
}
