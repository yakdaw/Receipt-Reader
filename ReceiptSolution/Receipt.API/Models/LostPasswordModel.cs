using System.ComponentModel.DataAnnotations;

namespace Receipt.API.Models
{
    public class LostPasswordModel
    {
        [Required]
        [Display(Name = "User name or email")]
        public string LoginInfo { get; set; }
    }
}