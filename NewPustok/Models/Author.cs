using System.ComponentModel.DataAnnotations;

namespace NewPustok.Models
{
    public class Author
    {
        public int Id { get; set; }
        [Required,MaxLength(16,ErrorMessage ="Name must be shorter than 16")]
        public string Name { get; set; }

    }
}
