using Farmers.App.Models;
using Farmers.DataModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Farmers.App.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: Admin/Index - Admin Dashboard
        public async Task<IActionResult> Index()
        {
            // Get unapproved farmers (Farmers with EmailConfirmed as false)
            var pendingFarmers = await _userManager.GetUsersInRoleAsync("Farmer");
            var unapprovedFarmers = pendingFarmers.Where(u => !u.EmailConfirmed).ToList();

            // Get active couriers (All users with the "Courier" role)
            var activeCouriers = await _userManager.GetUsersInRoleAsync("Courier");

            // Get active farmers (Farmers with EmailConfirmed as true)
            var activeFarmers = pendingFarmers.Where(u => u.EmailConfirmed).ToList();

            // Get active customers (All users with the "Customer" role)
            var activeCustomers = await _userManager.GetUsersInRoleAsync("Customer");

            var viewModel = new AdminDashboardViewModel
            {
                PendingFarmers = unapprovedFarmers,
                ActiveCouriers = activeCouriers,
                ActiveFarmers = activeFarmers,
                ActiveCustomers = activeCustomers
            };

            return View(viewModel);
        }

        // GET: Admin/AddCourier - Form to add courier account
        [HttpGet]
        public IActionResult AddCourier()
        {
            return View(new AddCourierViewModel());
        }

        // POST: Admin/AddCourier - Handle adding of courier account
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCourier(AddCourierViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FullName = model.FullName,
                    EmailConfirmed = true // No approval required for courier
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Create "Courier" role if it does not exist
                    if (!await _roleManager.RoleExistsAsync("Courier"))
                    {
                        var roleResult = await _roleManager.CreateAsync(new IdentityRole("Courier"));
                        if (!roleResult.Succeeded)
                        {
                            ModelState.AddModelError(string.Empty, "Error creating role. Please try again.");
                            return View(model);
                        }
                    }

                    // Assign "Courier" role to the user
                    await _userManager.AddToRoleAsync(user, "Courier");

                    TempData["SuccessMessage"] = "Courier account successfully created.";
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        // POST: Admin/ApproveFarmer
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveFarmer(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return NotFound();

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound();

            user.EmailConfirmed = true;
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                TempData["Message"] = "Farmer approved successfully.";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(string.Empty, "Unable to approve the farmer.");
            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/DenyFarmer
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DenyFarmer(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return NotFound();

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound();

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                TempData["Message"] = "Farmer denied successfully.";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(string.Empty, "Unable to deny the farmer.");
            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/RemoveCourier
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveCourier(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return NotFound();

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound();

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                TempData["Message"] = "Courier removed successfully.";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(string.Empty, "Unable to remove the courier.");
            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/RemoveFarmer
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveFarmer(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return NotFound();

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound();

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                TempData["Message"] = "Farmer removed successfully.";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(string.Empty, "Unable to remove the farmer.");
            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/RemoveCustomer
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveCustomer(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return NotFound();

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound();

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                TempData["Message"] = "Customer removed successfully.";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(string.Empty, "Unable to remove the customer.");
            return RedirectToAction(nameof(Index));
        }
    }
}
