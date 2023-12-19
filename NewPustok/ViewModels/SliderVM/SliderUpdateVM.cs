using System.ComponentModel.DataAnnotations;

namespace NewPustok.ViewModels.SliderVM
{
    public class SliderUpdateVM
    {
        public IFormFile ImageFile { get; set; }
        [Required, DataType("nvarchar"), MinLength(3, ErrorMessage = "The length of the title is less than 3"), MaxLength(50, ErrorMessage = "The length of the title is more than 20")]
        public string Title { get; set; }
        [Required, DataType("nvarchar"), MinLength(5, ErrorMessage = "The length of the Description is less than 5"), MaxLength(100, ErrorMessage = "The length of the Description is more than 60")]
        public string Description { get; set; }
        [Required]
        public sbyte Position { get; set; }
    }
}
