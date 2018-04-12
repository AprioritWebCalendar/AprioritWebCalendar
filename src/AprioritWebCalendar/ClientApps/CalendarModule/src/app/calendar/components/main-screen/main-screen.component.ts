import { Component, OnInit } from '@angular/core';

@Component({
    selector: 'app-main-screen',
    templateUrl: './main-screen.component.html'
})
export class MainScreenComponent implements OnInit {
    events: any[] = [];
    viewDate: Date = new Date();

    viewMode: string = "Month";

    constructor() { }

    ngOnInit() {
    }

    loadEvents(ids: Number[]) {
        console.log(ids.join(', '));
    }

    changeViewMode(viewMode: string) {
        this.viewMode = viewMode;
    }
}
