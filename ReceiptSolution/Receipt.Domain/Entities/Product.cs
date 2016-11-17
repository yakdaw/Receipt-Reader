namespace Receipt.Domain.Entities
{
    using System;

    public class Product
    {
        public int Id { get; set; }
        public int ReceiptId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Category { get; set; }

        public string Url { get; set; }

        public void GenerateApiUrlForProduct()
        {
            throw new NotImplementedException();
        }
    }
}
