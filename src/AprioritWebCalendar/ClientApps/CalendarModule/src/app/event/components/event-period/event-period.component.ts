import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { PeriodRequestModel } from '../../models/period.request.model';

@Component({
    selector: 'app-event-period',
    templateUrl: './event-period.component.html'
})
export class EventPeriodComponent {
    startEndDate: string[];

    @Input()
    period: PeriodRequestModel = new PeriodRequestModel();

    @Output()
    onPeriodChanged = new EventEmitter<PeriodRequestModel>();

    emitPeriodChanged() {
        this.period.PeriodStart = this.startEndDate[0];
        this.period.PeriodEnd = this.startEndDate[1];

        this.onPeriodChanged.emit(this.period);
        console.log(this.period);
        console.log(this.startEndDate);
    }
}
