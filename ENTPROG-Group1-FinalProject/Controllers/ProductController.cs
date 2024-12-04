using AutoMapper;
using Farmers.App.Models;
using Farmers.DataModel;
using Microsoft.AspNetCore.Mvc;
using Farmers.App.Models.Repositories;
using FTG.Repository.Repository;
using Microsoft.EntityFrameworkCore;

namespace SupplierINV.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext dbc;
        private readonly IMapper mapper;
        private readonly IProductRepo repo;
        public ProductController(IProductRepo repo, AppDbContext dbc, IMapper mapper)
        {
            this.repo = repo;
            this.dbc = dbc;
            this.mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            return View(mapper.Map<List<ProductVM>>(await repo.GetAllAsync()));
        }


        public IActionResult Add()
        {
            return View(new ProductVM());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(ProductVM model)
        {
            try
            {
                if (ModelState.IsValid == true)
                {
                    ProductVM product = new ProductVM();
                    var Seller = User.Identity.Name;
                    product.ProductName = model.ProductName;
                    await repo.AddAsync(mapper.Map<Product>(model));
                    await dbc.SaveChangesAsync();
                    return RedirectToAction("Index");
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

        public async Task<IActionResult> Edit(int? Id)
        {
            if (Id == null)
            {
                return RedirectToAction("Index");
            }

            ProductVM product = mapper.Map<ProductVM>(await repo.GetAsync((int)Id));
            return View(mapper.Map<ProductVM>(product));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductVM model)
        {
            if (ModelState.IsValid)
            {
                await repo.UpdateAsync(mapper.Map<Product>(model));
                return RedirectToAction("Index");
            }
            else
            {
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int Id)
        {
            await repo.DeleteAsync(Id);
            return RedirectToAction("Index");
        }
    }
}
