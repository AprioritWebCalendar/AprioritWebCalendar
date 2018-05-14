import { Component, OnInit } from '@angular/core';
import { Calendar } from '../../models/calendar';
import { DialogService } from 'ng2-bootstrap-modal';
import { CalendarService } from '../../services/calendar.service';
import { CalendarCheck } from '../left-calendar-menu/calendar.check.model';
import { NgForm } from '@angular/forms';
import { CustomDialogComponent } from '../../../services/custom.dialog.component';

export interface ICalendarEditModel {
    Id: Number;
    Name: string;
    Description: string;
    Color: string;
}

@Component({
    selector: 'app-calendar-edit',
    templateUrl: './calendar-edit.component.html'
})
export class CalendarEditComponent extends CustomDialogComponent<ICalendarEditModel, ICalendarEditModel> implements ICalendarEditModel {
    Id: Number;
    Name: string;
    Description: string;
    Color: string;

    public errors: string[];

    constructor(
        public dialogService: DialogService,
        private calendarService: CalendarService
    ) {
        super(dialogService);
    }

    
    editCalendar(editForm: NgForm) {
        this.errors = [];

        if (!editForm.valid || this.Color == undefined)
            return;

        let calendar = new Calendar();
        calendar.Name = this.Name;
        calendar.Description = this.Description;
        calendar.Color = this.Color.toUpperCase();

        this.calendarService.updateCalendar(this.Id, calendar)
            .subscribe((isOk: boolean) => {
                if (!isOk)
                    return;

                var result = {
                  Name: this.Name,
                  Description: this.Description,
                  Color: this.Color  
                };

                this.result = result as ICalendarEditModel;
                this.close();
            },
            (resp: Response) => {
                var errors = resp.json();

                if (errors instanceof Array) {
                    this.errors = errors;
                }
            });
    }
}
