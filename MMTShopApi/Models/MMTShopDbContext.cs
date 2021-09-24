

using System.Data.Entity;

namespace MMTShopApi.Models
{
  public class MMTShopDbContext : DbContext
  {
    public MMTShopDbContext() : base("MMTShopConStr")
    {
    }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
  }
}