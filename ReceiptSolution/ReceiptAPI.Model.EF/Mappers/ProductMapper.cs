namespace Receipt.API.Model.EF.Mappers
{
    using Receipt.Domain.Entities;

    internal class ProductMapper
    {
        public static Product MapFrom(DatabaseModel.Products from)
        {
            if (from == null)
            {
                return null;
            }

            var to = new Product();
            to.ID = from.Id;
            to.Name = from.Name;
            to.Price = from.Price;
            to.Category = from.Category;
            to.PurchasePlace = from.PurchasePlace;
            to.PurchaseDate = from.PurchaseDate;
            to.AddDate = from.AddDate;

            return to;
        }
    }
}
