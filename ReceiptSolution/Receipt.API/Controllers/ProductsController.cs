namespace Receipt.API.Controllers
{
    using Domain.Entities;
    using Receipt.API.Model;
    using System.Collections.Generic;
    using System.Web.Http;

    public class ProductsController : ApiController
    {
        private readonly IProductRepository repository;

        public ProductsController(IProductRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        [Route("api/products/")]
        public IEnumerable<Product> GetAllProducts()
        {
            return this.repository.GetAll();
        }
    }
}
