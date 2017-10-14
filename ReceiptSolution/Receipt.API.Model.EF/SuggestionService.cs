namespace Receipt.API.Model.EF
{
    using Algorithms;
    using Domain.Models;
    using Model;
    using System.Linq;

    public class SuggestionService : ISuggestionService
    {
        public const double BasicProductThreshold = 0.8;
        public const double OtherCategoryThreshold = 0.4;
        public const int OtherCategoryId = 12;

        PairMetricAlgorithm algorithm = null;

        public SuggestionService()
        {
            algorithm = new PairMetricAlgorithm();
        }

        public ProductWithCategory SuggestProductCategoryId(string userId, string productName, string purchasePlace)
        {
            var bestFitValue = -1.0;
            var bestFitCategory = -1;
            string bestFitProductName = null;

            using (var db = new DatabaseModel.ReceiptReaderDatabaseContext())
            {
                var basicProducs = db.BasicProduct.ToList();

                foreach (var basicProduct in basicProducs)
                {
                    var nameFit = algorithm.CompareStrings(productName, basicProduct.Name);

                    if (nameFit > bestFitValue)
                    {
                        bestFitValue = nameFit;
                        bestFitCategory = basicProduct.CategoryId;
                        bestFitProductName = basicProduct.Name;
                    }
                }
                
                if (bestFitValue >= BasicProductThreshold)
                {
                    return new ProductWithCategory()
                    { ProductName = bestFitProductName, CategoryId = bestFitCategory };
                }
                
                var customizedProducts = db.CustomizedProduct.Where(cp => cp.UserId == userId).ToList();

                if (purchasePlace != null)
                {
                    customizedProducts = customizedProducts.Where(cp => cp.PurchasePlace == purchasePlace).ToList();
                }

                foreach (var customizedProduct in customizedProducts)
                {
                    var nameFit = algorithm.CompareStrings(productName, customizedProduct.Name);

                    if (nameFit > bestFitValue)
                    {
                        bestFitValue = nameFit;
                        bestFitCategory = customizedProduct.CategoryId;
                        bestFitProductName = customizedProduct.Name;
                    }
                }
            }

            if (bestFitValue < OtherCategoryThreshold)
            {
                return new ProductWithCategory()
                { ProductName = null, CategoryId = OtherCategoryId };
            }

            return new ProductWithCategory()
            { ProductName = bestFitProductName, CategoryId = bestFitCategory };
        }
    }
}
