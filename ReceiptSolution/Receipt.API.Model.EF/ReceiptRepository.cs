namespace Receipt.API.Model.EF
{
    using System.Collections.ObjectModel;
    using Domain.Entities;
    using System.Collections.Generic;
    using System.Linq;
    using Mappers;

    public class ReceiptRepository : IReceiptRepository
    {
        private ReceiptMapper receiptMapper = null;

        public ReceiptRepository()
        {
            receiptMapper = new ReceiptMapper();
        }

        public Collection<Receipt> GetAllUserReceipts(string userId)
        {
            List<DatabaseModel.Receipt> receipts;

            using (var db = new DatabaseModel.ReceiptReaderDatabaseContext())
            {
                receipts = db.Receipt.Where(r => r.UserId == userId).ToList();
            }

            var domainReceipts = new Collection<Receipt>();

            foreach (var receipt in receipts)
            {
                var domainReceipt = receiptMapper.MapFromDatabase(receipt);
                domainReceipts.Add(domainReceipt);
            }

            return domainReceipts;
        }

        public Receipt GetUserReceiptById(string userId, int receiptId)
        {
            DatabaseModel.Receipt receipt;

            using (var db = new DatabaseModel.ReceiptReaderDatabaseContext())
            {
                receipt = db.Receipt.FirstOrDefault(r => (r.UserId == userId) && (r.Id == receiptId));
            }

            var domainReceipt = receiptMapper.MapFromDatabase(receipt);

            return domainReceipt;
        }

        public byte[] GetUserReceiptImage(string userId, int receiptId)
        {
            byte[] imageBytes = null;

            using (var db = new DatabaseModel.ReceiptReaderDatabaseContext())
            {
                var receipt = db.Receipt.FirstOrDefault(r => (r.UserId == userId) && (r.Id == receiptId));

                if (receipt != null)
                {
                    imageBytes = receipt.Image;
                }
            }

            return imageBytes;
        }
    }
}
