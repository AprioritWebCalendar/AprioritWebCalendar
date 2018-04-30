using System;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;

namespace AprioritWebCalendar.Web.Jobs
{
    public static class JobStarter
    {
        private static IScheduler _scheduler;

        public async static void RegisterJobs(IServiceProvider container)
        {
            _scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            _scheduler.JobFactory = new JobFactory(container);
            await _scheduler.Start();

            await StartNotificationJob();
        }

        private async static Task StartNotificationJob()
        {
            var job = JobBuilder.Create<NotificationJob>().Build();

            var trigger = TriggerBuilder.Create()
                .WithIdentity("NotificationTrigger", "NotificationGroup")
                .StartNow()
                .WithSimpleSchedule(x => x.WithIntervalInMinutes(1).RepeatForever())
                .Build();

            await _scheduler.ScheduleJob(job, trigger);
        }
    }
}
