using HahnDroneAPI.Profiles.Models;
using System.Collections.Generic;

namespace HahnDroneAPI.ViewModel
{
    public class DroneResponse
    {
        public IEnumerable<DroneDto> Drones { get; set; }
        public int Count { get; set; }
    }
}
