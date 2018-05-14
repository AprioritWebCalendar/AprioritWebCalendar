import { Component, OnInit } from '@angular/core';
import { DialogService } from 'ng2-bootstrap-modal';
import { CalendarService } from '../../services/calendar.service';
import { Calendar } from '../../models/calendar';
import { CustomDialogComponent } from '../../../services/custom.dialog.component';

export interface ICalendarDeleteModel {
    calendar: Calendar;
    userId: number
}

@Component({
    selector: 'app-calendar-delete',
    templateUrl: './calendar-delete.component.html'
})
export class CalendarDeleteComponent extends CustomDialogComponent<ICalendarDeleteModel, boolean> implements ICalendarDeleteModel {
    calendar: Calendar;
    userId: number;
    
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

         this.calendarService.deleteCalendar(this.calendar.Id)
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
