namespace Receipt.API.Models
{
    using Domain.Entities;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class UpdatedReceiptModel
    {
        [Display(Name = "Purchase date")]
        public DateTime? PurchaseDate { get; set; }

        [Display(Name = "Purchase place")]
        public string PurchasePlace { get; set; }

        [Display(Name = "Receipt image")]
        public byte[] Image { get; set; }

        public Receipt MapToDomainReceipt()
        {
            var domainReceipt = new Receipt();

            domainReceipt.PurchaseDate = this.PurchaseDate;
            domainReceipt.PurchasePlace = this.PurchasePlace;
            domainReceipt.Image = this.Image;

            return domainReceipt;
        }
    }
}