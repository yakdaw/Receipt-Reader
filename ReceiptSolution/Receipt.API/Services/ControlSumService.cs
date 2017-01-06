namespace Receipt.API.Services
{
    using Models;
    using System.Linq;

    public class ControlSumService
    {
        public bool ValidateReceiptControlSum(NewReceiptModel receipt)
        {
            var productsPrice = receipt.Products.Sum(p => p.Price * p.Quantity);
            
            if (receipt.ControlSum == productsPrice)
            {
                return true;
            }

            return false;
        }
    }
}