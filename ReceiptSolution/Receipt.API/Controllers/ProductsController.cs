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

        [HttpGet]
        [Route("api/products/{id}")]
        public IHttpActionResult GetProductById(int id)
        {
            var product = this.repository.GetOneById(id);
            if (product == null)
            {
                return this.NotFound();
            }

            return this.Ok(product);
        }
    }
}
