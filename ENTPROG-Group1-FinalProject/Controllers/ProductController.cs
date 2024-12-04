using AutoMapper;
using Farmers.App.Models;
using Farmers.DataModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FTG.Repository.Repository;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Farmers.App.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IProductRepo _productRepo;
        private readonly IFarmerRepo _farmerRepo;
        private readonly ICategoryRepo _categoryRepo;
        private readonly IMapper _mapper;

        public ProductController(IProductRepo productRepo, IFarmerRepo farmerRepo, ICategoryRepo categoryRepo, IMapper mapper)
        {
            _productRepo = productRepo;
            _farmerRepo = farmerRepo;
            _categoryRepo = categoryRepo;
            _mapper = mapper;
        }

        // GET: Product/Index
        public async Task<IActionResult> Index()
        {
            var products = await _productRepo.GetAllAsync();
            var productVMs = products.Select(p => new ProductVM
            {
                ProductId = p.ProductId,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Stock = p.Stock,
                CategoryName = p.Category.Name,
                FarmerName = p.Farmer.User.FullName,
                DateAdded = p.DateAdded
            }).ToList();

            return View(productVMs);
        }

        // GET: Product/MyProducts
        [Authorize(Roles = "Farmer")]
        public async Task<IActionResult> MyProducts()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var farmer = await _farmerRepo.GetByUserIdAsync(userId);
            if (farmer == null)
            {
                return Unauthorized();
            }

            var products = await _productRepo.GetByFarmerIdAsync(farmer.FarmerId);
            var productVMs = _mapper.Map<List<ProductVM>>(products);
            return View(productVMs);
        }

        // GET: Product/Add
        [Authorize(Roles = "Farmer")]
        public IActionResult Add()
        {
            var model = new ProductVM
            {
                Categories = new List<SelectListItem>
                {
                    new SelectListItem { Value = "1", Text = "Vegetables" },
                    new SelectListItem { Value = "2", Text = "Fruits" },
                    new SelectListItem { Value = "3", Text = "Dairy" }
                }
            };
            return View(model);
        }

        // POST: Product/Add
        [HttpPost]
        [Authorize(Roles = "Farmer")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(ProductVM model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var farmer = await _farmerRepo.GetByUserIdAsync(userId);
                if (farmer == null)
                {
                    return Unauthorized();
                }

                var product = _mapper.Map<Product>(model);
                product.FarmerId = farmer.FarmerId;

                await _productRepo.AddAsync(product);
                return RedirectToAction(nameof(MyProducts));
            }

            // Re-populate categories in case of validation errors
            model.Categories = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "Vegetables" },
                new SelectListItem { Value = "2", Text = "Fruits" },
                new SelectListItem { Value = "3", Text = "Dairy" }
            };
            return View(model);
        }

        // GET: Product/Edit/{id}
        [Authorize(Roles = "Farmer")]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productRepo.GetAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var farmer = await _farmerRepo.GetByUserIdAsync(userId);
            if (farmer == null || product.FarmerId != farmer.FarmerId)
            {
                return Unauthorized();
            }

            var model = _mapper.Map<ProductVM>(product);
            model.Categories = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "Vegetables" },
                new SelectListItem { Value = "2", Text = "Fruits" },
                new SelectListItem { Value = "3", Text = "Dairy" }
            };

            return View(model);
        }

        // POST: Product/Edit/{id}
        [HttpPost]
        [Authorize(Roles = "Farmer")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductVM model)
        {
            if (ModelState.IsValid)
            {
                var product = await _productRepo.GetAsync(id);
                if (product == null)
                {
                    return NotFound();
                }

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var farmer = await _farmerRepo.GetByUserIdAsync(userId);
                if (farmer == null || product.FarmerId != farmer.FarmerId)
                {
                    return Unauthorized();
                }

                _mapper.Map(model, product);
                await _productRepo.UpdateAsync(product);

                return RedirectToAction(nameof(MyProducts));
            }

            // Re-populate categories in case of validation errors
            model.Categories = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "Vegetables" },
                new SelectListItem { Value = "2", Text = "Fruits" },
                new SelectListItem { Value = "3", Text = "Dairy" }
            };
            return View(model);
        }

        // DELETE: Product/Delete/{id}
        [HttpPost]
        [Authorize(Roles = "Farmer")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productRepo.GetAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var farmer = await _farmerRepo.GetByUserIdAsync(userId);
            if (farmer == null || product.FarmerId != farmer.FarmerId)
            {
                return Unauthorized();
            }

            await _productRepo.DeleteAsync(id);
            return RedirectToAction(nameof(MyProducts));
        }
    }
}
