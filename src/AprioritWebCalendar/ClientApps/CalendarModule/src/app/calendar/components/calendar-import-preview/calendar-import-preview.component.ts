import { Component, Input, OnInit } from '@angular/core';
import { CalendarImportPreviewModel } from '../../models/calendar-import.preview.model';
import { Event } from '../../../event/models/event';

@Component({
    selector: 'app-calendar-import-preview',
    templateUrl: './calendar-import-preview.component.html'
})
export class CalendarImportPreviewComponent implements OnInit {
    @Input()
    public model: CalendarImportPreviewModel;

    public events: Event[] = [];

    public ngOnInit(): void {
        this.events = this.model.Calendar.Events.map(e => e.Event);
    }

    public getLocaleDate(d: Date) : string {
        return new Date(d).toLocaleDateString();
    }
}
