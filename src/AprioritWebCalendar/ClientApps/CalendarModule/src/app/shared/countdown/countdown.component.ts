import { Component, OnInit, Input } from '@angular/core';
import * as moment from 'moment';

@Component({
    selector: 'app-countdown',
    templateUrl: './countdown.component.html'
})
export class CountdownComponent implements OnInit {

    @Input()
    seconds: number;

    timeString: string;

    public ngOnInit(): void {
        setInterval(() => {
            console.log(this.seconds);

            if (this.seconds <= 0) {
                this.timeString = "Time Elapsed!"
                return;
            }

            this.seconds--;

            this.timeString = moment().startOf("day")
                .seconds(this.seconds)
                .format(this.seconds >= 60 * 60 ? "HH:mm:ss" : "mm:ss");
        }, 1000);
    }
}
