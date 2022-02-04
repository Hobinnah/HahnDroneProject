using HahnDroneAPI.Db.Entities;
using HahnDroneAPI.Db.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HahnDroneAPI.Db.Repositories.Implementations
{
    public class AuditEventLogRepository : Repository<AuditEventLog>, IAuditEventLogRepository
    {

        public AuditEventLogRepository(HahnDroneDBContext context) : base(context)
        {

        }

        public IQueryable<AuditEventLog> AuditEventLogs => _context.AuditEventLogs.OrderByDescending(x => x.AuditEventLogID).AsQueryable();

    }
}
