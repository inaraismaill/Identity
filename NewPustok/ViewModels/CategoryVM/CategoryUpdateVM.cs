using System.ComponentModel.DataAnnotations;

namespace NewPustok.ViewModels.SliderVM
{
    public class CategoryUpdateVM
    {
        [Required, MaxLength(16)]
        public string Name { get; set; }
    }
}
