using HahnDroneAPI.Db.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HahnDroneAPI.Profiles.Models
{
    public class DroneMedicationDetailDto
    {
        public int DroneMedicationDetailID { get; set; }
        [Required]
        public int DroneMedicationMasterID { get; set; }
        [Required]
        public int MedicationID { get; set; }
        public virtual Medication Medication { get; set; }
    }
}
