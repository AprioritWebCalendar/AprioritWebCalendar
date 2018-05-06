import { Component, OnInit } from '@angular/core';
import { EventService } from '../../services/event.service';
import { Observable } from 'rxjs/Observable';
import { Event } from '../../models/event';
import { IEventDetailsParams, EventDetailsComponent } from '../event-details/event-details.component';
import { DialogService } from 'ng2-bootstrap-modal';

@Component({
    selector: 'app-event-search',
    templateUrl: './event-search.component.html'
})
export class EventSearchComponent implements OnInit {
    public events: Observable<Event>;
    public searchText: string;

    public eventsTake: number = 5;

    private _searchText;

    constructor(
        private eventService: EventService,
        private _dialogService: DialogService
    ) { }

    public ngOnInit() {
        this.events = Observable.create((observer: any) => {
            this.eventService.search(this.searchText, this.eventsTake)
                .subscribe(foundEvents => {
                    this._searchText = this.searchText;

                    if (foundEvents != null && foundEvents.length > 0) {
                        observer.next(foundEvents);
                    }
                });
        });
    }

    public eventClicked(event: Event) {
        this.searchText = this._searchText;

        this.eventService.getById(event.Id)
            .subscribe(e => {
                let params: IEventDetailsParams = {
                    event: e
                };

                this._dialogService.addDialog(EventDetailsComponent, params);
            });
    }
}
