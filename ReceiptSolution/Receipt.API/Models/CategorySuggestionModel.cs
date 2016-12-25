namespace Receipt.API.Models
{
    using System.ComponentModel.DataAnnotations;

    public class CategorySuggestionModel
    {
        [Required]
        [Display(Name = "Product name")]
        public string ProductName { get; set; }

        [Display(Name = "Purchase place")]
        public string PurchasePlace { get; set; }
    }
}