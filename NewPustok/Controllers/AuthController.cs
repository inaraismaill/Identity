﻿//using NewPustok.Models;
//using NewPustok.ViewModels.AuthVM;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using NewPustok.Helpers;

//namespace NewPustok.Controllers
//{
//    public class AuthController : Controller
//    {
//        SignInManager<AppUser> _signInManager { get; }
//        UserManager<AppUser> _userManager { get; }
//        RoleManager<IdentityRole> _roleManager { get; }

//        public AuthController(SignInManager<AppUser> signInManager,
//            UserManager<AppUser> userManager,
//            RoleManager<IdentityRole> roleManager)
//        {
//            _signInManager = signInManager;
//            _userManager = userManager;
//            _roleManager = roleManager;
//        }
//        public IActionResult Login()
//        {
//            return View();
//        }
//        [HttpPost]
//        public async Task<IActionResult> Login(string? returnUrl, LoginVM vm)
//        {
//            AppUser user;
//            if (vm.UsernameOrEmail.Contains("@"))
//            {
//                user = await _userManager.FindByEmailAsync(vm.UsernameOrEmail);
//            }
//            else
//            {
//                user = await _userManager.FindByNameAsync(vm.UsernameOrEmail);
//            }
//            if (user == null)
//            {
//                ModelState.AddModelError("", "Username or password is wrong");
//                return View(vm);
//            }
//            var result = await _signInManager.PasswordSignInAsync(user, vm.Password, vm.IsRemember, true);
//            if (!result.Succeeded)
//            {
//                if (result.IsLockedOut)
//                {
//                    ModelState.AddModelError("", "Too many attempts wait until " + DateTime.Parse(user.LockoutEnd.ToString()).ToString("HH:mm"));
//                }
//                else
//                {
//                    ModelState.AddModelError("", "Username or password is wrong");
//                }
//                return View(vm);
//            }
//            if (returnUrl != null)
//            {
//                return LocalRedirect(returnUrl);
//            }
//            return RedirectToAction("Index", "Home");
//        }
//        public IActionResult Register()
//        {
//            return View();
//        }
//        [HttpPost]
//        public async Task<IActionResult> Register(RegisterVM vm)
//        {
//            if (!ModelState.IsValid)
//            {
//                return View(vm);
//            }
//            var user = new AppUser
//            {
//                Fullname = vm.Fullname,
//                Email = vm.Email,
//                UserName = vm.Username
//            };
//            var result = await _userManager.CreateAsync(user, vm.Password);
//            if (!result.Succeeded)
//            {
//                foreach (var error in result.Errors)
//                {
//                    ModelState.AddModelError("", error.Description);
//                }
//                return View(vm);
//            }
//            var roleResult = await _userManager.AddToRoleAsync(user, Roles.Member.ToString());
//            if (!roleResult.Succeeded)
//            {
//                ModelState.AddModelError("", "Something went wrong. Please contact admin");
//                return View(vm);
//            }
//            return View();
//        }
//        public async Task<IActionResult> Logout()
//        {
//            await _signInManager.SignOutAsync();
//            return RedirectToAction("Index", "Home");
//        }
//        public async Task<bool> CreateRoles()
//        {
//            foreach (var item in Enum.GetValues(typeof(Roles)))
//            {
//                if (!await _roleManager.RoleExistsAsync(item.ToString()))
//                {
//                    var result = await _roleManager.CreateAsync(new IdentityRole
//                    {
//                        Name = item.ToString()
//                    });
//                    if (!result.Succeeded)
//                    {
//                        return false;
//                    }
//                }
//            }
//            return true;
//        }
//    }

//}
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NewPustok.Helpers;
using NewPustok.Models;
using NewPustok.ViewModels;
using NewPustok.ViewModels.AuthVM;

namespace WebApplicationPustok.Controllers
{
    public class AuthController : Controller
    {
        public AuthController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        SignInManager<AppUser> _signInManager { get; }
        UserManager<AppUser> _userManager { get; }
        RoleManager<IdentityRole> _roleManager { get; }


        //==============================Registr==========================
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var user = new AppUser
            {
                Fullname = vm.Fullname,
                Email = vm.Email,
                UserName = vm.Username
            };
            var result = await _userManager.CreateAsync(user, vm.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(vm);
            }
            var roleResult = await _userManager.AddToRoleAsync(user, Roles.Member.ToString());
            if (!roleResult.Succeeded)
            {
                ModelState.AddModelError("", "Something went wrong. Please contact admin");
                return View(vm);
            }
            return RedirectToAction("Index", "Home");
        }

        //=====================================Login============================
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM vm)
        {
            AppUser user;
            if (vm.UsernameOrEmail.Contains("@"))
            {
                user = await _userManager.FindByEmailAsync(vm.UsernameOrEmail);
              
            }
            else
            {
                user = await _userManager.FindByNameAsync(vm.UsernameOrEmail);
            }
            var result = await _signInManager.PasswordSignInAsync(user, vm.Password, vm.IsRemember, true);
            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                {
                    ModelState.AddModelError("", "Too many attempts wait until " + DateTime.Parse(user.LockoutEnd.ToString()).ToString("HH:mm"));
                }
                else
                {
                    ModelState.AddModelError("", "Username or password is wrong");
                }
                return View(vm);
            }

            //if (returnUrl != null)
            //{
            //	return LocalRedirect(returnUrl);
            // }
            return RedirectToAction("Index", "Home");

        }


        //=========================Logout==============================
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        //=========================Role========================
        public async Task<bool> CreatedRoles()
        {
            foreach (var item in Enum.GetValues(typeof(Roles)))
            {
                if (!await _roleManager.RoleExistsAsync(item.ToString()))
                {
                    var result = await _roleManager.CreateAsync(new IdentityRole
                    {
                        Name = item.ToString()
                    });
                    if (!result.Succeeded)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        public async Task<IActionResult> Update()
        {

            var existsUser = await _userManager.FindByNameAsync(User.Identity.Name);
            UserUpdateVM vm = new UserUpdateVM();

            if (existsUser != null)
            {
                vm = new UserUpdateVM() 
                { 
                    Fullname = existsUser.Fullname,
                    Username = existsUser.UserName,
                };
            }
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Update(UserUpdateVM vm)
        {
           
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            if(User.Identity.Name==null) return NotFound();

            var existsUser = await _userManager.FindByNameAsync(User.Identity.Name);
            existsUser.UserName = vm.Username;
            existsUser.PasswordHash = vm.NewPassword;
            existsUser.Fullname = vm.Fullname;

            if (vm.ImageFile != null && vm.ImageFile.Length > 0)
            {
                existsUser.ProfileImageUrl = await vm.ImageFile.SaveAsync(PathConstants.Product);
            }

            await _userManager.UpdateAsync(existsUser);

            return View();
        }
    }
}
