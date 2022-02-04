using System;
using System.ComponentModel.DataAnnotations;


namespace HahnDroneAPI.Profiles.Models
{
    public class AuditEventLogDto
    {
        public int AuditEventLogID { get; set; }
        [Required]
        public int DroneID { get; set; }
        [Required]
        public decimal BatteryCapacity { get; set; }
        public string Message { get; set; }
        public DateTime AuditDate { get; set; } = DateTime.Now;
    }
}
