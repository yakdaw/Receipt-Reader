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

        public Product() { }

        //public Product(string name, string category, string purchasePlace, DateTime purchaseDate)
        //{
        //    this.Name = name;
        //    this.Category = category;
        //    this.PurchasePlace = purchasePlace;
        //    this.PurchaseDate = purchaseDate;
        //}
    }
}
