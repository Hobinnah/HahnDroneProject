using HahnDroneAPI.Models;
using HahnDroneAPI.Profiles.Models;
using HahnDroneAPI.ViewModel;
using System.Threading.Tasks;

namespace HahnDroneAPI.Services.Interfaces
{
    public interface IAuditEventLogService
    {
        Task<AuditEventLogResponse> GetAuditEventLogsAsync(QueryParameters queryParameters);
        Task CreateAuditEventLogAsync();
        Task<AuditEventLogDto> DeleteAuditEventLogAsync(int auditEventLogID);
    }
}
