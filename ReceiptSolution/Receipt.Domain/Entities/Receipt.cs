namespace Receipt.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public class Receipt
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime AddDate { get; set; }
        public DateTime? PurchaseDate { get; set; }
        // Temporary nullable controlSum
        public decimal? ControlSum { get; set; }
        public byte[] Image { get; set; }

        public List<Product> Products { get; set; }

        public string Url { get; set; }

        public Receipt()
        {
            Products = new List<Product>();
        }

        public void GenerateApiUrlForReceipt()
        {
            throw new NotImplementedException();
        }
    }
}
