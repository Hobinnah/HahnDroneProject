using Hangfire;
using HahnDroneAPI.Services.Interfaces;
using System.Threading.Tasks;

namespace HahnDroneAPI.HangFire
{
    public class HangFireJob
    {
        private readonly IAuditEventLogService _auditEventLogService;
        public HangFireJob(IAuditEventLogService auditEventLogService)
        {
            _auditEventLogService = auditEventLogService;
        }

        public async Task Run(IJobCancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            await PeriodicAuditEventLog();            
        }

        public async Task PeriodicAuditEventLog()
        {
            await _auditEventLogService.CreateAuditEventLogAsync();
        }

    }

}
