using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MMTShopApi.Models
{
  public class Category
  {
    [Key]
    public int CatID { get; set; }
    [Required]
    [StringLength(30, ErrorMessage = "Name should be less than 30"), MinLength(3, ErrorMessage = "Name should be greater than 3")]
    public string Name { get; set; }

    [Required]
    [MaxLength(5), MinLength(5)]
    [RegularExpression("^[0-9]*$", ErrorMessage = "SKU Range must be numeric")]
    public string SkuValue { get; set; }
    public virtual IEnumerable<Product> Products { get; set; }
  }
}