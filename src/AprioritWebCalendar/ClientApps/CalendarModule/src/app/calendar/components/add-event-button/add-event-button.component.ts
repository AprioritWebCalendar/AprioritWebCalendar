import { Component, Output, EventEmitter } from '@angular/core';

@Component({
    selector: 'app-add-event-button',
    templateUrl: './add-event-button.component.html',
    styleUrls: ['./add-event-button.component.css']
})
export class AddEventButtonComponent {
    @Output()
    onCreateEventClicked = new EventEmitter();

    createClicked() {
        this.onCreateEventClicked.emit();
    }
}
