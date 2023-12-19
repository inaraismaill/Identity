using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewPustok.Contexts;
using NewPustok.ViewModels.SliderVM;

namespace Diana.ViewComponents;
public class SliderViewComponent:ViewComponent
{
    NewPustokDbContext _context { get; }

    public SliderViewComponent(NewPustokDbContext context)
    {
        _context = context;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        return View(await _context.Sliders.Select(s => new SliderItemVM
        {
            Id = s.Id,
            ImageUrl = s.ImageUrl,
            IsLeft = s.IsLeft,
            Description= s.Description,
            Title = s.Title,
        }).ToListAsync());
    }
}
