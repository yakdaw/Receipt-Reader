using System;
using System.Collections.Generic;
using System.Linq;
namespace Receipt.Web.ViewModels
{
    public class PieChartModel
    {
        public string[] CategoryNames { get; set; }
        public decimal[] CategoryPrices { get; set; }

        public PieChartModel(string[] categoryNames, decimal[] categoryPrices)
        {
            this.CategoryNames = categoryNames;
            this.CategoryPrices = categoryPrices;
        }
    }
}