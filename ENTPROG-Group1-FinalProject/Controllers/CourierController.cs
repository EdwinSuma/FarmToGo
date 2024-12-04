using Microsoft.AspNetCore.Mvc;

namespace Farmers.App.Controllers
{
    public class CourierController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
