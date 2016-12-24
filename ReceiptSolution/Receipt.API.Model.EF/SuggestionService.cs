namespace Receipt.API.Model.EF
{
    using Algorithms;
    using Model;
    using System.Linq;

    public class SuggestionService : ISuggestionService
    {
        PairMetricAlgorithm algorithm = null;

        public SuggestionService()
        {
            algorithm = new PairMetricAlgorithm();
        }

        public int SuggestProductCategoryId(string productName)
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
            }

            return bestFitCategory;
        }
    }
}
