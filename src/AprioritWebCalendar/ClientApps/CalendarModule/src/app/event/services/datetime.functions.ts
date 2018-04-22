import { Injectable } from "@angular/core";
import * as moment from 'moment';
import { RRule } from 'rrule';
import { Event } from "../models/event";
import { Period } from "../models/period";
import { PeriodType } from "../models/period.type";

export function mergeDateTime(date: Date, time: string = null) : Date {
    if (time == null) {
        return moment(date).toDate();
    } else {
        let timeAsDate = getTimeAsDate(time);

        return moment(date)
            .add(timeAsDate.getHours(), 'hour')
            .add(timeAsDate.getMinutes(), 'minute')
            .toDate();
    }
}

export function setEndOfDay(date: Date) : Date {
    return moment(date)
        .add(1, 'day')
        .subtract(1, 'minute')
        .toDate();
}

export function getTimeAsDate(time: string) : Date {
    return moment(time, "HH:mm:ss").toDate();
}

export function getUtc(date: Date) : Date {
    return moment(date).utc().toDate();
}

export function getLocalTime(date: Date) : Date {
    // Yes! This is shit, trash, crutch.
    // I don't know, but it works.

    var local = moment.utc(moment(date).local().toString()).local().toDate();
    console.log(local);
    return local;
}

export function getRule(period: Period) : RRule {
    return new RRule({
        dtstart: new Date(period.PeriodStart.toLocaleString()),
        until: new Date(period.PeriodEnd.toLocaleString()),
        freq: this.getFrequency(period.Type),
        interval: period.Cycle != null ? period.Cycle : 1,
    });
}

export function getFrequency(type: PeriodType) : RRule.Frequency {
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
