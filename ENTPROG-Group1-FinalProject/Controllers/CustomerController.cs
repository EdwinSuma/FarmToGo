using Microsoft.AspNetCore.Mvc;

namespace Farmers.App.Controllers
{
    public class CustomerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
