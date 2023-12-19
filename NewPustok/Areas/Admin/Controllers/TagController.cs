using Microsoft.AspNetCore.Mvc;
using NewPustok.Contexts;
using NewPustok.ViewModels.TagVM;
using NewPustok.ViewModels.SliderVM;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace NewPustok.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin, Admin, Moderator")]
    public class TagController : Controller
    {
        NewPustokDbContext _db { get; }

        public TagController(NewPustokDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _db.Tags.Select(c => new TagListItemVM { Id = c.Id, Name = c.Name }).ToListAsync());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(TagCreateVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            if (await _db.Tags.AnyAsync(x => x.Name == vm.Name))
            {
                ModelState.AddModelError("Name", vm.Name + " already exist");
                return View(vm);
            }
            await _db.Tags.AddAsync(new Models.Tag { Name = vm.Name });
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();
            var data = await _db.Tags.FindAsync(id);
            if (data == null) return RedirectToAction(nameof(Index));
            _db.Tags.Remove(data);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();
            var data = await _db.Tags.FindAsync(id);
            if (data == null) return RedirectToAction(nameof(Index));
            return View(new TagUpdateVM
            {
                Name = data.Name
            });
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, TagUpdateVM vm)
        {
            if (id == null || id <= 0) return BadRequest();
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var data = await _db.Tags.FindAsync(id);
            if (data == null) return NotFound();
            data.Name = vm.Name;
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
