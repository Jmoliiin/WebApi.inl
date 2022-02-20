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
using e_shop.Models.Order;
using e_shop.Models.Helpers;
using e_shop.Models.OrderRows;
using e_shop.Filters;

namespace e_shop.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly SqlContext _context;

        public OrdersController(SqlContext context)
        {
            _context = context;
        }

        /// <summary>
        /// GET: api/Orders
        /// </summary>
        /// <returns></returns>
        [UseApiKey]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderOutputModel>>> GetOrders()
        {
            
            var items = new List<OrderOutputModel>();

            foreach (var i in await _context.Orders.Include(x => x.Status).Include(x => x.Costumers).ThenInclude(x => x.CostumersAddress).ToListAsync())
            {
                var items1 = new List<OrderAndListOfOrderRows>();
                foreach (var y in await _context.OrderRows.Where(x=>x.OrdersId==i.Id).Include(x => x.Products).ToListAsync())
                {
                    items1.Add(new OrderAndListOfOrderRows(y.Id, y.OrdersId, y.ProductsId, y.Products.ProductName, y.Quantity, y.Products.Price));
                    

                };
                items.Add(new OrderOutputModel(
                    i.Id,
                    i.TotalPrice,
                    i.Status.Id,
                    i.Status.StatusType,
                    i.Costumers.Id,
                    i.Created,
                    i.Modified,
                    new AddressModel(i.Costumers.CostumersAddress.StreetName, i.Costumers.CostumersAddress.PostalCode, i.Costumers.CostumersAddress.City, i.Costumers.CostumersAddress.Country),
                    items1
                    ));
            }
                
            return items;
        }

        /// <summary>
        /// GET: api/Orders/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [UseApiKey]
        public async Task<ActionResult<OrderOutputModel>> GetOrdersEntity(int id)
        {
            //hämta order-inkludera status,kunder och deras adresser
            var ordersEntity = await _context.Orders.Include(x => x.Status).Include(x => x.Costumers).ThenInclude(x => x.CostumersAddress).Include(x=>x.OrderRows).FirstOrDefaultAsync(x => x.Id == id);

            if (ordersEntity == null)
            {
                return NotFound(" Order med Id inte funnen");
            }
            var orderRowEntity = await _context.OrderRows.Where(x => x.OrdersId == id).Include(x => x.Products).ToListAsync();
            var items = new List<OrderAndListOfOrderRows>();
            foreach (var i in orderRowEntity)
                items.Add(new OrderAndListOfOrderRows(i.Id, i.OrdersId, i.ProductsId, i.Products.ProductName, i.Quantity, i.Products.Price));
            return new OrderOutputModel(
                ordersEntity.Id,
                ordersEntity.TotalPrice,
                ordersEntity.Status.Id,
                ordersEntity.Status.StatusType,
                ordersEntity.Costumers.Id,
                ordersEntity.Created,
                ordersEntity.Modified,
                new AddressModel(ordersEntity.Costumers.CostumersAddress.StreetName, ordersEntity.Costumers.CostumersAddress.PostalCode, ordersEntity.Costumers.CostumersAddress.City, ordersEntity.Costumers.CostumersAddress.Country),
                items //returnera lista
                );
        }

        /// <summary>
        /// PUT: api/Orders/5
        /// .Updatera orderns status
        ///.Kontrollera att Statusen på ordern inte är samma som innan
        ///.Om statusen har status 1.Ej klar går det endast ändra statusen om orderows finns på ordern
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [UseAdminApiKey]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrdersEntity(int id, OrderUpdateModel model)
        {
            if (id != model.Id)
            {
                return BadRequest($"Order with id: ${model.Id} do not exist");
            }
            //hämta in order & status för att kunna ändra statusen (Kan hämta in även costumer om man vill kunna byta ordens costumerId)
            //var ordersEntity = await _context.Orders.Include(x => x.Costumers).ThenInclude(x => x.CostumersAddress).Include(x => x.status).FirstOrDefaultAsync(x => x.Id == model.Id);

            var ordersEntity = await _context.Orders.Include(x => x.Status).Include(x => x.OrderRows).FirstOrDefaultAsync(x => x.Id == model.Id);
            //kontrollera om ordens status är oförändrad
            if (ordersEntity.Status.Id == model.StatusId)
                return BadRequest($"The order already have status: {ordersEntity.Status.Id}.{ordersEntity.Status.StatusType}");
            //kontrollera om ordern har orderows
            if (ordersEntity.StatusId == 1)
                if (ordersEntity.OrderRows.Count <= 0)
                    return BadRequest($"Order needs orderrows to update status");

            ordersEntity.StatusId = model.StatusId;
            ordersEntity.Modified=DateTime.Now;

            _context.Entry(ordersEntity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrdersEntityExists(id))
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
        /// POST: api/Orders
        /// Skapa en order
        /// Kontrollera att CostumerId som ska kopplas till Order, finns i databasen.
        /// Kontrollera om det finns en Order som har status 1.Påbörjad dvs ej inskickad order.Skapar då ej ny order, utan orderows lägg på befintlig order
        /// Kontroll så statusId finns i databsen
        /// </summary>
        /// <param name="ordersEntity"></param>
        /// <returns></returns>
        [UseAdminApiKey]
        [HttpPost]
        public async Task<ActionResult<OrderOutputModel>> PostOrdersEntity(OrderInputModel model)
        {
            //Hårdkodar status tillfälligt ,då funktionen under orderController hanteras rätt.
            //Det ska ej gå att göra ny order när statusId är id=1.dvs när ordern inte är inskickad än till behandling.

            
            var s1 = new StatusEntity { StatusType = "Påbörjad" };
            var s2 = new StatusEntity { StatusType = "Behandlas" };
            var s3 = new StatusEntity { StatusType = "Skickad" };

            var statusExist = await _context.Statuses.FirstOrDefaultAsync(x => x.StatusType == "Påbörjad");
            if (statusExist == null)
            {
                _context.Statuses.Add(s1);
                _context.Statuses.Add(s2);
                _context.Statuses.Add(s3);
                await _context.SaveChangesAsync();
            }
            

            var status = await _context.Statuses.FirstOrDefaultAsync(x => x.Id == model.StatusId);
            var costumer = _context.Costumers.Include(x => x.CostumersAddress).FirstOrDefault(x => x.Id == model.CostumersId);
            var order = await _context.Orders.Where(x => x.StatusId == model.StatusId).FirstOrDefaultAsync(x => x.CostumersId == model.CostumersId);
            
            //Kontrollera om costumer finns
            if (costumer == null)
                return NotFound("CostumerId not exist");
            //Kolla om det finns en order på kunden
            if (order != null)
                //om det finns en order som är pågående och ej inlämnad än.tex en kundvangn med pågående shopping?? (hittar på här,vet ej hur kundvagn funkar)
                if (order.StatusId == 1)
                    return BadRequest("Påbörjad order finns redan på kunden");

            var ordersEntity = new OrdersEntity(model.CostumersId, model.StatusId,DateTime.Now);

            //konstrollera om status finns
            if (status == null) 
                return BadRequest("Status finns inte");

            _context.Orders.Add(ordersEntity);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrdersEntity", new { id = ordersEntity.Id }, new OrderOutputModel(
                ordersEntity.Id,
                ordersEntity.TotalPrice,
                ordersEntity.Status.Id,
                ordersEntity.Status.StatusType,
                ordersEntity.Costumers.Id,
                ordersEntity.Created,
                ordersEntity.Modified,
                new AddressModel(ordersEntity.Costumers.CostumersAddress.StreetName, ordersEntity.Costumers.CostumersAddress.PostalCode, ordersEntity.Costumers.CostumersAddress.City, ordersEntity.Costumers.CostumersAddress.Country)
                ));
        }

        /// <summary>
        /// DELETE: api/Orders/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [UseAdminApiKey]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrdersEntity(int id)
        {
            var ordersEntity = await _context.Orders.FindAsync(id);
            if (ordersEntity == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(ordersEntity);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrdersEntityExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
