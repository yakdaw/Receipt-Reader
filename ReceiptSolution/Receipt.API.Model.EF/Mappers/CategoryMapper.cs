namespace Receipt.API.Model.EF.Mappers
{
    using Domain.Entities;

    class CategoryMapper
    {
        public Category MapFromDatabase(DatabaseModel.Category databaseCategory)
        {
            if (databaseCategory == null)
            {
                return null;
            }

            var domainCategory = new Category();
            domainCategory.Id = databaseCategory.Id;
            domainCategory.Name = databaseCategory.Name;

            return domainCategory;
        }

        public DatabaseModel.Category MapToDatabase(Category domainCategory)
        {
            if (domainCategory == null)
            {
                return null;
            }

            var databaseCategory = new DatabaseModel.Category();
            databaseCategory.Id = domainCategory.Id;
            databaseCategory.Name = domainCategory.Name;

            return databaseCategory;
        }
    }
}
