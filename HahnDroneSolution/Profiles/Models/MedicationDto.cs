
using System.ComponentModel.DataAnnotations;

namespace HahnDroneAPI.Profiles.Models
{
    public class MedicationDto
    {
        public int MedicationID { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9_-]+$", ErrorMessage = "Allows only letters, numbers, dash (-) and underscore (_)")]
        public string Name { get; set; }
        public decimal Weight { get; set; }
        [Required]
        [RegularExpression(@"^[A-Z0-9_]+$", ErrorMessage = "Allows only upper case letters (A-Z), underscore (_) and numbers (0-9)")]
        public string Code { get; set; }
        public string Image { get; set; }
    }
}
