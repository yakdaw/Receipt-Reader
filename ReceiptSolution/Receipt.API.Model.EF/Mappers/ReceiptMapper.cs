namespace Receipt.API.Model.EF.Mappers
{
    using Domain.Entities;
    using System;

    class ReceiptMapper
    {
        public Receipt MapFromDatabase(DatabaseModel.Receipt databaseReceipt)
        {
            if (databaseReceipt == null)
            {
                return null;
            }

            var domainReceipt = new Receipt();
            domainReceipt.Id = databaseReceipt.Id;
            domainReceipt.AddDate = databaseReceipt.AddDate;
            domainReceipt.PurchaseDate = databaseReceipt.PurchaseDate;
            domainReceipt.PurchasePlace = databaseReceipt.PurchasePlace;
            domainReceipt.ControlSum = databaseReceipt.ControlSum;
            domainReceipt.Image = databaseReceipt.Image;

            return domainReceipt;
        }

        public DatabaseModel.Receipt MapToDatabase(Receipt domainReceipt)
        {
            if (domainReceipt == null)
            {
                return null;
            }

            var databaseReceipt = new DatabaseModel.Receipt();
            databaseReceipt.Id = domainReceipt.Id;
            databaseReceipt.UserId = domainReceipt.UserId;
            databaseReceipt.AddDate = DateTime.Now;
            databaseReceipt.PurchaseDate = domainReceipt.PurchaseDate;
            databaseReceipt.PurchasePlace = domainReceipt.PurchasePlace;
            databaseReceipt.ControlSum = domainReceipt.ControlSum;
            databaseReceipt.Image = domainReceipt.Image;

            return databaseReceipt;
        }
    }
}
