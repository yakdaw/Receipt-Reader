namespace Receipt.API.Model.EF.Mappers
{
    using Domain.Entities;

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
            domainReceipt.UserId = databaseReceipt.UserId;
            domainReceipt.AddDate = databaseReceipt.AddDate;
            domainReceipt.PurchaseDate = databaseReceipt.PurchaseDate;
            databaseReceipt.PurchasePlace = databaseReceipt.PurchasePlace;
            domainReceipt.ControlSum = databaseReceipt.ControlSum;
            domainReceipt.Image = databaseReceipt.Image;
            
            // Product list
            // Url

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
            databaseReceipt.AddDate = domainReceipt.AddDate;
            databaseReceipt.PurchaseDate = domainReceipt.PurchaseDate;
            databaseReceipt.PurchasePlace = domainReceipt.PurchasePlace;
            databaseReceipt.ControlSum = domainReceipt.ControlSum;
            databaseReceipt.Image = domainReceipt.Image;

            return databaseReceipt;
        }
    }
}
