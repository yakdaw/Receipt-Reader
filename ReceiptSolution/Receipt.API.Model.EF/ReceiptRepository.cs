namespace Receipt.API.Model.EF
{
    using System;
    using System.Collections.ObjectModel;
    using Domain.Entities;

    public class ReceiptRepository : IReceiptRepository
    {
        public Collection<Receipt> GetAllUserReceipts(string userId)
        {
            throw new NotImplementedException();
        }

        public Receipt GetUserReceiptById(string userId, int receiptId)
        {
            throw new NotImplementedException();
        }
    }
}
