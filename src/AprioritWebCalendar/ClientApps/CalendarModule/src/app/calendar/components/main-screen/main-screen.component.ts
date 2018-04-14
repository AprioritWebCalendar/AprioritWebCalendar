import { Component, OnInit } from '@angular/core';
import { MonthViewDay } from 'calendar-utils';

@Component({
    selector: 'app-main-screen',
    templateUrl: './main-screen.component.html'
})
export class MainScreenComponent implements OnInit {
    events: any[] = [];

    public viewDate: Date = new Date();
    public viewMode: string = "month";

    constructor() { }

    ngOnInit() {
    }

    loadEvents(ids: Number[]) {
        console.log(ids.join(', '));
    }

    changeViewMode(viewMode: string) {
        this.viewMode = viewMode;
    }

    dayClicked(day: MonthViewDay) {
        console.log(day);
    }

    viewDateChanged(date: Date) {
        console.log(`View date has been changed ${date}`);
    }

    openCreateEventModal() {
        alert("There will be a modal window to create an event.");
    }
}
