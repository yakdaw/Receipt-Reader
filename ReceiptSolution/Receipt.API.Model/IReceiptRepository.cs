﻿namespace Receipt.API.Model
{
    using Domain.Entities;
    using System.Collections.ObjectModel;

    public interface IReceiptRepository
    {
        Collection<Receipt> GetAllUserReceipts(string userId);
        Receipt GetUserReceiptById(string userId, int receiptId);
        byte[] GetUserReceiptImage(string userId, int receiptId);
        Collection<Category> GetUserReceiptCategories(string userId, int receiptId);

        void Add(string userId, Receipt receipt);
        void AddEmpty(string userId);
        void Update(string userId, int receiptId, Receipt updatedReceipt);
        void Delete(string userId, int receiptId);
    }
}
