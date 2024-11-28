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
                TempData["MessageType"] = "success";
                return RedirectToAction(nameof(Index));
            }

            TempData["Message"] = "Unable to approve the farmer.";
            TempData["MessageType"] = "error";
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
                TempData["MessageType"] = "success";
                return RedirectToAction(nameof(Index));
            }

            TempData["Message"] = "Unable to deny the farmer.";
            TempData["MessageType"] = "error";
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
                TempData["MessageType"] = "success";
                return RedirectToAction(nameof(Index));
            }

            TempData["Message"] = "Unable to remove the courier.";
            TempData["MessageType"] = "error";
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
                TempData["MessageType"] = "success";
                return RedirectToAction(nameof(Index));
            }

            TempData["Message"] = "Unable to remove the farmer.";
            TempData["MessageType"] = "error";
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
                TempData["MessageType"] = "success";
                return RedirectToAction(nameof(Index));
            }

            TempData["Message"] = "Unable to remove the customer.";
            TempData["MessageType"] = "error";
            return RedirectToAction(nameof(Index));
        }
    }
}
