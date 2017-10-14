namespace Receipt.Web.ViewModels
{
    using Domain.Entities;

    public class ProductModel
    {
        public int Id { get; set; }
        public int ReceiptId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public Category Category { get; set; }

        public string Url { get; set; }
    }
}