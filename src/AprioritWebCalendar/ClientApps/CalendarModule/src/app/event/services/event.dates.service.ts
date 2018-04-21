import { Injectable } from "@angular/core";
import * as moment from 'moment';
import { RRule } from 'rrule';
import { Event } from "../models/event";
import { Period } from "../models/period";
import { PeriodType } from "../models/period.type";

@Injectable()
export class EventDatesService {
    public getTimeAsDate(time: string) {
        return moment(time, "HH:mm:ss").toDate();
    }

    public getStartDate(event: Event): Date {
        if (event.IsAllDay) {
            return new Date(event.StartDate.toLocaleString());
        } else {
            let startTime = moment(event.StartTime, "HH:mm:ss").toDate();

            return moment(event.StartDate.toLocaleString())
                .add(startTime.getHours(), 'hour')
                .add(startTime.getMinutes(), 'minute')
                .toDate();
        }
    }

    public getEndDate(event: Event): Date {
        if (event.IsAllDay) {
            return moment(event.EndDate.toLocaleString())
                .add(1, 'day')
                .subtract(1, 'minute')
                .toDate();
        } else {
            let endTime = moment(event.EndTime, "HH:mm:ss").toDate();

            return moment(event.EndDate.toLocaleString())
                .add(endTime.getHours(), 'hour')
                .add(endTime.getMinutes(), 'minute')
                .toDate();
        }
    }

    public getRule(period: Period): RRule {
        return new RRule({
            dtstart: new Date(period.PeriodStart.toLocaleString()),
            until: new Date(period.PeriodEnd.toLocaleString()),
            freq: this.getFrequency(period.Type),
            interval: period.Cycle != null ? period.Cycle : 1,
        });
    }

    private getFrequency(type: PeriodType): RRule.Frequency {
        switch (type) {
            case PeriodType.Yearly:
                return RRule.YEARLY;

            case PeriodType.Monthly:
                return RRule.MONTHLY;

            case PeriodType.Weekly:
                return RRule.WEEKLY;

            default:
                return RRule.DAILY;
        }
    }
}