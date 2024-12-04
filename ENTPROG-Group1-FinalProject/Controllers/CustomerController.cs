using Microsoft.AspNetCore.Mvc;
using Farmers.App.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Linq;
using FTG.Repository.Repository;
using Microsoft.AspNetCore.Authorization;

namespace Farmers.App.Controllers
{
     // Restrict access to logged-in users
    public class CustomerController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IProductRepo _productRepo; // Service to fetch products
        private readonly IOrderRepo _orderRepo; // Service to handle orders

        public CustomerController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IProductRepo productService,
            IOrderRepo orderService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _productRepo = productService;
            _orderRepo = orderService;
        }

        // GET: Customer/Products
        public async Task<IActionResult> Index()
        {
            // Ensure the user is logged in and is a customer
            var user = await _userManager.GetUserAsync(User);
            if (user == null || !await _userManager.IsInRoleAsync(user, "Customer"))
            {
                return RedirectToAction("Login", "Account"); // Redirect if not authenticated or not a customer
            }

            // Fetch products that are in stock
            var products = await _productRepo.GetAvailableProductsAsync();
            var inStockProducts = products.Where(p => p.Stock > 0); // Ensure stock is greater than 0

            // Create CustomerViewModel
            var viewModel = new CustomerViewModel
            {
                Products = inStockProducts
            };

            return View(viewModel); // Pass CustomerViewModel to the view
        }

        // GET: Customer/BuyProduct/5
        public async Task<IActionResult> BuyProduct(int id)
        {
            // Fetch the product by ID
            var product = await _productRepo.GetProductByIdAsync(id);
            if (product == null || product.Stock <= 0)
            {
                return NotFound(); // Ensure product exists and is in stock
            }

            // Create BuyProductViewModel
            var model = new BuyProductViewModel
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock
            };

            // Create CustomerViewModel with BuyProductViewModel
            var viewModel = new CustomerViewModel
            {
                BuyProductViewModel = model
            };

            return View(viewModel);
        }

        // POST: Customer/BuyProduct/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BuyProduct(CustomerViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return RedirectToAction("Login", "Account"); // Redirect if not authenticated
                }

                var product = await _productRepo.GetProductByIdAsync(viewModel.BuyProductViewModel.ProductId);
                if (product == null || product.Stock < viewModel.BuyProductViewModel.Quantity)
                {
                    ModelState.AddModelError(string.Empty, "Product is not available in the requested quantity.");
                    return View(viewModel); // Ensure product exists and stock is sufficient
                }

                // Create an order for the customer
                var orderResult = await _orderRepo.CreateOrderAsync(user, product, viewModel.BuyProductViewModel.Quantity);
                if (orderResult.IsSuccess)
                {
                    TempData["SuccessMessage"] = $"Successfully purchased {product.Name}.";
                    return RedirectToAction("OrderConfirmation", new { orderId = orderResult.OrderId });
                }

                // Handle order creation error
                ModelState.AddModelError(string.Empty, "There was an issue processing your order. Please try again.");
            }

            return View(viewModel);
        }

        // GET: Customer/OrderConfirmation/5
        public async Task<IActionResult> OrderConfirmation(int orderId)
        {
            // Fetch the order details
            var order = await _orderRepo.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                return NotFound();
            }

            // Create CustomerViewModel with Order
            var viewModel = new CustomerViewModel
            {
                Order = order
            };

            return View(viewModel); // Pass Order to the view
        }
    }
} 

