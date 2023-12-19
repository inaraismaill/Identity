using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewPustok.Contexts;
using NewPustok.Helpers;
using NewPustok.Models;
using NewPustok.ViewModels;
using NewPustok.ViewModels.UserVM;

namespace NewPustok.Controllers
{
    public class UserController : Controller
    {
        NewPustokDbContext _db { get; }
        IWebHostEnvironment _env { get; }
        public UserController(NewPustokDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _db.Users.Select(s => new UserListItemVM
            {
                Username = s.Username,
                Fullname = s.Fullname,
                Password = s.Password,
                ConfirmPassword = s.ConfirmPassword,
                ImageUrl = s.ImageUrl,

            }).ToListAsync();
            return View(user);
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(UserCreateVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            User User = new User
            {
                ImageUrl = await vm.ImageFile.SaveAsync(PathConstants.Product),
                Password = vm.Password,
                ConfirmPassword = vm.ConfirmPassword,
                Username = vm.Username,
                Fullname = vm.Fullname,

            };
            //await _db.Users.AddAsync(Models.User);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();
            var data = await _db.Users.FindAsync(id);
            if (data == null) return RedirectToAction(nameof(Index));
            _db.Users.Remove(data);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();
            var data = await _db.Users.FindAsync(id);
            if (data == null) return RedirectToAction(nameof(Index));
            return View(new UserUpdateVM
            {
                Fullname = data.Fullname,
                Username = data.Username,
                Password = data.Password,
                ConfirmPassword = data.ConfirmPassword,
            });
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, UserUpdateVM vm)
        {
            if (id == null || id <= 0) return BadRequest();

            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var data = await _db.Users.FindAsync(id);
            if (data == null) return NotFound();
            data.Fullname = vm.Fullname;
            data.Username = vm.Username;
            data.Password = vm.Password;
            data.ConfirmPassword = vm.ConfirmPassword;
            if (vm.ImageFile != null && vm.ImageFile.Length > 0)
            {
                data.ImageUrl = await vm.ImageFile.SaveAsync(PathConstants.Product);
            }

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
