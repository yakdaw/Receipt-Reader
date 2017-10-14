namespace Receipt.API.Models
{
    using Domain.Entities;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class NewReceiptModel
    {
        [Required]
        [CannotBeEmpty]
        [Display(Name = "Products in receipt")]
        public List<NewProductModel> Products { get; set; }

        [Required]
        [Display(Name = "Controlsum")]
        public decimal ControlSum { get; set; }

        [Display(Name = "Purchase date")]
        public DateTime? PurchaseDate { get; set; }

        [Display(Name = "Purchase place")]
        public string PurchasePlace { get; set; }

        [Display(Name = "Receipt image")]
        public byte[] Image { get; set; }

        public Receipt MapToDomainReceipt()
        {
            var domainReceipt = new Receipt();
            var domainProducts = new List<Product>();

            foreach (NewProductModel p in this.Products)
            {
                var domainProduct = p.MapToDomainProduct();
                domainProducts.Add(domainProduct);
            }

            domainReceipt.Products = domainProducts;
            domainReceipt.ControlSum = this.ControlSum;
            domainReceipt.PurchaseDate = this.PurchaseDate;
            domainReceipt.PurchasePlace = this.PurchasePlace;
            domainReceipt.Image = this.Image;

            return domainReceipt;
        }
    }
}