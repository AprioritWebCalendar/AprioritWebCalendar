using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using AprioritWebCalendar.Business.DomainModels;
using AprioritWebCalendar.Business.Recurrences;
using AprioritWebCalendar.Infrastructure.Enums;

namespace AprioritWebCalendar.Test
{
    public class RecurrenceCalculatorTest
    {
        [Fact]
        public void RecurrenceCalculator_Year_1()
        {
            var ev = new Event
            {
                Period = new Period
                {
                    PeriodStart = new DateTime(2006, 4, 30),
                    PeriodEnd = new DateTime(2011, 11, 26),
                    Type = PeriodType.Yearly
                }
            };

            var expected = 6;
            var calculator = new RecurrenceCalculator(ev);

            var events = calculator.GetRecurrences();

            Assert.Equal(expected, events.Count());
        }

        [Fact]
        public void RecurrenceCalculator_Year_2()
        {
            var ev = new Event
            {
                Period = new Period
                {
                    PeriodStart = new DateTime(2009, 4, 19),
                    PeriodEnd = new DateTime(2013, 11, 1),
                    Type = PeriodType.Yearly
                }
            };

            var expected = 5;
            var calculator = new RecurrenceCalculator(ev);

            var events = calculator.GetRecurrences();

            Assert.Equal(expected, events.Count());
        }

        [Fact]
        public void RecurrenceCalculator_Month_1()
        {
            var ev = new Event
            {
                Period = new Period
                {
                    PeriodStart = new DateTime(2016, 2, 11),
                    PeriodEnd = new DateTime(2017, 5, 11),
                    Type = PeriodType.Monthly
                }
            };

            var expected = 16;
            var calculator = new RecurrenceCalculator(ev);

            var events = calculator.GetRecurrences();

            Assert.Equal(expected, events.Count());
        }

        [Fact]
        public void RecurrenceCalculator_Month_2()
        {
            var ev = new Event
            {
                Period = new Period
                {
                    PeriodStart = new DateTime(2017, 9, 20),
                    PeriodEnd = new DateTime(2018, 4, 20),
                    Type = PeriodType.Monthly
                }
            };

            var expected = 8;
            var calculator = new RecurrenceCalculator(ev);

            var events = calculator.GetRecurrences();

            Assert.Equal(expected, events.Count());
        }

        [Fact]
        public void RecurrenceCalculator_Week_1()
        {
            var ev = new Event
            {
                Period = new Period
                {
                    PeriodStart = new DateTime(2018, 2, 12),
                    PeriodEnd = new DateTime(2018, 4, 29),
                    Type = PeriodType.Weekly
                }
            };

            var expected = 11;
            var calculator = new RecurrenceCalculator(ev);

            var events = calculator.GetRecurrences();

            Assert.Equal(expected, events.Count());
        }

        [Fact]
        public void RecurrenceCalculator_Custom_1()
        {
            var ev = new Event
            {
                Period = new Period
                {
                    PeriodStart = new DateTime(2015, 12, 18),
                    PeriodEnd = new DateTime(2016, 2, 8),
                    Type = PeriodType.Custom,
                    Cycle = 1
                }
            };

            var expected = 53;
            var calculator = new RecurrenceCalculator(ev);

            var events = calculator.GetRecurrences();

            Assert.Equal(expected, events.Count());
        }
    }
}
