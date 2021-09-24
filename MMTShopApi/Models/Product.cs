using System.ComponentModel.DataAnnotations;

namespace MMTShopApi.Models
{
  public class Product
  {
    [Key]
    public int ID { get; set; }
    public int CatID { get; set; }
    [Required]
    [MaxLength(5), MinLength(5)]
    [RegularExpression("^[0-9]*$", ErrorMessage = "SKU Range must be numeric")]
    public string SKU { get; set; }
    [Required]
    [StringLength(30, ErrorMessage = "Name should be less than 30"), MinLength(3, ErrorMessage = "Name should be greater than 3")]
    public string Name { get; set; }
    public string Description { get; set; }
    [Range(1, 1000000, ErrorMessage = "Price must be between 1 and 1000000")]
    public double Price { get; set; }
    public virtual Category Category { get; set; }
  }

  public class ProductAdd
  {
    [Key]
    public int ID { get; set; }
    public int CatID { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
  }
}