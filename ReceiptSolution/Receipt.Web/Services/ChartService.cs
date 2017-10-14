namespace Receipt.Web.Services
{
    using Models;
    using System.Collections.Generic;
    using System.Linq;
    using ViewModels;

    public class ChartService
    {
        static string[] Categories = new string[]
        {
            "Alkohole i tytoń",
            "Apteczka",
            "Art. biurowe",
            "Art dla zwierząt",
            "Art domowe",
            "Art dziecięce",
            "Art. sypkie",
            "Chemia gospodarcza",
            "Ciasta, desery, dodatki",
            "Dania gotowe",
            "Higiena",
            "Inne",
            "Kawa, herbata, kakao",
            "Konserwy",
            "Kosmetyki",
            "Mięso i wędliny",
            "Motoryzacja",
            "Mrożonki i lody",
            "Nabiał",
            "Pieczywo",
            "Prasa, książki, płyty",
            "Przetwory",
            "Przyprawy, sosy, dodatki",
            "Ryby",
            "Sprzęt AGD i RTV",
            "Słodycze i przekąski",
            "Tłuszcze",
            "Ubrania",
            "Warzywa i owoce",
            "Woda i napoje",
            "Żywność dietetyczna"
        };

        public PieChartModel GenerateChartModelFromProductsList(List<ProductModel> responseProducts)
        {
            var categoryPrices = new List<CategoryPriceModel>();

            for (int i = 0; i < Categories.Length; i++)
            {
                var categoryPrice = responseProducts
                    .Where(p => p.Category.Id == (i + 1))
                    .Sum(p => p.Price * p.Quantity);

                if (categoryPrice != 0.0m)
                {
                    categoryPrices.Add(new CategoryPriceModel(Categories[i], categoryPrice));
                }
            }

            var categoryArray = categoryPrices.Select(cp => cp.Category).ToArray();
            var priceArray = categoryPrices.Select(cp => cp.Price).ToArray();

            return new PieChartModel(categoryArray, priceArray);
        }
    }
}