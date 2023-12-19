using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewPustok.Contexts;
using NewPustok.ViewModels.CategoryVM;
using NewPustok.ViewModels.SliderVM;

namespace NewPustok.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin, Admin, Moderator")]
    public class CategoryController : Controller
    {
        NewPustokDbContext _db { get; }

        public CategoryController(NewPustokDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _db.Categorys.Select(c => new CategoryListItemVM { Id = c.Id, Name = c.Name }).ToListAsync());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreateVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            if (await _db.Categorys.AnyAsync(x => x.Name == vm.Name))
            {
                ModelState.AddModelError("Name", vm.Name + " already exist");
                return View(vm);
            }
            await _db.Categorys.AddAsync(new Models.Category { Name = vm.Name });
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();
            var data = await _db.Categorys.FindAsync(id);
            if (data == null) return RedirectToAction(nameof(Index));
            _db.Categorys.Remove(data);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();
            var data = await _db.Categorys.FindAsync(id);
            if (data == null) return RedirectToAction(nameof(Index));
            return View(new CategoryUpdateVM
            {
                Name = data.Name
            });
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, CategoryUpdateVM vm)
        {
            if (id == null || id <= 0) return BadRequest();
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var data = await _db.Categorys.FindAsync(id);
            if (data == null) return NotFound();
            data.Name = vm.Name;
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
    
}
