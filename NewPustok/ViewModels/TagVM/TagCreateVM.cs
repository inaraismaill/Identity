using NewPustok.Models;

namespace NewPustok.ViewModels.TagVM
{
    public class TagCreateVM
    {
        public string Name { get; set; }
        public ICollection<BlogTag>? BlogTags { get; set; }
    }
}
