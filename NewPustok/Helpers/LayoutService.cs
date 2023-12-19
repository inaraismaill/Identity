using NewPustok.Contexts;
using NewPustok.Models;

namespace NewPustok.Helpers
{
    public class LayoutService
    {
        NewPustokDbContext _context { get; }

        public LayoutService(NewPustokDbContext context)
        {
            _context = context;
        }

        public async Task<Settings> GetSettingsAsync()
            => await _context.Settings.FindAsync(1);
    }
}
