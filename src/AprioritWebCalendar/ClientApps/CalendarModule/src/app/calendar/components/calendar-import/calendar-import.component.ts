import { Component } from '@angular/core';
import { CalendarIcalService } from '../../services/calendar.ical.service';
import { DialogService } from 'ng2-bootstrap-modal';
import { CalendarCheck } from '../left-calendar-menu/calendar.check.model';
import { NgForm } from '@angular/forms';
import { CalendarImportPreviewModel } from '../../models/calendar-import.preview.model';
import { CalendarImportModel } from '../../models/calendar-import.model';
import { CustomDialogComponent } from '../../../services/custom.dialog.component';

@Component({
    selector: 'app-calendar-import',
    templateUrl: './calendar-import.component.html'
})
export class CalendarImportComponent extends CustomDialogComponent<CalendarImportModel, CalendarCheck> {

    constructor(
        public dialogService: DialogService,
        private iCalService: CalendarIcalService
    ) {
        super(dialogService);
    }

    public model: CalendarImportModel = new CalendarImportModel();
    public calendarPreview: CalendarImportPreviewModel = new CalendarImportPreviewModel();

    public errors: string[] = [];
    public isSave: boolean = false;
    public isLoading: boolean = false;

    public import(form: NgForm) : void {
        this.errors = [];

        if (!form.valid || this.model.Color == undefined || this.model.File == undefined)
            return;

        this.isLoading = true;
        this.model.Color = this.model.Color.toUpperCase();

        this.iCalService.importCalendar(this.model)
            .subscribe((preview: CalendarImportPreviewModel) => {
                this.calendarPreview = preview;
                this.isLoading = false;
                this.isSave = true;

                setTimeout(() => this.close(), 300000);
            }, resp => {
                var errors = resp.json();
                this.isLoading = false;

                if (errors instanceof Array) {
                    this.errors = errors;
                }
            });
    }

    public saveCalendar() : void {
        this.iCalService.saveCalendar(this.calendarPreview.Key)
            .subscribe(id => {
                let calendarCheck = new CalendarCheck();

                calendarCheck.Id = id;
                calendarCheck.Name = this.calendarPreview.Calendar.Name;
                calendarCheck.Description = this.calendarPreview.Calendar.Description;
                calendarCheck.Color = this.calendarPreview.Calendar.Color;
                calendarCheck.IsReadOnly = false;

                this.result = calendarCheck;
                this.close();
            }, resp => {
                var errors = resp.json();

                if (errors instanceof Array) {
                    this.errors = errors;
                }
            });
    }

    public onFileSelected(files: FileList) : void {
        this.model.File = files[0];
    }
}
