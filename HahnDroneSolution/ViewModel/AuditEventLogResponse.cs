using HahnDroneAPI.Profiles.Models;
using System.Collections.Generic;

namespace HahnDroneAPI.ViewModel
{
    public class AuditEventLogResponse
    {
        public IEnumerable<AuditEventLogDto> AuditEventLogs { get; set; }
        public int Count { get; set; }
    }
}
