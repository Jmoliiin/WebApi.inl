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
using e_shop.Models.Product;
using e_shop.Filters;

namespace e_shop.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly SqlContext _context;

        public ProductsController(SqlContext context)
        {
            _context = context;
        }
        /// <summary>
        /// GET: api/Products
        /// </summary>
        /// <returns>List of products</returns>
        [UseApiKey]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductOutputModel>>> GetProducts()
        {
            var items = new List<ProductOutputModel>();

            foreach (var i in await _context.Products.Include(x => x.ProductInventory).Include(x => x.Category).ToListAsync())
                items.Add(new ProductOutputModel(
                    i.Id,
                    i.ArticleNumber,
                    i.ProductName,
                    i.Description,
                    i.Price,
                    i.Category.Id,
                    i.Category.CategoryName,
                    i.ProductInventory.Id,
                    i.ProductInventory.Quantity,
                    i.Created,
                    i.Modified
                ));
            return items;
        }

        /// <summary>
        /// GET: api/Products/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [UseApiKey]
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductOutputModel>> GetProductsEntity(int id)
        {
            var productEntity = await _context.Products.Include(x => x.Category).Include(x => x.ProductInventory).FirstOrDefaultAsync(x => x.Id == id);

            if (productEntity == null)
            {
                return NotFound("Product med Id inte funnen");
            }

            return new ProductOutputModel(
                    productEntity.Id,
                    productEntity.ArticleNumber,
                    productEntity.ProductName,
                    productEntity.Description,
                    productEntity.Price,
                    productEntity.Category.Id,
                    productEntity.Category.CategoryName,
                    productEntity.ProductInventory.Id,
                    productEntity.ProductInventory.Quantity,
                    productEntity.Created,
                    productEntity.Modified

                );
        }

        /// <summary>
        /// PUT: api/Products/5
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [UseAdminApiKey]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductsEntity(int id, ProductUpdateModel model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }
            var productEntity = await _context.Products.Include(x => x.ProductInventory).Include(x => x.Category).FirstOrDefaultAsync(x => x.Id == model.Id);

            var inventory = await _context.ProductInventorys.FirstOrDefaultAsync(x => x.Quantity == model.InventoryQuantity);
            var category = await _context.Categories.FirstOrDefaultAsync(x => x.CategoryName == model.CategoryName);

            //Om inventory matchar i databasen,Lägg in på befintligt id
            if (inventory != null)
                productEntity.ProductInventoryId = inventory.Id;
            else
                //om inventory inte finns i databsen, lägg in en ny id i databas
                productEntity.ProductInventory = new ProductInventoryEntity(model.InventoryQuantity);
            if (category != null)
                //lägg in i befintlig databas category
                productEntity.CategoryId = category.Id;
            else 
                return BadRequest("Kategorin du vill lägga produkten under existerar inte i databsen.");
            
            //Kan lägga in här så man kan byta artikelnummer
            productEntity.ProductName = model.ProductName;
            productEntity.Description = model.Description;
            productEntity.Price = model.Price;
            productEntity.Category.CategoryName = model.CategoryName;
            productEntity.ProductInventory.Quantity = model.InventoryQuantity;
            productEntity.Modified = DateTime.Now;



            _context.Entry(productEntity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductsEntityExists(id))
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
        /// POST: api/Products
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [UseAdminApiKey]
        [HttpPost]
        public async Task<ActionResult<ProductOutputModel>> PostProductsEntity(ProductInputModel model)
        {
            if (await _context.Products.AnyAsync(x => x.ArticleNumber == model.ArticleNumber))
                return BadRequest("Artikelnummer finns redan registrerad");
            if (await _context.Products.AnyAsync(x => x.ProductName == model.ProductName))
                return BadRequest("Product name finns redan registrerad");
            if (!await _context.Categories.AnyAsync(x => x.CategoryName == model.CategoryName))
                return BadRequest("Categorin finns inte");

            var productEntity = new ProductsEntity(model.ArticleNumber, model.ProductName, model.Description, model.Price, DateTime.Now);
            var inventory = await _context.ProductInventorys.FirstOrDefaultAsync(x => x.Quantity == model.Quantity);
            var category = await _context.Categories.FirstOrDefaultAsync(x => x.CategoryName == model.CategoryName);

            //Om inventory matchar i databasen,Lägg in på befintligt id
            if (inventory != null)
                productEntity.ProductInventoryId = inventory.Id;
            else
                //om inventory inte finns i databsen, lägg in en ny id i databas
                productEntity.ProductInventory = new ProductInventoryEntity(model.Quantity);
            if (category != null)
                //lägg in i befintlig databas category
                productEntity.CategoryId = category.Id;

            //Lägg till product
            _context.Products.Add(productEntity);
            await _context.SaveChangesAsync();

            //, productsEntity);
            return CreatedAtAction("GetProductsEntity", new { id = productEntity.Id }, new ProductOutputModel(
                productEntity.Id,
                productEntity.ArticleNumber,
                productEntity.ProductName,
                productEntity.Description,
                productEntity.Price,
                productEntity.Category.Id,
                productEntity.Category.CategoryName,
                productEntity.ProductInventory.Id,
                productEntity.ProductInventory.Quantity,
                productEntity.Created

                ));
        }

        /// <summary>
        /// DELETE: api/Products/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [UseAdminApiKey]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductsEntity(int id)
        {
            var productsEntity = await _context.Products.FindAsync(id);
            if (productsEntity == null)
            {
                return NotFound();
            }

            _context.Products.Remove(productsEntity);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductsEntityExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
