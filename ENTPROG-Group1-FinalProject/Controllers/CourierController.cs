using AutoMapper;
using Farmers.App.Models;
using Farmers.App.Models.Repositories;
using Farmers.DataModel;
using Microsoft.AspNetCore.Mvc;

namespace Farmers.App.Controllers
{
    public class CourierController : Controller
    {
        private readonly AppDbContext dbc;
        private readonly IMapper mapper;
        private readonly IProductRepo repo;
        public CourierController(IProductRepo repo, AppDbContext dbc, IMapper mapper)
        {
            this.repo = repo;
            this.dbc = dbc;
            this.mapper = mapper;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Add()
        {
            return View(new CreateCourierViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(CreateCourierViewModel model)
        {
            try
            {
                if (ModelState.IsValid == true)
                {
                    Courier c = mapper.Map<Courier>(model);
                    await dbc.AddAsync(model);
                    await dbc.SaveChangesAsync();
                    return RedirectToAction("/Admin/Index");
                }
                else
                {
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ex.Message);
                return View(model);
            }
        }
    }
}
