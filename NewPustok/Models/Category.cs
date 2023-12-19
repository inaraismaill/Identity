using System.ComponentModel.DataAnnotations;
 
namespace NewPustok.Models;

public class Category
{
    public int Id { get; set; }
    [Required,MaxLength(16)]
    public string Name { get; set; }
    
}
