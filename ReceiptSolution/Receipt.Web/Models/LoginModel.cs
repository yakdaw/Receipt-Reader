namespace Receipt.Web.Models
{
    using System.ComponentModel.DataAnnotations;

    public class LoginModel
    {
        [Required(ErrorMessage = "Enter username")]
        [Display(Name = "Login")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Enter password")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}