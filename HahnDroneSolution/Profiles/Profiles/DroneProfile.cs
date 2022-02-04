using HahnDroneAPI.Db.Entities;
using HahnDroneAPI.Profiles.Models;

namespace HahnDroneAPI.Profiles.Profiles
{
    public class DroneProfile : AutoMapper.Profile
    {
        public DroneProfile()
        {
            CreateMap<Drone, DroneDto>().ReverseMap();
            CreateMap<Model, ModelDto>().ReverseMap();
        }
    
    }
}
