namespace Receipt.Web.Models
{
    using System.ComponentModel.DataAnnotations;

    public class LostPasswordModel
    {
        [Required]
        [Display(Name = "Username or e-mail")]
        public string LoginInfo { get; set; }
    }
}