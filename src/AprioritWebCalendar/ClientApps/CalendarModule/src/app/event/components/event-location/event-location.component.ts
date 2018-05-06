import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Location } from '../../models/location';
import { MouseEvent } from '@agm/core';

@Component({
    selector: 'app-event-location',
    templateUrl: './event-location.component.html'
})
export class EventLocationComponent implements OnInit {
    
    ngOnInit(): void {
        if (this.location.Lattitude != null && this.location.Longitude != null)
            return;

        let geo = navigator.geolocation;

        if (geo) {
            geo.getCurrentPosition(position => {
                this.location.Longitude = position.coords.longitude;
                this.location.Lattitude = position.coords.latitude;

                console.log(position);

                this.emitLocationChanged();
            });
        } else {
            this.location.Lattitude = 48.4491614;
            this.location.Longitude = 35.2137371;

            console.log("Location from browser isn't available.");
            this.emitLocationChanged();
        }
    }

    @Input()
    location: Location;

    @Output()
    onLocationChanged = new EventEmitter<Location>();

    mapClicked($event: MouseEvent) {
        this.location.Longitude = $event.coords.lng;
        this.location.Lattitude = $event.coords.lat;
        this.emitLocationChanged();
    }

    emitLocationChanged() {
        this.onLocationChanged.emit(this.location);
        console.log(this.location)
    }
}
