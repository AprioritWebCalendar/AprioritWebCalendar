import { Component, OnInit } from '@angular/core';
import { DialogComponent, DialogService } from 'ng2-bootstrap-modal';
import { CalendarService } from '../../services/calendar.service';

export interface ICalendarDeleteModel {
    Id: Number,
    Name: string;
}

@Component({
    selector: 'app-calendar-delete',
    templateUrl: './calendar-delete.component.html'
})
export class CalendarDeleteComponent extends DialogComponent<ICalendarDeleteModel, boolean> implements ICalendarDeleteModel {
    Id: Number;
    Name: string;

    public errors: string[];

    constructor(
        public dialogService: DialogService,
        private calendarService: CalendarService
    ) {
        super(dialogService);
     }

     deleteCalendar() {
        this.errors = [];

        if (!confirm("Are you sure?"))
            return;

        if (!confirm("Really?"))
            return;

         this.calendarService.deleteCalendar(this.Id)
            .subscribe((isOk: boolean) => {
                if (isOk) {
                    this.result = true;
                    this.close();
                }
            },
            (resp: Response) => {
                var errors = resp.json();

                if (errors instanceof Array) {
                    this.errors = errors;
                }
            });
     }

}
