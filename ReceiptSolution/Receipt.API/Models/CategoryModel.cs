
namespace Receipt.API.Models
{
    using Domain.Entities;

    public class CategoryModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }

        public CategoryModel(Category domainCategory, string userName, int receiptId, string hostUrl)
        {
            this.Id = domainCategory.Id;
            this.Name = domainCategory.Name;
            this.Url = hostUrl + "/api/" + userName + "/receipts/" + receiptId + "/categories/" + Id;
        }
    }
}