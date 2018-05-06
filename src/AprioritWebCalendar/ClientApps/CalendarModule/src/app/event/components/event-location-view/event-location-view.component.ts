import { Component, Input } from '@angular/core';
import { Location } from '../../models/location';

@Component({
    selector: 'app-event-location-view',
    templateUrl: './event-location-view.component.html'
})
export class EventLocationViewComponent {
    @Input()
    location: Location;
}
