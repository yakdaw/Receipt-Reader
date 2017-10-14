namespace Receipt.API.Model
{
    using Domain.Entities;
    using System.Collections.ObjectModel;

    public interface IProductRepository
    {
        Collection<Product> GetAllUserProducts(string userId);
        Collection<Product> GetUserProductsByReceipt(string userId, int receiptId);
        Collection<Product> GetUserProductsByReceiptCategory(string userId, int receiptId, int categoryId);
        Product GetUserProductById(string userId, int receiptId, int productId);

        void Add(string userId, int receiptId, Product product);
        void Update(string userId, int receiptId, int productId, Product updatedProduct);
        void Delete(string userId, int receiptId, int productId);
    }
}
