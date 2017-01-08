namespace Receipt.Web.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using ViewModels;

    public class ReceiptUpdateModel
    {
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [Display(Name = "Purchase date")]
        public DateTime? PurchaseDate { get; set; }

        [Display(Name = "Purchase place")]
        public string PurchasePlace { get; set; }

        public void MapFromApiModel(ReceiptModel apiModel)
        {
            this.PurchaseDate = apiModel.PurchaseDate;
            this.PurchasePlace = apiModel.PurchasePlace;
        }
    }
}