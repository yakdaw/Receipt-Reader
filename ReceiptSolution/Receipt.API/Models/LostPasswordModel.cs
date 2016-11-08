using System.ComponentModel.DataAnnotations;

namespace Receipt.API.Models
{
    public class LostPasswordModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }
    }
}