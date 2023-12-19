using System.ComponentModel.DataAnnotations;

namespace NewPustok.Models
{
    public class Tag
    {
        public int Id { get; set; }
        [MaxLength(64)]
        public string Name { get; set; }
        public ICollection<BlogTag>? BlogTags { get; set; }
    }
}
