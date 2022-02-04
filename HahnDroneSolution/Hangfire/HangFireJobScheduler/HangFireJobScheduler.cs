using Hangfire;
using HahnDroneAPI.Configurations;
using HahnDroneAPI.Services.Interfaces;
using System;

namespace HahnDroneAPI.HangFire.HangFireJobScheduler
{
    public class HangFireJobScheduler
    {
        public static void ScheduleRecurringJobs()
        {

            RecurringJob.RemoveIfExists(nameof(HangFireJob));
            RecurringJob.AddOrUpdate<HangFireJob>(nameof(HangFireJob),
                job => job.Run(JobCancellationToken.Null), Cron.Minutely(), TimeZoneInfo.Local);
        }
    }
}
