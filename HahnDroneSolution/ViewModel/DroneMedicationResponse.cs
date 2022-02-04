using HahnDroneAPI.Profiles.Models;
using System.Collections.Generic;

namespace HahnDroneAPI.ViewModel
{
    public class DroneMedicationResponse
    {
        public IEnumerable<MedicationDto> Medications { get; set; }
        public int Count { get; set; }
    }
}
