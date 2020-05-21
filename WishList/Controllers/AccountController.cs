using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.KeyVault.Models;
using WishList.Models;
using WishList.Models.AccountViewModels;

namespace WishList.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View("Register");
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Register(RegisterViewModel registerViewModel) 
        {
            if (!ModelState.IsValid)
                return View(registerViewModel);
            
            var newUser = new ApplicationUser()
            {
                UserName = registerViewModel.Email,
                Email = registerViewModel.Email,
            };
            var password = registerViewModel.Password;

            var identityResult = _userManager.CreateAsync(newUser, password).Result;
            if (!identityResult.Succeeded)
            {
                foreach(var error in identityResult.Errors)
                {
                    ModelState.AddModelError("Password", error.Description);
                }

                return View(registerViewModel);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
