import { Component } from '@angular/core';
import { DialogService } from 'ng2-bootstrap-modal';
import { CalendarIcalService } from '../../services/calendar.ical.service';
import { NgForm } from '@angular/forms';
import { saveAs } from 'file-saver/FileSaver';
import { ToastsManager } from 'ng2-toastr';
import { CustomDialogComponent } from '../../../services/custom.dialog.component';

export interface ICalendarExportParams {
    id: number;
    fileName: string;
}

@Component({
    selector: 'app-calendar-export',
    templateUrl: './calendar-export.component.html'
})
export class CalendarExportComponent extends CustomDialogComponent<ICalendarExportParams, boolean>
                                        implements ICalendarExportParams {

    public id: number;
    public fileName: string;

    constructor(
        public dialogService: DialogService,
        private icalService: CalendarIcalService,
        private toastr: ToastsManager
    ) {
        super(dialogService);
    }

    public export(form: NgForm) : void {
        if (!form.valid)
            return;

        this.icalService.exportCalendar(this.id)
            .subscribe(resp => {
                let blob = new Blob([resp.text()], { type: "text/calendar" });
                saveAs(blob, `${this.fileName}.ics`);

                this.toastr.success("The calendar has been exported as .ics successfully.");
                this.result = true;
                this.close();
            }, e => {
                this.toastr.error("Unable to export the calendar. Try to reload the page.");
            });
    }
}
