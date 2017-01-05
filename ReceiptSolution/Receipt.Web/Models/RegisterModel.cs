namespace Receipt.Web.Models
{
    using System.ComponentModel.DataAnnotations;

    public class RegisterModel
    {
        [Required(ErrorMessage = "Username is required")]
        [Display(Name = "Username")]
        public string Name { get; set; }

        [Required(ErrorMessage = "E-mail is required")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Enter password")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}