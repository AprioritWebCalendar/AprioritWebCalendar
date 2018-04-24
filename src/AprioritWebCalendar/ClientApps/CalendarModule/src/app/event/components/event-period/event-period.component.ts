import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { PeriodRequestModel } from '../../models/period.request.model';

@Component({
    selector: 'app-event-period',
    templateUrl: './event-period.component.html'
})
export class EventPeriodComponent implements OnInit {
    startEndDate: Date[];

    ngOnInit(): void {
        this.startEndDate = [
            new Date(this.period.PeriodStart),
            new Date(this.period.PeriodEnd)
        ];
    }

    @Input()
    period: PeriodRequestModel = new PeriodRequestModel();

    @Output()
    onPeriodChanged = new EventEmitter<PeriodRequestModel>();

    emitPeriodChanged() {
        this.period.PeriodStart = this.startEndDate[0].toDateString();
        this.period.PeriodEnd = this.startEndDate[1].toDateString();

        this.onPeriodChanged.emit(this.period);
        console.log(this.period);
        console.log(this.startEndDate);
    }
}
