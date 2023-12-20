using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewPustok.Areas.Admin.ViewModels.ProductAVM;
using NewPustok.Contexts;
using NewPustok.ViewModels.SliderVM;

namespace NewPustok.Controllers
{
    public class HomeController : Controller
    {
        NewPustokDbContext _db { get; }
        public HomeController(NewPustokDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {

            HomeVM vm = new HomeVM
            {
                
                Products = await _db.Products.Where(p => !p.IsDeleted).Select(p => new AdminProductListItemVM
                {
                    Id = p.Id,
                    Category = p.Category,
                    Discount = p.Discount,
                    Name = p.Name,
                    ImageFront = p.ImageFront,
                    ImageBack = p.ImageBack,
                    IsDeleted = p.IsDeleted,
                    CostPrice = p.CostPrice,
                    Quantity = p.Quantity,
                    SellPrice = p.SellPrice
                }).ToListAsync()
            };
            return View(vm);
        }
        public IActionResult AccessDenied()
        {
            return View();
        }

    } 
}