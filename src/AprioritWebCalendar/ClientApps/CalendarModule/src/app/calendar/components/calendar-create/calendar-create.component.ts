import { Component, OnInit } from '@angular/core';
import { DialogService, DialogComponent } from 'ng2-bootstrap-modal';
import { CalendarService } from '../../services/calendar.service';
import { CalendarCreateModel } from './calendar-create.model';
import { NgForm } from '@angular/forms';
import { CalendarCheck } from '../left-calendar-menu/calendar.check.model';
import { Calendar } from '../../models/calendar';

@Component({
    selector: 'app-calendar-create',
    templateUrl: './calendar-create.component.html'
})
export class CalendarCreateComponent extends DialogComponent<CalendarCreateModel, CalendarCheck> {
    public model: CalendarCreateModel = new CalendarCreateModel();
    public errors: string[];

    constructor(
        public dialogService: DialogService,
        private calendarService: CalendarService
    ) {
        super(dialogService);
     }

     createCalendar(createForm: NgForm) {
        this.errors = [];

        if (!createForm.valid || this.model.Color == undefined)
            return;

        let calendar = new Calendar();
        calendar.Name = this.model.Name;
        calendar.Description = this.model.Description;
        calendar.Color = this.model.Color;

        this.calendarService.createCalendar(calendar)
            .subscribe((id: Number) => {
                let result = new CalendarCheck();
                result.Id = id;
                result.Name = this.model.Name;
                result.Description = this.model.Description;
                result.Color = this.model.Color;

                this.result = result;
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