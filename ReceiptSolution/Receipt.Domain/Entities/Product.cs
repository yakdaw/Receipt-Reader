namespace Receipt.Domain.Entities
{
    using System;

    public class Product
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        public string PurchasePlace { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public DateTime AddDate { get; set; }
    }
}
