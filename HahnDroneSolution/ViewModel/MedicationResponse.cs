using HahnDroneAPI.Profiles.Models;
using System.Collections.Generic;

namespace HahnDroneAPI.ViewModel
{
    public class MedicationResponse
    {

        public IEnumerable<MedicationDto> Medications { get; set; }
        public int Count { get; set; }
    }
}
