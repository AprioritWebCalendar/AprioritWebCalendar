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

export function getTimeAsString(date: Date) : string {
    return moment(date).utc().format("HH:mm");
}

export function getTimeAsLocalString(date: Date) : string {
    return moment(date).format("HH:mm");
}

export function getWithoutTime(date: Date) : Date {
    return moment(date).startOf('day').toDate();
}

export function getLocalTime(date: Date) : Date {
    // Yes! This is shit, trash, crutch.
    // I don't know, but it works.

    return moment.utc(moment(date).local().toString()).local().toDate();
}

export function getRule(period: Period) : RRule {
    var rrule = new RRule({
        dtstart: getWithoutTime(period.PeriodStart),
        until: getWithoutTime(period.PeriodEnd),
        freq: getFrequency(period.Type),
        interval: period.Cycle == null ? 1 : period.Cycle,
    });

    return rrule;
}

export function getFrequency(type: PeriodType) : RRule.Frequency {
    if (type == PeriodType.Yearly)
        return RRule.YEARLY;
    else if (type == PeriodType.Monthly)
        return RRule.MONTHLY;
    else if (type == PeriodType.Weekly)
        return RRule.WEEKLY;
    else
        return RRule.DAILY;
}
