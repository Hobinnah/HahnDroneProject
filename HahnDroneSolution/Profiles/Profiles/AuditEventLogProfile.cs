using HahnDroneAPI.Db.Entities;
using HahnDroneAPI.Profiles.Models;

namespace HahnDroneAPI.Profiles.Profiles
{
    public class AuditEventLogProfile : AutoMapper.Profile
    {
        public AuditEventLogProfile()
        {
            CreateMap<AuditEventLog, AuditEventLogDto>().ReverseMap();
        }
    }
}
