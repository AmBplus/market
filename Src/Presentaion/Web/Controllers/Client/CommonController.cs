using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.Client
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommonController : Controller
    {
  
        [HttpGet("MenuCategories")]
        public IActionResult MenuCategories()
        {
            return PartialView("Client/Common/_MenuCategoriesPartial");
        }
    }
}
