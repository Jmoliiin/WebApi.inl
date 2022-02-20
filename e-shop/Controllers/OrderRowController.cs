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
using e_shop.Models.OrderRows;
using e_shop.Models.Order;
using e_shop.Filters;

namespace e_shop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderRowController : ControllerBase
    {
        private readonly SqlContext _context;

        public OrderRowController(SqlContext context)
        {
            _context = context;
        }


        /// <summary>
        /// GET: api/OrderRow
        /// </summary>
        /// <returns></returns>
        [UseApiKey]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderRowOutputModel>>> GetOrderRows()
        {
            var items = new List<OrderRowOutputModel>();

            foreach (var i in await _context.OrderRows.Include(x => x.Products).ThenInclude(x => x.ProductInventory).ToListAsync())
                items.Add(new OrderRowOutputModel(
                 i.Id,
                 i.OrdersId,
                 i.ProductsId,
                 i.Products.ProductName,
                 i.Quantity,
                 i.Products.Price
                    ));

            return items;
        }

        /// <summary>
        /// GET: api/OrderRow/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [UseApiKey]
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderRowOutputModel>> GetOrderRowEntity(int id)
        {
            //hämta order-inkludera status,kunder och deras adresser
            var orderRowEntity = await _context.OrderRows.Include(x => x.Products).FirstOrDefaultAsync(x => x.Id == id);

            if (orderRowEntity == null)
            {
                return NotFound(" OrderRow med Id inte funnen");
            }

            return new OrderRowOutputModel(
                orderRowEntity.Id,
                orderRowEntity.OrdersId,
                orderRowEntity.ProductsId,
                orderRowEntity.Products.ProductName,
                orderRowEntity.Quantity,
                orderRowEntity.Products.Price

                );
        }

        /// <summary>
        /// PUT: api/OrderRow/5
        /// Updatera orderrowens produkt och qvantitet.
        /// Ta bort totalpris i orderrow av föregående produkten.
        /// Lägg tillbaka produktens inventarie.
        /// Dra av totalpriset i ordern.
        /// Lägg till nya totalpriset i orderrow av nya valda produkten
        /// Dra av i produktens inventarie
        /// Lägg in orderrowens totalpris i orden
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        /// //key= kund ska kunna lägga orderrow
        [UseAdminApiKey]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrderRowEntity(int id, OrderRowUpdateModel model)
        {
            /*obs.ev kan man ta bort att man kan updatera product id och endast ändra quantitet i orderrow*/

            if (id != model.Id)
            {
                return BadRequest();
            }
            //Tar in gammla orderrow med egenskaper
            var orderRowEntity = await _context.OrderRows.Include(x => x.Products).ThenInclude(x=>x.ProductInventory).Include(x => x.Orders).FirstOrDefaultAsync(x => x.Id == model.Id);

            //ta bort förra totalpriset orderrow hade i orderns totalpris
            orderRowEntity.Orders.TotalPrice -= orderRowEntity.TotalPrice;

            //Lägg tillbaka inventory från förra produkten
            var resetQuantity = (orderRowEntity.Products.ProductInventory.Quantity + orderRowEntity.Quantity);
            var resetInventory = await _context.ProductInventorys.FirstOrDefaultAsync(x => x.Quantity == resetQuantity);
            if (resetInventory != null)
            {
                orderRowEntity.Products.ProductInventoryId = resetInventory.Id;

            }else
                orderRowEntity.Products.ProductInventory = new ProductInventoryEntity(resetQuantity);

            await _context.SaveChangesAsync();

            //Kontrollerar vald produkt id och lägger in rätt värden
            if (!await _context.Products.AnyAsync(x => x.Id == model.ProductsId))
                return BadRequest("Product id dont exist");

            var product = await _context.Products.Include(x => x.ProductInventory).FirstOrDefaultAsync(x => x.Id == model.ProductsId);
            var _totalprice = (product.Price * model.Quantity);


            //dra av nya quantity på vald product
            var quantity = (product.ProductInventory.Quantity - model.Quantity); //??????????????
            var Inventory = await _context.ProductInventorys.FirstOrDefaultAsync(x => x.Quantity == quantity);
            if (Inventory != null)
            {
                product.ProductInventoryId = Inventory.Id; 
            }
            else
                product.ProductInventory = new ProductInventoryEntity(quantity);

            //Uppdatera nya värden i orderrow
            orderRowEntity.ProductsId = model.ProductsId;
            orderRowEntity.Quantity = model.Quantity;
            orderRowEntity.TotalPrice = _totalprice;

            //Ändra totalpris i orden
            orderRowEntity.Orders.TotalPrice += orderRowEntity.TotalPrice;
          
            _context.Entry(orderRowEntity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderRowEntityExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        /// <summary>
        /// POST: api/OrderRow
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [UseAdminApiKey]
        [HttpPost]
        public async Task<ActionResult<OrderRowOutputModel>> PostOrderRowEntity(OrderRowInputModel model)
        {
            if(!await _context.Products.AnyAsync(x => x.Id == model.ProductId))
                return NotFound("Product id dont exist");
            if (!await _context.Orders.AnyAsync(x => x.Id == model.OrderID))
                return NotFound("Order Id dont exist");

            var product = await _context.Products.Include(x=>x.ProductInventory).FirstOrDefaultAsync(x=>x.Id == model.ProductId);
            var _totalprice = product.Price * model.Quantity;

            var orderRowEntity = new OrderRowEntity(model.Quantity,_totalprice, model.OrderID,model.ProductId);
            var order = await _context.Orders.FirstOrDefaultAsync(x=>x.Id==model.OrderID);

            order.TotalPrice += _totalprice;
            order.Modified = DateTime.Now;

            _context.Entry(order).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            //Ändra i product inventarie då produkter läggs i order
            var _newInventoryQuntity = (product.ProductInventory.Quantity - model.Quantity);
            var inventory = await _context.ProductInventorys.FirstOrDefaultAsync(x => x.Quantity == _newInventoryQuntity);

            //Om inventory matchar i databasen,Lägg in på befintligt id
            if (inventory!=null)
                product.ProductInventoryId = inventory.Id;
            else {
                //om inventory inte finns i databsen, lägg in en ny id i databas
                product.ProductInventory = new ProductInventoryEntity(_newInventoryQuntity);
            }

            _context.OrderRows.Add(orderRowEntity);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrderRowEntity", new { id = orderRowEntity.Id }, new OrderRowOutputModel(
                orderRowEntity.Id,
                orderRowEntity.OrdersId,
                orderRowEntity.ProductsId,
                orderRowEntity.Products.ProductName,
                orderRowEntity.Quantity,
                orderRowEntity.Products.Price  
                ));
        }

        /// <summary>
        /// DELETE: api/OrderRow/5      
        /// Tar bort orderrows totalpris i Order
        /// Lägger tillbaka vald kvantitet av produkten i inventarie
        /// Raderar orderRow från databas
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Ok()</returns>
        [UseAdminApiKey]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderRowEntity(int id)
        {
            //var orderRowEntity = await _context.OrderRows.FindAsync(id);

            //Tar in  orderrow med egenskaper
            var orderRowEntity = await _context.OrderRows.Include(x => x.Products).ThenInclude(x => x.ProductInventory).Include(x => x.Orders).FirstOrDefaultAsync(x => x.Id == id);

            
            if (orderRowEntity == null)
            {
                return NotFound();
            }
            //ta bort totalpriset orderrow har i orderns totalpris
            orderRowEntity.Orders.TotalPrice -= orderRowEntity.TotalPrice;

            //Lägg tillbaka inventory från  produkten
            var resetQuantity = (orderRowEntity.Products.ProductInventory.Quantity + orderRowEntity.Quantity);
            var resetInventory = await _context.ProductInventorys.FirstOrDefaultAsync(x => x.Quantity == resetQuantity);
            if (resetInventory != null)
            {
                orderRowEntity.Products.ProductInventoryId = resetInventory.Id;

            }
            else
                orderRowEntity.Products.ProductInventory = new ProductInventoryEntity(resetQuantity);

            _context.OrderRows.Remove(orderRowEntity);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool OrderRowEntityExists(int id)
        {
            return _context.OrderRows.Any(e => e.Id == id);
        }
    }
}
