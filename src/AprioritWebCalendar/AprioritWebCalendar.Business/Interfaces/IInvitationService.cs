using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AprioritWebCalendar.Business.DomainModels;

namespace AprioritWebCalendar.Business.Interfaces
{
    public interface IInvitationService
    {
        Task<Invitation> GetInvitationAsync(int eventId, int userId);
        Task<int> RemoveOldInvitationsAsync(DateTime dateTime);
    }
}
