using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HahnDroneAPI.Models
{
    public class DroneMedicationRequest
    {
        public int DroneID { get; set; }
        public IEnumerable<int> MedicationIDs { get; set; }
        public string CapturedBy { get; set; } = "System";
        public DateTime? CapturedDate { get; set; } = DateTime.Now.Date;
    }
}
