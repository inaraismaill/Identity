using NewPustok.Models;
using System.ComponentModel.DataAnnotations;

namespace NewPustok.Models
{
    public class Blog
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime LastUpdatedAt { get; set; } = DateTime.Now;
        public int TagIds { get; set; }
        public ICollection<BlogTag>? BlogTags { get; set; }
    }
}