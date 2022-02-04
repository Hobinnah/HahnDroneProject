using System.ComponentModel.DataAnnotations;

namespace HahnDroneAPI.Db.Entities
{
    public class DroneMedicationDetail
    {
        [Key]
        public int DroneMedicationDetailID { get; set; }
        [Required]
        public int DroneMedicationMasterID { get; set; }
        [Required]
        public int MedicationID { get; set; }
        public virtual Medication Medication { get; set; }
    }
}
