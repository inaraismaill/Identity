using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NewPustok.Contexts;
using NewPustok.Helpers;
using NewPustok.Models;
using NewPustok.ViewModels.BlogVM;

namespace NewPustok.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin, Admin, Moderator")]
    public class BlogController : Controller
    {
        public BlogController(NewPustokDbContext db)
        {
            _db = db;
        }
        NewPustokDbContext _db { get; set; }
        public async Task<IActionResult> Index()
        {
            return View(await _db.Blogs.Select(s => new BlogListItemVM {
                Id = s.Id,
                Title = s.Title,
                Description = s.Description,
                ImageUrl = s.ImageUrl,
                Tags = s.BlogTags.Select(pc => pc.Tag)
            }).ToListAsync());
        }
        public IActionResult Create()
        {
            ViewBag.Tags = _db.Tags.ToList();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(BlogCreateVM vm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Tags = _db.Tags.ToList();
                return View(vm);
            }
            if (await _db.Tags.Where(c => vm.TagId.Contains(c.Id)).Select(c => c.Id).CountAsync() != vm.TagId.Count())
            {
                ModelState.AddModelError("TagsId", "Tag doesnt exist");
                ViewBag.Tags = _db.Tags.ToList() ;
                return View(vm);
            }
            Blog blog = new Blog
            {
                Title = vm.Title,
                Description = vm.Description,
                CreatedAt = DateTime.Now,
                LastUpdatedAt = DateTime.Now,
                ImageUrl = vm.ImageUrl,
                BlogTags = vm.TagId.Select(id => new BlogTag
                {
                    TagId = id,
                }).ToList()
            };
            await _db.Blogs.AddAsync(blog);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null || id <= 0) return BadRequest();

            var data = await _db.Blogs.FindAsync(id);
            if (data == null) return NotFound();
            ViewBag.Tags = _db.Tags.ToList();

            return View(new BlogUpdateVM
            {
                Title = data.Title,
                Description = data.Description,
                ImageUrl = data.ImageUrl,
            });
        }
        [HttpPost]

        public async Task<IActionResult> Update(int? id, BlogUpdateVM vm)
        {
            if (id == null || id <= 0) return BadRequest();
            if (!ModelState.IsValid)
            {
                ViewBag.Tags = _db.Tags.ToList();
                return View(vm);
            }
            var data = await _db.Blogs.FindAsync(id);
            if (data == null) return NotFound();
            data.LastUpdatedAt = DateTime.Now;
            data.Title = vm.Title;
            data.Description = vm.Description;
            data.ImageUrl = vm.ImageUrl;

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int? id)
        {

            if (id == null) return BadRequest();

            var data = await _db.Blogs.FindAsync(id);
            if (data == null) return NotFound();
            _db.Blogs.Remove(data);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }
    }
}

