namespace Receipt.Web.ViewModels
{
    using System;

    public class ReceiptModel
    {
        public int Id { get; set; }
        public DateTime AddDate { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public string PurchasePlace { get; set; }
        public decimal ControlSum { get; set; }

        public string Url { get; set; }
        public string ImageUrl { get; set; }
    }
}