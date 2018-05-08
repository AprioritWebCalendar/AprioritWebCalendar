using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using AprioritWebCalendar.Business.Interfaces;
using AprioritWebCalendar.Data.Interfaces;
using AprioritWebCalendar.Data.Models;
using DomainInvitation = AprioritWebCalendar.Business.DomainModels.Invitation;

namespace AprioritWebCalendar.Business.Services
{
    public class InvitationService : IInvitationService
    {
        private readonly IRepository<Invitation> _invitationRepository;
        private readonly IMapper _mapper;

        public InvitationService(IRepository<Invitation> invitationRepository, IMapper mapper)
        {
            _invitationRepository = invitationRepository;
            _mapper = mapper;
        }

        public async Task<DomainInvitation> GetInvitationAsync(int eventId, int userId)
        {
            var invitation = (await _invitationRepository.FindAllIncludingAsync(i => i.EventId == eventId && i.UserId == userId,
                    i => i.Event, i => i.Event.Owner, i => i.Event.Period, i => i.Invitator))
                .FirstOrDefault();

            return _mapper.Map<DomainInvitation>(invitation);
        }

        public async Task<int> RemoveOldInvitationsAsync(DateTime dateTime)
        {
            Expression<Func<Invitation, bool>> filter = i => (i.Event.Period == null && i.Event.IsAllDay && dateTime >= i.Event.EndDate.Value.AddDays(1))
                || (i.Event.Period == null && !i.Event.IsAllDay && dateTime >= i.Event.EndDate.Value.AddMinutes(i.Event.EndTime.Value.TotalMinutes))
                || (i.Event.Period != null && i.Event.IsAllDay && dateTime >= i.Event.Period.PeriodEnd.AddDays(1))
                || (i.Event.Period != null && !i.Event.IsAllDay && dateTime >= i.Event.Period.PeriodEnd.AddMinutes(i.Event.EndTime.Value.TotalMinutes));


            var invitations = (await _invitationRepository.FindAllIncludingAsync(i => i.Event, i => i.Event.Period))
                .Where(filter)
                .ToList();

            var count = invitations?.Count;

            if (count > 0)
            {
                await _invitationRepository.RemoveRangeAsync(invitations);
                await _invitationRepository.SaveAsync();
            }

            return count ?? 0;
        }
    }
}
