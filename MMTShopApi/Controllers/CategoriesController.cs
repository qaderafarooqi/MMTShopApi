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
  public class CategoriesController : ApiController
  {
    private MMTShopDbContext db = new MMTShopDbContext();

    /// <summary>
    /// Show list of Categories
    /// </summary>
    /// <returns></returns>
    // GET: api/Categories
    public IQueryable<Category> GetCategories()
    {
      return db.Categories;
    }

    // GET: api/Categories/5
    /// <summary>
    /// Show a specific category
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [ResponseType(typeof(Category))]
    public async Task<IHttpActionResult> GetCategory(int id)
    {
      Category category = await db.Categories.FindAsync(id);
      if (category == null)
      {
        return NotFound();
      }

      return Ok(category);
    }

    // PUT: api/Categories/5
    /// <summary>
    /// Edit Category
    /// </summary>
    /// <param name="id"></param>
    /// <param name="category"></param>
    /// <returns></returns>
    [ResponseType(typeof(void))]
    public async Task<IHttpActionResult> PutCategory(int id, Category category)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      if (id != category.CatID)
      {
        return BadRequest();
      }

      //existance check
      var skuCheck = db.Categories.Where(w => w.CatID != id && w.SkuValue == category.SkuValue).Count();
      if (skuCheck != 0)
      {
        throw new Exception("SKU value already assigned to an other category");
      }

      db.Entry(category).State = EntityState.Modified;

      try
      {
        await db.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!CategoryExists(id))
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

    // POST: api/Categories
    /// <summary>
    /// Add new category
    /// </summary>
    /// <param name="category"></param>
    /// <returns></returns>
    [ResponseType(typeof(Category))]
    public async Task<IHttpActionResult> PostCategory(Category category)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }
      var skuCheck = db.Categories.Where(w => w.SkuValue == category.SkuValue).Count();
      if (skuCheck != 0)
      {
        throw new Exception("SKU value already assigned to an other category");
      }

      db.Categories.Add(category);
      await db.SaveChangesAsync();

      return CreatedAtRoute("DefaultApi", new { id = category.CatID }, category);
    }

    // DELETE: api/Categories/5
    /// <summary>
    /// Delete a specific category
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [ResponseType(typeof(Category))]
    public async Task<IHttpActionResult> DeleteCategory(int id)
    {
      Category category = await db.Categories.FindAsync(id);
      if (category == null)
      {
        return NotFound();
      }

      db.Categories.Remove(category);
      await db.SaveChangesAsync();

      return Ok(category);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        db.Dispose();
      }
      base.Dispose(disposing);
    }

    private bool CategoryExists(int id)
    {
      return db.Categories.Count(e => e.CatID == id) > 0;
    }
  }
}

