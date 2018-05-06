using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using Xunit;
using AprioritWebCalendar.Bootstrap;
using AprioritWebCalendar.Business.Services;
using AprioritWebCalendar.Data.Interfaces;
using AprioritWebCalendar.Data.Models;

namespace AprioritWebCalendar.Test.Services
{
    public class InvitationServiceTest
    {
        private readonly IMapper _mapper;
        private readonly InvitationService _invitationService;

        public InvitationServiceTest()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MapperProfile());
            });

            var mockInvitationRepository = new Mock<IRepository<Invitation>>();

            mockInvitationRepository.Setup(repo => repo.FindAllIncludingAsync(It.IsAny<Expression<Func<Invitation, object>>[]>()))
                .Returns(_GetTestInvitations());

            _invitationService = new InvitationService(mockInvitationRepository.Object, config.CreateMapper());
        }

        [Fact]
        public async void RemoveOldInvitationsSuccess_1()
        {
            var dateTime = new DateTime(2016, 4, 21, 8, 55, 0);
            var expected = 3;

            var actual = await _invitationService.RemoveOldInvitationsAsync(dateTime);

            Assert.Equal(expected, actual);
        }

        private Task<IQueryable<Invitation>> _GetTestInvitations()
        {
            return Task.Run(() =>
            {
                var list = new List<Invitation>
                {
                    new Invitation
                    {
                        Event = new Event
                        {
                            EndDate = new DateTime(2016, 4, 21),
                            EndTime = new TimeSpan(8, 55, 0)
                        }
                    },

                    new Invitation
                    {
                        Event = new Event
                        {
                            IsAllDay = true,
                            EndDate = new DateTime(2017, 11, 30)
                        }
                    },

                    new Invitation
                    {
                        Event = new Event
                        {
                            IsAllDay = true,
                            Period = new Period
                            {
                                PeriodEnd = new DateTime(2011, 11, 26)
                            }
                        }
                    },

                    new Invitation
                    {
                        Event = new Event
                        {
                            EndTime = new TimeSpan(12, 0, 0),
                            Period = new Period
                            {
                                PeriodEnd = new DateTime(2013, 11, 1)
                            }
                        }
                    }
                };

                return list.AsQueryable();
            });
        }
    }
}
