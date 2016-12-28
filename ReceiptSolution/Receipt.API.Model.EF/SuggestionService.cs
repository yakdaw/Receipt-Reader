﻿namespace Receipt.API.Model.EF
{
    using Algorithms;
    using Model;
    using System.Linq;

    public class SuggestionService : ISuggestionService
    {
        public const double basicProductThreshold = 0.7;

        PairMetricAlgorithm algorithm = null;

        public SuggestionService()
        {
            algorithm = new PairMetricAlgorithm();
        }

        public int SuggestProductCategoryId(string userId, string productName, string purchasePlace)
        {
            var bestFitValue = -1.0;
            var bestFitCategory = -1;

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
                    }
                }
                
                if (bestFitValue >= basicProductThreshold)
                {
                    return bestFitCategory;
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
                    }
                }
            }

            return bestFitCategory;
        }
    }
}