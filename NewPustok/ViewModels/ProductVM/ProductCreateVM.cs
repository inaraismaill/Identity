﻿using NewPustok.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewPustok.ViewModels.ProductVM;

public class ProductCreateVM
{
    [MaxLength(64)]
    public string Name { get; set; }
    [MaxLength(128)]
    public string? About { get; set; }
    public string? Description { get; set; }
    [Column(TypeName = "smallmoney")]
    public decimal SellPrice { get; set; }
    [Column(TypeName = "smallmoney")]
    public decimal CostPrice { get; set; }
    [Range(0, 100)]
    public float Discount { get; set; }
    public ushort Quantity { get; set; }
    public IFormFile ImageFileBack { get; set; }
    public IFormFile ImageFileFront { get; set; }
    public int CategoryId { get; set; }
}
