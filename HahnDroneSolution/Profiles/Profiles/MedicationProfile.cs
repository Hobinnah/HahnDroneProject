using HahnDroneAPI.Db.Entities;
using HahnDroneAPI.Profiles.Models;


namespace HahnDroneAPI.Profiles.Profiles
{
    public class MedicationProfile : AutoMapper.Profile
    {
        public MedicationProfile()
        {
            CreateMap<Medication, MedicationDto>().ReverseMap();
        }
    }
}
