using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Quartz;
using AprioritWebCalendar.Business.Interfaces;

namespace AprioritWebCalendar.Web.Jobs
{
    public class InvitationsDeletingJob : IJob
    {
        private readonly IInvitationService _invitationService;
        private readonly ILogger _logger;

        public InvitationsDeletingJob(IInvitationService invitationService, ILoggerFactory loggerFactory)
        {
            _invitationService = invitationService;
            _logger = loggerFactory.CreateLogger("InvitationsDeletingJobLogger");
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("InvitationsDeletingJob is running.");
            var now = DateTime.UtcNow;

            try
            {
                var deleted = await _invitationService.RemoveOldInvitationsAsync(now);
                _logger.LogInformation($"Old invitations deleted: {deleted}. Time: {now}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unable to delete old invitations: {ex.Message}.");
            }
        }
    }
}
