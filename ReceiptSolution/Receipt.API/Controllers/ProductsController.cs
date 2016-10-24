namespace Receipt.API.Controllers
{
    using Authentication;
    using Domain.Entities;
    using Receipt.API.Model;
    using System.Collections.Generic;
    using System.Web.Http;

    public class ProductsController : ApiController
    {
        private readonly IProductRepository repository;
        private readonly AuthService authService; 

        public ProductsController(IProductRepository repository)
        {
            this.repository = repository;
            this.authService = new AuthService();
        }

        [HttpGet]
        [Authorize]
        [Route("api/products/")]
        public IEnumerable<Product> GetAllProducts()
        {
            string userName = this.authService.GetUserName(this.User);

            return this.repository.GetAll();
        }

        [HttpGet]
        [Authorize]
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
