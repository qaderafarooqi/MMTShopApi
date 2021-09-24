using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using MMTShopApi.Models;

namespace MMTShopApi.Controllers
{
    public class ProductsController : ApiController
    {
        private MMTShopDbContext db = new MMTShopDbContext();

    // GET: api/Products
    /// <summary>
    /// GET LIST OF PRODUCTS
    /// </summary>
    /// <returns></returns>
    public IQueryable<Product> GetProducts()
    {
      return db.Products;
    }
    // GET: api/ProductsCat
    /// <summary>
    /// Get list of product having a specific category
    /// </summary>
    /// <param name="categoryid"></param>
    /// <returns></returns>

    public List<Product> GetProductsCat(int categoryid)
    {
      var productlist = db.Products.Where(c => c.CatID == categoryid).ToList();
      return productlist;
    }

    // GET: api/Products/5
    /// <summary>
    /// GET INFORMATION OF A PRODUCT
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [ResponseType(typeof(Product))]
    public async Task<IHttpActionResult> GetProduct(int id)
    {
      Product product = await db.Products.FindAsync(id);
      if (product == null)
      {
        return NotFound();
      }

      return Ok(product);
    }

    // PUT: api/Products/5
    /// <summary>
    /// EDIT A PRODUCT
    /// </summary>
    /// <param name="id"></param>
    /// <param name="product"></param>
    /// <returns></returns>
    [ResponseType(typeof(void))]
    public async Task<IHttpActionResult> PutProduct(int id, ProductAdd productadd)
    {
      Product product = new Product();
      product.ID = productadd.ID;
      product.CatID = productadd.CatID;
      product.Name = productadd.Name;
      product.Description = productadd.Description;
      product.Price = productadd.Price;

      //existance check
      var skuCheck = db.Products.Where(w => w.ID == id && w.CatID != productadd.CatID).Count();
      product.SKU = NextSKUNumber(productadd.CatID);

      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      if (id != product.ID)
      {
        return BadRequest();
      }



      db.Entry(product).State = EntityState.Modified;

      try
      {
        await db.SaveChangesAsync();
      }

      catch (DbUpdateConcurrencyException)
      {
        if (!ProductExists(id))
        {
          return NotFound();
        }
        else
        {
          throw;
        }
      }

      return StatusCode(HttpStatusCode.NoContent);
    }

    // POST: api/Products
    /// <summary>
    /// ADD A PRODUCT
    /// </summary>
    /// <param name="product"></param>
    /// <returns></returns>
    [ResponseType(typeof(Product))]
    public async Task<IHttpActionResult> PostProduct(ProductAdd productadd)
    {
      Product product = new Product();
      product.ID = productadd.ID;
      product.CatID = productadd.CatID;
      string nextskunumber = NextSKUNumber(productadd.CatID);
      product.SKU = nextskunumber;
      product.Name = productadd.Name;
      product.Description = productadd.Description;
      product.Price = productadd.Price;

      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      db.Products.Add(product);
      await db.SaveChangesAsync();

      return CreatedAtRoute("DefaultApi", new { id = product.ID }, product);
    }

    // DELETE: api/Products/5
    /// <summary>
    /// DELETE A PRODUCT
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [ResponseType(typeof(Product))]
    public async Task<IHttpActionResult> DeleteProduct(int id)
    {
      Product product = await db.Products.FindAsync(id);
      if (product == null)
      {
        return NotFound();
      }

      db.Products.Remove(product);
      await db.SaveChangesAsync();

      return Ok(product);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        db.Dispose();
      }
      base.Dispose(disposing);
    }

    private bool ProductExists(int id)
    {
      return db.Products.Count(e => e.ID == id) > 0;
    }

    private string NextSKUNumber(int catid)
    {
      string currentNumber = db.Categories.Where(c => c.CatID == catid).Select(c => c.SkuValue).FirstOrDefault();
      return Convert.ToString(Convert.ToInt16(currentNumber) + 1);
    }
  }
}