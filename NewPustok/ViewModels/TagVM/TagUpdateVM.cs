using NewPustok.Models;

namespace NewPustok.ViewModels.TagVM
{
    public class TagUpdateVM
    {
        public string Name { get; set; }
        public ICollection<BlogTag>? BlogTags { get; set; }
    }
}
