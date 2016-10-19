namespace Receipt.Domain.Entities
{
    using System;

    public class Product
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string purchasePlace { get; set; }
        public DateTime purchaseDate { get; set; }
        public DateTime addDate { get; set; }
   
        public Product(string name, string category, string purchasePlace, DateTime purchaseDate)
        {
            this.Name = name;
            this.Category = category;
            this.purchasePlace = purchasePlace;
            this.purchaseDate = purchaseDate;
        }
    }
}
