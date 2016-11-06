namespace Receipt.API.Controllers
{
    using Domain.Entities;
    using Model;
    using Services;
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

            return this.repository.GetAll(userName);
        }
    }
}
