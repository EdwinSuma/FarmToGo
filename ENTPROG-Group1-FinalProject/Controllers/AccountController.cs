﻿using Farmers.App.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Farmers.App.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        // GET: Account/Register
        public IActionResult Register()
        {
            return View(); // This will use Register.cshtml where users choose their registration type
        }

        // POST: Account/Register (handles both customers and farmers)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string userRole)
        {
            if (ModelState.IsValid)
            {
                // Ensure that userRole is provided and valid
                if (string.IsNullOrEmpty(userRole) || (userRole != "Customer" && userRole != "Farmer"))
                {
                    ModelState.AddModelError(string.Empty, "Please select a valid role (Customer or Farmer).");
                    return View(model);
                }

                // Check if the email is already registered
                var existingUser = await _userManager.FindByEmailAsync(model.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", "The email address is already registered. Please use a different email.");
                    return View(model);
                }

                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FullName = model.FullName,
                    EmailConfirmed = userRole == "Customer" // Farmers need admin approval, so they won't be confirmed by default
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Create role if it does not exist
                    if (!await _roleManager.RoleExistsAsync(userRole))
                    {
                        var roleResult = await _roleManager.CreateAsync(new IdentityRole(userRole));
                        if (!roleResult.Succeeded)
                        {
                            ModelState.AddModelError(string.Empty, "Error creating role. Please try again.");
                            return View(model);
                        }
                    }

                    // Assign the role to the user
                    await _userManager.AddToRoleAsync(user, userRole);

                    if (userRole == "Farmer")
                    {
                        // Redirect farmers to PendingApproval page
                        return RedirectToAction("PendingApproval", "Account");
                    }
                    else if (userRole == "Customer")
                    {
                        // Sign in customer immediately
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return RedirectToAction("Index", "Home");
                    }
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model); // Reuse the same view in case of validation errors
        }

        // GET: Account/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user == null)
                {
                    // If the email does not exist in the system
                    ModelState.AddModelError("Email", "The email address does not exist in our system.");
                    return View(model);
                }

                if (!await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    // If the password is incorrect for the given email
                    ModelState.AddModelError("Password", "The password you entered is incorrect. Please try again.");
                    return View(model);
                }

                // Redirect unapproved farmers to the PendingApproval page
                if (!user.EmailConfirmed && await _userManager.IsInRoleAsync(user, "Farmer"))
                {
                    return RedirectToAction("PendingApproval", "Account");
                }

                // Sign the user in if the credentials are correct
                var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                // In case of other login failure reasons
                ModelState.AddModelError(string.Empty, "Login failed. Please try again.");
            }

            return View(model);
        }

        // POST: Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        // GET: Account/PendingApproval
        public IActionResult PendingApproval()
        {
            return View();
        }

        // GET: Account/Manage - View for managing account
        [HttpGet]
        public async Task<IActionResult> Manage()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction(nameof(Login));
            }

            var model = new ManageAccountViewModel
            {
                FullName = user.FullName,
                Email = user.Email
            };

            return View(model);
        }

        // GET: Account/EditProfile
        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction(nameof(Login));
            }

            var model = new UpdateProfileViewModel
            {
                FullName = user.FullName,
                Email = user.Email
            };

            return View(model);
        }

        // POST: Account/EditProfile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(UpdateProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction(nameof(Login));
            }

            user.FullName = model.FullName;
            user.Email = model.Email;
            user.UserName = model.Email;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Profile updated successfully.";
                return RedirectToAction(nameof(Manage));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        // GET: Account/ChangePassword
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        // POST: Account/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction(nameof(Login));
            }

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Password changed successfully.";
                return RedirectToAction(nameof(Manage));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        // POST: Account/DeleteAccount
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAccount()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction(nameof(Login));
            }

            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction(nameof(Index), "Home");
            }

            TempData["ErrorMessage"] = "Error occurred while deleting your account. Please try again.";
            return RedirectToAction(nameof(Manage));
        }
    }
}
