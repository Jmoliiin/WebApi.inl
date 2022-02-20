#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using e_shop.Data;
using e_shop.Entities;
using e_shop.Models.Category;
using e_shop.Filters;

namespace e_shop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly SqlContext _context;

        public CategoryController(SqlContext context)
        {
            _context = context;
        }
        /// <summary>
        /// GET: api/Category
        /// Skapar en lista. Forech loop som För varje object i databasen läggs categorynId & categoryName till i listan
        /// </summary>
        /// <returns>returnerar listan </returns>
        [UseApiKey]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryOutputModel>>> GetCategories()
        {
            var items = new List<CategoryOutputModel>();
            foreach (var i in await _context.Categories.OrderBy(x=>x.Id).ToListAsync())
                items.Add(new CategoryOutputModel(i.Id, i.CategoryName));

            return items;
        }

        /// <summary>
        /// GET: api/Category/5
        /// Söker efter en categoryId i databasen
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returnerar en model med id och categoryname</returns>
        [UseApiKey]
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryOutputModel>> GetCategoryEntity(int id)
        {
            var categoryEntity = await _context.Categories.FindAsync(id); //Leta efter CategoryId i databasen
            if (categoryEntity == null)
            {
                return NotFound("Catagory med Id inte funnen");
            }

            return new CategoryOutputModel(
                    categoryEntity.Id,
                    categoryEntity.CategoryName
                ); ;
        }

        /// <summary>
        /// PUT: api/Category/5
        /// Updaterar categoryname i databasen
        /// </summary>
        /// <param name="id"></param>
        /// <param name="categoryEntity"></param>
        /// <returns>Returns no content</returns>
        [UseAdminApiKey]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategoryEntity(int id, CategoryOutputModel model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }
            var categoryEntity = await _context.Categories.FindAsync(model.Id);
            //kontrollera om categori namnet redan finns
            if (await _context.Categories.AnyAsync(x => x.CategoryName == model.CategoryName))
                return BadRequest("Kategorin finns redan");

            categoryEntity.CategoryName = model.CategoryName;
            _context.Entry(categoryEntity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryEntityExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// POST: api/Category
        /// </summary>
        /// <param name="categoryEntity"></param>
        /// <returns></returns>
        [UseAdminApiKey]
        [HttpPost]
        public async Task<ActionResult<CategoryOutputModel>> PostCategoryEntity(CategoryInputModel model)
        {
            if (await _context.Categories.AnyAsync(x => x.CategoryName == model.CategoryName))
                return BadRequest("Kategorin finns redan");
            var categoryEntity = new CategoryEntity(model.CategoryName);
            //Lägg in category i databasen
            _context.Categories.Add(categoryEntity);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCategoryEntity", new { id = categoryEntity.Id }, new CategoryOutputModel(categoryEntity.Id, categoryEntity.CategoryName));

        }

        /// <summary>
        /// DELETE: api/Category/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [UseAdminApiKey]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoryEntity(int id)
        {
            var categoryEntity = await _context.Categories.FindAsync(id);
            if (categoryEntity == null) 
            {
                return NotFound();
            }
            categoryEntity.CategoryName = "";
            _context.Entry(categoryEntity).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CategoryEntityExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }
    }
}
