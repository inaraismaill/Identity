using NewPustok.Models;

namespace NewPustok.ViewModels.BlogVM
{
    public class BlogUpdateVM
    {
        public string ImageUrl { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime LastUpdatedAt { get; set; } = DateTime.Now;
        public IEnumerable<int>? TagId { get; set; }
    }
}
