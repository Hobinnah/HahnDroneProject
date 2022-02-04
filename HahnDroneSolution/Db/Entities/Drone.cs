using HahnDroneAPI.Models.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace HahnDroneAPI.Db.Entities
{
    public class Drone
    {
        [Key]
        public int DroneID { get; set; }
        public int ModelID { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Serial number length can not be more than 100.")]
        public string SerialNumber { get; set; }
        [Required]
        [Range(1, 500, ErrorMessage = "Drone weight can only be between 1 and 500.")]
        public decimal Weight { get; set; }
        [Required]
        [Range(0, 100, ErrorMessage = "Drone's battery capacity can only be between 0 and 100.")]
        public int BatteryCapacity { get; set; }
        public StateEnum State { get; set; }
        public virtual Model Model { get; set; }
    }

   

    //public virtual ModelDto Model { get; set; }
}
