using HahnDroneAPI.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HahnDroneAPI.Db.Entities
{
    public class DroneMedicationMaster
    {
        [Key]
        public int DroneMedicationMasterID { get; set; }
        [Required]
        public int DroneID { get; set; }
        [Required]
        public StateEnum Status { get; set; }
        public string CapturedBy { get; set; } = "System";
        public DateTime CapturedDate { get; set; } = DateTime.Now.Date;
        public virtual Drone Drone { get; set; }
        public virtual List<DroneMedicationDetail> DroneMedicationDetails { get; set; }
    }

}
