using HahnDroneAPI.Db.Entities;
using HahnDroneAPI.Profiles.Models;

namespace HahnDroneAPI.Profiles.Profiles
{
    public class ModelProfile : AutoMapper.Profile
    {
        public ModelProfile()
        {
            CreateMap<Model, ModelDto>().ReverseMap();
        }
    }
}
