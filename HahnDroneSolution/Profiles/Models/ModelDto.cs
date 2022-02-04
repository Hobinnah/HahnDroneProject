using System.ComponentModel.DataAnnotations;

namespace HahnDroneAPI.Profiles.Models
{
    public class ModelDto
    {
        public int ModelID { get; set; }
        [StringLength(20, MinimumLength = 2)]
        public string Description { get; set; }
    }
}
