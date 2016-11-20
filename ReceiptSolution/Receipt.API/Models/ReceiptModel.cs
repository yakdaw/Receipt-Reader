namespace Receipt.API.Models
{
    using Domain.Entities;
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

        public ReceiptModel(Receipt domainReceipt, string userName, string hostUrl)
        {
            this.Id = domainReceipt.Id;
            this.AddDate = domainReceipt.AddDate;
            this.PurchaseDate = domainReceipt.PurchaseDate;
            this.PurchasePlace = domainReceipt.PurchasePlace;
            this.ControlSum = domainReceipt.ControlSum;

            this.Url = hostUrl + "/api/" + userName + "/receipts/" + Id;
            this.ImageUrl = Url + "/image";
        }
    }
}