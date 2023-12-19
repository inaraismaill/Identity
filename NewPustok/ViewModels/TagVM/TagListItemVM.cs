using NewPustok.Models;
using System.ComponentModel.DataAnnotations;

namespace NewPustok.ViewModels.TagVM
{
    public class TagListItemVM
    {
        public int Id { get; set; }
        [MaxLength(64)]
        public string Name { get; set; }
        public ICollection<BlogTag>? BlogTags { get; set; }
    }
}
