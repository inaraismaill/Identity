using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewPustok.Areas.Admin.ViewModels.ProductAVM;
using NewPustok.Contexts;
using NewPustok.Helpers;
using NewPustok.Models;
using NewPustok.ViewModels.ProductVM;

namespace NewPustok.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin, Admin, Moderator")]
    public class ProductController : Controller
    {
        NewPustokDbContext _db { get; }
        IWebHostEnvironment _env { get; }

        public ProductController(NewPustokDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        public IActionResult Index()
        {

            return View(_db.Products.Select(p => new AdminProductListItemVM
            {
                Id = p.Id,
                Name = p.Name,
                CostPrice = p.CostPrice,
                Discount = p.Discount,
                Category = p.Category,
                Description = p.Description,
                About = p.About,
                
                IsDeleted = p.IsDeleted,
                Quantity = p.Quantity,
                SellPrice = p.SellPrice,
                ImageBack=p.ImageBack,
                ImageFront=p.ImageFront
                
            }));
        }
        public IActionResult Create()
        { 
            ViewBag.Categorys = _db.Categorys;;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateVM vm)
        {
            if (vm.CostPrice > vm.SellPrice)
            {
                ModelState.AddModelError("CostPrice", "Sell price must be bigger than cost price");
            }
            if (!ModelState.IsValid)
            {
                ViewBag.Categorys = _db.Categorys;
                return View(vm);
            }
            if (!await _db.Categorys.AnyAsync(c => c.Id == vm.CategoryId))
            {
                ModelState.AddModelError("CategoryId", "Category doesnt exist");
                ViewBag.Categorys = _db.Categorys;
                return View(vm);
            }
            Product prod = new Product
            {
                Name = vm.Name,
                About = vm.About,
                Quantity = vm.Quantity,
                Description = vm.Description,
                Discount = vm.Discount,
               
                CostPrice = vm.CostPrice,
                SellPrice = vm.SellPrice,
                CategoryId = vm.CategoryId,

                ImageFront = await vm.ImageFileFront.SaveAsync(PathConstants.Product),
                ImageBack = await vm.ImageFileBack.SaveAsync(PathConstants.Product),
                
            };
            await _db.Products.AddAsync(prod);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();
            var data = await _db.Products.FindAsync(id);
            if (data == null) return RedirectToAction(nameof(Index));
            _db.Products.Remove(data);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();
            var data = await _db.Products.FindAsync(id);
            if (data == null) return RedirectToAction(nameof(Index));
            return View(new ProductUpdateVM
            {
                Name = data.Name,
                About = data.About,
                Quantity = data.Quantity,
                Description = data.Description,
                Discount = data.Discount,
                CostPrice = data.CostPrice,
                SellPrice = data.SellPrice,
                CategoryId = data.CategoryId,
            });
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, ProductUpdateVM vm)
        {
            if (id == null || id <= 0) return BadRequest();

            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var data = await _db.Products.FindAsync(id);
            if (data == null) return NotFound();

            if (vm.ImageFileBack != null && vm.ImageFileBack.Length > 0)
            {
                data.ImageBack = await vm.ImageFileBack.SaveAsync(PathConstants.Product);
            }

            data.Description = vm.Description;
            data.Name=vm.Name;
            data.About = vm.About;
            data.Quantity = vm.Quantity;
            data.Discount = vm.Discount;
            data.CostPrice = vm.CostPrice;
            data.SellPrice = vm.SellPrice;
            data.CategoryId = vm.CategoryId;
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
