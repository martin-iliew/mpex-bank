using Microsoft.AspNetCore.Mvc;

namespace Mpex.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
