using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AprioritWebCalendar.Business.Interfaces
{
    public interface IInvitationService
    {
        Task<int> RemoveOldInvitationsAsync(DateTime dateTime);
    }
}
