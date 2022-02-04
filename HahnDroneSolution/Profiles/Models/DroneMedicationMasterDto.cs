using HahnDroneAPI.Db.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HahnDroneAPI.Profiles.Models
{
    public class DroneMedicationMasterDto
    {
        public int DroneMedicationMasterID { get; set; }
        [Required]
        public int DroneID { get; set; }
        [Required]
        public int Status { get; set; }
        public string CapturedBy { get; set; } = "System";
        public DateTime CapturedDate { get; set; } = DateTime.Now;
        public virtual Drone Drone { get; set; }
        public virtual List<DroneMedicationDetail> DroneMedicationDetails { get; set; }
    }
}
