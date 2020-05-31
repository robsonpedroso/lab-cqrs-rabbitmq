using CQRS.Core.Application;
using CQRS.Core.Domain.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CQRS.Api.Controllers
{
    [ApiController, Route("v1/[controller]")]
    public class ProductsController : Controller
    {
        private readonly ProductApplication productApplication;

        public ProductsController(ProductApplication productApplication)
            => this.productApplication = productApplication;

        [HttpPost, Route("")]
        public IActionResult Save(Product product)
        {
            return Ok(productApplication.Save(product));
        }

        [HttpGet, Route("{id}")]
        public IActionResult Get(Guid id)
        {
            var result = productApplication.Get(id);

            return Ok(result);
        }

        [HttpDelete, Route("{id}")]
        public IActionResult Delete(Guid id)
        {
            productApplication.Delete(id);

            return Ok();
        }
    }
}