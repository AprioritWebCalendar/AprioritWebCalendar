import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';

@Component({
    selector: 'app-calendar-view-switcher',
    templateUrl: './calendar-view-switcher.component.html'
})
export class CalendarViewSwitcherComponent {
    @Input()
    viewMode: string;

    @Output()
    onViewModeChanged = new EventEmitter<string>();

    onCalendarViewModeChanged() {
        console.log(`Calendar view mode has been changed: ${this.viewMode}.`);
        this.onViewModeChanged.emit(this.viewMode);
    }
}
