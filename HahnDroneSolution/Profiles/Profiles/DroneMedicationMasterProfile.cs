using HahnDroneAPI.Db.Entities;
using HahnDroneAPI.Profiles.Models;

namespace HahnDroneAPI.Profiles.Profiles
{
    public class DroneMedicationMasterProfile : AutoMapper.Profile
    {
        public DroneMedicationMasterProfile()
        {
            CreateMap<DroneMedicationMaster, DroneMedicationMasterDto>().ReverseMap();
            CreateMap<DroneMedicationDetail, DroneMedicationDetailDto>().ReverseMap();
            CreateMap<Medication, MedicationDto>().ReverseMap();
        }
    }
}
