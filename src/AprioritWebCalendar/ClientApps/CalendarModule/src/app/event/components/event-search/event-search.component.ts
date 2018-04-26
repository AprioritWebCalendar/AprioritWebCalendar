import { Component, OnInit } from '@angular/core';
import { EventService } from '../../services/event.service';
import { Observable } from 'rxjs/Observable';
import { Event } from '../../models/event';

@Component({
    selector: 'app-event-search',
    templateUrl: './event-search.component.html'
})
export class EventSearchComponent implements OnInit {
    events: Observable<Event>;
    searchText: string;

    eventsTake: number = 5;

    constructor(
        private eventService: EventService
    ) { }

    ngOnInit() {
        this.events = Observable.create((observer: any) => {
            this.eventService.search(this.searchText, this.eventsTake)
                .subscribe(foundEvents => {
                    if (foundEvents != null && foundEvents.length > 0) {
                        observer.next(foundEvents);
                    }
                });
        });
    }

    eventClicked(event: Event) {
        alert("There will be a modal widnow with information about event.");
    }
}
