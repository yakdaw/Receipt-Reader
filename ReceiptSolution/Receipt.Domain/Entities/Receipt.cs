namespace Receipt.Domain.Entities
{
    using System;
    using System.Collections.Generic;

    public class Receipt
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime AddDate { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public string PurchasePlace { get; set; }
        public decimal ControlSum { get; set; }
        public byte[] Image { get; set; }

        public List<Product> Products { get; set; }
    }
}
