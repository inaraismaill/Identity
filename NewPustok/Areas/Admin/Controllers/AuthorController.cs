using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewPustok.Contexts;
using NewPustok.ViewModels.AuthorVM;

namespace NewPustok.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin, Admin, Moderator")]
    public class AuthorController : Controller
    {
        NewPustokDbContext _db { get; }

        public AuthorController(NewPustokDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _db.Authors.Select(c => new AuthorIndexVM { Id = c.Id, Name = c.Name }).ToListAsync());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(AuthorCreateVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            if (await _db.Authors.AnyAsync(x => x.Name == vm.Name))
            {
                ModelState.AddModelError("Name", vm.Name + " already exist");
                return View(vm);
            }
            await _db.Authors.AddAsync(new Models.Author { Name = vm.Name });
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();
            var data = await _db.Authors.FindAsync(id);
            if (data == null) return RedirectToAction(nameof(Index));
            _db.Authors.Remove(data);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();
            var data = await _db.Authors.FindAsync(id);
            if (data == null) return RedirectToAction(nameof(Index));
            return View(new AuthorUpdateVM
            {
                Name = data.Name
            });
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, AuthorUpdateVM vm)
        {
            if (id == null || id <= 0) return BadRequest();
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var data = await _db.Authors.FindAsync(id);
            if (data == null) return NotFound();
            data.Name = vm.Name;
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
