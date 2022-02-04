using HahnDroneAPI.Db.Entities;
using HahnDroneAPI.Profiles.Models;

namespace HahnDroneAPI.Profiles.Profiles
{
    public class DroneMedicationDetailProfile : AutoMapper.Profile
    {
        public DroneMedicationDetailProfile()
        {
            CreateMap<DroneMedicationDetail, DroneMedicationDetailDto>().ReverseMap();
        }
    }
}
