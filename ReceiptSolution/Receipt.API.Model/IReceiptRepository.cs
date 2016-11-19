﻿namespace Receipt.API.Model
{
    using Domain.Entities;
    using System.Collections.ObjectModel;

    public interface IReceiptRepository
    {
        Collection<Receipt> GetAllUserReceipts(string userId);
        Receipt GetUserReceiptById(string userId, int receiptId);
        byte[] GetUserReceiptImage(string userId, int receiptId);
    }
}
