using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using Xunit;
using AprioritWebCalendar.Business.Interfaces;
using AprioritWebCalendar.Data.Interfaces;
using AprioritWebCalendar.Data.Models;
using AprioritWebCalendar.Business.Services;
using AprioritWebCalendar.Bootstrap;
using AprioritWebCalendar.Infrastructure.Enums;

namespace AprioritWebCalendar.Test.Services
{
    public class NotificationServiceTest
    {
        private readonly IMapper _mapper;
        private readonly Mock<IRepository<EventCalendar>> _mock = new Mock<IRepository<EventCalendar>>();

        public NotificationServiceTest()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MapperProfile());
            });

            _mapper = config.CreateMapper();
        }

        private void SetupMock(Task<IQueryable<EventCalendar>> eventCalendars)
        {
            _mock.Setup(repo => repo.FindAllIncludingAsync(It.IsAny<Expression<Func<EventCalendar, object>>[]>()))
                .Returns(eventCalendars);
        }

        #region Tests.

        [Fact]
        public async Task GetEventsForNotify_1()
        {
            SetupMock(GetEventCalendars());
            
            var notificationService = new NotificationService(_mock.Object, _mapper);
            var dateTime = new DateTime(2016, 4, 21, 8, 40, 0);
            var expectedCount = 2;

            var events = await notificationService.GetEventsToNotifyAsync(dateTime);
            
            Assert.Equal(expectedCount, events.Count());
        }

        [Fact]
        public async Task GetEventsForNotify_2()
        {
            SetupMock(GetEventCalendars());

            var notificationService = new NotificationService(_mock.Object, _mapper);
            var dateTime = new DateTime(2017, 9, 20, 14, 25, 0);
            var expectedCount = 4;

            var events = await notificationService.GetEventsToNotifyAsync(dateTime);

            Assert.Equal(expectedCount, events.Count());
        }

        [Fact]
        public async Task GetEventsForNotify_3()
        {
            SetupMock(GetEventCalendars());

            var notificationService = new NotificationService(_mock.Object, _mapper);
            var dateTime = new DateTime(2015, 2, 2, 7, 10, 0);
            var expectedCount = 1;

            var events = await notificationService.GetEventsToNotifyAsync(dateTime);

            Assert.Equal(expectedCount, events.Count());
        }

        [Fact]
        public async Task GetEventsForNotify_4()
        {
            SetupMock(GetEventCalendars());

            var notificationService = new NotificationService(_mock.Object, _mapper);
            var dateTime = new DateTime(1997, 7, 13, 18, 10, 0);

            var events = await notificationService.GetEventsToNotifyAsync(dateTime);

            Assert.Null(events);
        }

        [Fact]
        public async Task GetEventsForNotify_Period_1()
        {
            SetupMock(GetEventCalendars());

            var notificationService = new NotificationService(_mock.Object, _mapper);
            var dateTime = new DateTime(2016, 2, 11, 7, 45, 0);
            var exprectedCount = 5;

            var events = await notificationService.GetEventsToNotifyAsync(dateTime);

            Assert.Equal(exprectedCount, events.Count());
        }

        [Fact]
        public async Task GetEventsForNotify_Period_2()
        {
            SetupMock(GetEventCalendars());

            var notificationService = new NotificationService(_mock.Object, _mapper);
            var dateTime = new DateTime(2017, 2, 11, 7, 45, 0);
            var exprectedCount = 5;

            var events = await notificationService.GetEventsToNotifyAsync(dateTime);

            Assert.Equal(exprectedCount, events.Count());
        }

        [Fact]
        public async Task GetEventsForNotify_Period_3()
        {
            SetupMock(GetEventCalendars());

            var notificationService = new NotificationService(_mock.Object, _mapper);
            var dateTime = new DateTime(2018, 4, 25, 15, 35, 0);
            var exprectedCount = 3;

            var events = await notificationService.GetEventsToNotifyAsync(dateTime);

            Assert.Equal(exprectedCount, events.Count());
        }

        #endregion

        public Task<IQueryable<EventCalendar>> GetEventCalendars()
        {
            return Task.Run(() =>
            {
                var list = new List<EventCalendar>
                {
                    new EventCalendar
                    {
                        Event = new Event
                        {
                            RemindBefore = 15,
                            StartDate = new DateTime(2016, 4, 21),
                            StartTime = new TimeSpan(8, 55, 0)
                        },
                        Calendar = new Calendar {
                            SharedUsers = new List<UserCalendar>
                            {
                                new UserCalendar { IsSubscribed = true, UserId = 2 }
                            },
                            OwnerId = 1
                        }
                    },

                    new EventCalendar
                    {
                        Event = new Event
                        {
                            RemindBefore = 30,
                            StartDate = new DateTime(2017, 9, 20),
                            StartTime = new TimeSpan(14, 55, 0)
                        },
                        Calendar = new Calendar
                        {
                            SharedUsers = new List<UserCalendar>
                            {
                                new UserCalendar { IsSubscribed = true },
                                new UserCalendar { IsSubscribed = true },
                                new UserCalendar { IsSubscribed = true },

                                new UserCalendar { IsSubscribed = false },
                                new UserCalendar { IsSubscribed = false },
                            }
                        }
                    },

                    new EventCalendar
                    {
                        Event = new Event
                        {
                            RemindBefore = 10,
                            StartDate = new DateTime(2015, 2, 2),
                            StartTime = new TimeSpan(7, 20, 0)
                        },
                        Calendar = new Calendar
                        {
                            SharedUsers = new List<UserCalendar>()
                        }
                    },

                    new EventCalendar
                    {
                        Event = new Event
                        {
                            StartDate = new DateTime(1997, 7, 13),
                            StartTime = new TimeSpan(18, 10, 0)
                        },
                        Calendar = new Calendar
                        {
                            SharedUsers = new List<UserCalendar>
                            {
                                new UserCalendar { IsSubscribed = true },
                                new UserCalendar { IsSubscribed = true },
                                new UserCalendar { IsSubscribed = true },
                                new UserCalendar { IsSubscribed = true },
                            }
                        }
                    },

                    new EventCalendar
                    {
                        Event = new Event
                        {
                            Period = new Period
                            {
                                PeriodStart = new DateTime(2016, 2, 11),
                                PeriodEnd = new DateTime(2018, 2, 11),
                                Type = PeriodType.Yearly
                            },
                            StartTime = new TimeSpan(8, 0, 0),
                            EndTime = new TimeSpan(8, 30, 0),
                            RemindBefore = 15
                        },
                        Calendar = new Calendar
                        {
                            SharedUsers = new List<UserCalendar>
                            {
                                new UserCalendar { IsSubscribed = true },
                                new UserCalendar { IsSubscribed = true },
                                new UserCalendar { IsSubscribed = true },
                                new UserCalendar { IsSubscribed = true },
                            }
                        }
                    },

                    new EventCalendar
                    {
                        Event = new Event
                        {
                            Period = new Period
                            {
                                PeriodStart = new DateTime(2016, 4, 25),
                                PeriodEnd = new DateTime(2018, 4, 25),
                                Type = PeriodType.Yearly
                            },
                            StartTime = new TimeSpan(15, 50, 0),
                            EndTime = new TimeSpan(16, 45, 0),
                            RemindBefore = 15
                        },
                        Calendar = new Calendar { }
                    },

                    new EventCalendar
                    {
                        Event = new Event
                        {
                            RemindBefore = 25,
                            StartDate = new DateTime(2018, 4, 25),
                            StartTime = new TimeSpan(16, 0, 0)
                        },
                        Calendar = new Calendar
                        {
                            SharedUsers = new List<UserCalendar>
                            {
                                new UserCalendar { IsSubscribed = true }
                            }
                        }
                    }
                };

                return list.AsQueryable();
            });
        }
    }
}
