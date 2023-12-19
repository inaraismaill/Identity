using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewPustok.Contexts;
using NewPustok.Helpers;
using NewPustok.Models;
using NewPustok.ViewModels.SliderVM;

namespace NewPustok.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin, Admin, Moderator")]
    public class SliderController : Controller
    {
        NewPustokDbContext _db { get; }
        IWebHostEnvironment _env { get; }
        public SliderController(NewPustokDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            var slid= await _db.Sliders.Select(s=> new SliderItemVM
            {
                Title = s.Title,
                Description = s.Description,
                Id = s.Id,
                ImageUrl=s.ImageUrl,
                IsLeft = s.IsLeft,
            }).ToListAsync();
            return View(slid);
        }
        public async Task<IActionResult> Create() 
        { 
            return View(); 
        }
        [HttpPost]
        public async Task<IActionResult> Create(SliderCreateVM vm)
        {
           
            if (vm.Position < -1 || vm.Position > 1)
            {
                ModelState.AddModelError("Position", "Wrong input");
            }
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            Slider slider = new Slider
            {
                Title = vm.Title,
                Description = vm.Description,
                ImageUrl =await vm.ImageFile.SaveAsync(PathConstants.Product), 
                IsLeft = vm.Position switch
                {
                    0 => null,
                    -1 => true,
                    1 => false 
                }
            };
            await _db.Sliders.AddAsync(slider);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();
            var data= await _db.Sliders.FindAsync(id);
            if(data == null) return RedirectToAction(nameof(Index));
            _db.Sliders.Remove(data);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();
            var data = await _db.Sliders.FindAsync(id);
            if (data == null) return RedirectToAction(nameof(Index));
            return View(new SliderUpdateVM 
            {   
                Description = data.Description,
                Title= data.Title,
                Position=data.IsLeft switch
                {
                    null => 0,
                    true => -1,
                    false => 1
                }
            });
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, SliderUpdateVM vm)
        {
            if (id == null || id <= 0) return BadRequest();
            if (vm.Position < -1 || vm.Position > 1)
            {
                ModelState.AddModelError("Position", "Wrong input");
            }
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var data = await _db.Sliders.FindAsync(id);
            if (data == null) return NotFound();
            data.Description = vm.Description;
            data.Title = vm.Title;
            if (vm.ImageFile != null && vm.ImageFile.Length > 0)
            {
                data.ImageUrl = await vm.ImageFile.SaveAsync(PathConstants.Product);
            }
            data.IsLeft = vm.Position switch
            {
                0 => null,
                -1 => true,
                1 => false
            };
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
