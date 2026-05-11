using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.Client
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : Controller
    {
        [HttpGet("products")]
        public IActionResult Products()
        {
            return PartialView("Client/HomePage/_ProductsPartial");
        }
        [HttpGet("loadProducts")]
        public IActionResult LoadProducts()
        {
            return PartialView("Client/HomePage/_ProductsPartial");
        }
    }
}
