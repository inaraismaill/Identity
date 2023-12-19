using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using NewPustok.Models;
using System.Drawing;

namespace NewPustok.Areas.Admin.ViewModels.ProductAVM
{
    public class AdminProductListItemVM
    {
        public int Id { get; set; }
        [MaxLength(64)]
        public string Name { get; set; }
        public string? About { get; set; }
        public string? Description { get; set; }
        [Column(TypeName = "smallmoney")]
        public decimal SellPrice { get; set; }
        [Column(TypeName = "smallmoney")]
        public decimal CostPrice { get; set; }
        [Range(0, 100)]
        public float Discount { get; set; }
        public ushort Quantity { get; set; }
        public string ImageFront { get; set; }
        public string ImageBack { get; set; }
        public Category? Category { get; set; }
        public bool IsDeleted { get; set; }
    }
}