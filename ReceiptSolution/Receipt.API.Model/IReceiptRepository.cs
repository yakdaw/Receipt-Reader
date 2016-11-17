namespace Receipt.API.Model
{
    using Receipt.Domain.Entities;
    using System.Collections.ObjectModel;

    public interface IReceiptRepository
    {
        Collection<Receipt> GetAllUserReceipts(string userId);
    }
}
