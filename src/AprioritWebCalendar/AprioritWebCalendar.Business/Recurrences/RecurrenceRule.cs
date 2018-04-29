using System;
using System.Collections.Generic;
using AprioritWebCalendar.Business.DomainModels;
using AprioritWebCalendar.Infrastructure.Enums;

namespace AprioritWebCalendar.Business.Recurrences
{
    public class RecurrenceRule
    {
        private readonly Func<DateTime, DateTime> _addFunc;
        private readonly Period _period;

        public RecurrenceRule(Period period)
        {
            _period = period;
            var add = period.Type == PeriodType.Custom ? period.Cycle.Value : 1;

            switch (period.Type)
            {
                case PeriodType.Yearly:
                    _addFunc = (d) => d.AddYears(add);
                    break;

                case PeriodType.Monthly:
                    _addFunc = (d) => d.AddMonths(add);
                    break;

                case PeriodType.Weekly:
                    _addFunc = (d) => d.AddDays(add * 7);
                    break;

                case PeriodType.Custom:
                    _addFunc = (d) => d.AddDays(add);
                    break;
            }
        }

        public IEnumerable<DateTime> GetDates()
        {
            var date = _period.PeriodStart;

            do
            {
                yield return date;
                date = _addFunc(date);
            }
            while (date <= _period.PeriodEnd);

            yield break;
        }
    }
}
