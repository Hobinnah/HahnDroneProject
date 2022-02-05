using System.ComponentModel.DataAnnotations;

namespace HahnDroneAPI.Db.Entities
{
    public class Model
    {
        [Key]
        public int ModelID { get; set; }
        [StringLength(20, MinimumLength = 2)]
        [Required]
        public string Description { get; set; }
    }
}
