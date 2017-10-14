namespace Receipt.Web.Models
{
    public class CategoryPriceModel
    {
        public string Category { get; set; }
        public decimal Price { get; set; }

        public CategoryPriceModel(string category, decimal price)
        {
            this.Category = category;
            this.Price = price;
        }
    }
}