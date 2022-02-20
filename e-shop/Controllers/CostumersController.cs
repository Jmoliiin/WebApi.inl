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
using e_shop.Models.Costumer;
using e_shop.Filters;

namespace e_shop.Controllers
{
    [UseAdminApiKey]
    [Route("api/[controller]")]
    [ApiController]
    public class CostumersController : ControllerBase
    {
        private readonly SqlContext _context;

        public CostumersController(SqlContext context)
        {
            _context = context;
        }
        /// <summary>
        /// GET: api/Costumers
        /// </summary>
        /// <returns></returns>
        [UseApiKey]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CostumerOutputModel>>> GetCostumers()
        {
            var items = new List<CostumerOutputModel>();
            foreach (var i in await _context.Costumers.Include(x => x.CostumersAddress).ToListAsync())
                items.Add(new CostumerOutputModel(
                    i.Id,
                    i.FirstName,
                    i.LastName,
                    i.Email,
                    i.CostumersAddress.StreetName,
                    i.CostumersAddress.PostalCode,
                    i.CostumersAddress.City,
                    i.CostumersAddress.Country
                ));

            return items;
        }

        /// <summary>
        /// GET: api/Costumers/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [UseApiKey]
        [HttpGet("{id}")]
        public async Task<ActionResult<CostumerOutputModel>> GetCostumersEntity(int id)
        {
            var costumerEntity = await _context.Costumers.Include(x => x.CostumersAddress).FirstOrDefaultAsync(x => x.Id == id);

            if (costumerEntity == null)
            {
                return NotFound("Costumer med Id inte funnen");
            }

            return new CostumerOutputModel(
                    costumerEntity.Id,
                    costumerEntity.FirstName,
                    costumerEntity.LastName,
                    costumerEntity.Email,
                    costumerEntity.CostumersAddress.StreetName,
                    costumerEntity.CostumersAddress.PostalCode,
                    costumerEntity.CostumersAddress.City,
                    costumerEntity.CostumersAddress.Country
                );
        }

        /// <summary>
        /// PUT: api/Costumers/5
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [UseAdminApiKey]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCostumersEntity(int id, CostumerUpdateModel model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }


            var costumerEntity = await _context.Costumers.Include(x => x.CostumersAddress).FirstOrDefaultAsync(x => x.Id == model.Id);
            var address = await _context.CostumersAddresses.FirstOrDefaultAsync(x => x.StreetName == model.StreetName && x.PostalCode == model.PostalCode && x.City == model.City && x.Country == model.Country);

            //Om adress matchar i databasen,Lägg in på befintligt id
            if (address != null)
                costumerEntity.CostumersAddressId = address.Id;
            //om adress inte finns i databasen, lägg in ny
            else 
                costumerEntity.CostumersAddress = new CostumersAddressEntity(model.StreetName, model.PostalCode, model.City, model.Country);

            costumerEntity.FirstName = model.FirstName;
            costumerEntity.LastName = model.LastName;
            
            //costumerEntity.Email = model.Email;
            /*Kan även lägga in email men tänker att den bör ske separat om man bygger vidare på projektet. 
             * Kontrollera då så användarens inloggade kontos email = kontroll av befintlig email. 
             * Sen kontrollera nya email att den inte redan finns i databas och sedan radera gammla email i databasen och lägga dit nya?
             */
            


            _context.Entry(costumerEntity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CostumersEntityExists(id))
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
        /// POST: api/Costumers
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [UseAdminApiKey]
        [HttpPost]
        public async Task<ActionResult<CostumersEntity>> PostCostumersEntity(CostumerInputModel model)
        {
            // Om email finns registrerad
            if (await _context.Costumers.AnyAsync(x => x.Email == model.Email))
                return BadRequest("Email finns redan");

            var costumerEntity = new CostumersEntity(model.FirstName, model.LastName, model.Email);
            var address = await _context.CostumersAddresses.FirstOrDefaultAsync(x => x.StreetName == model.StreetName && x.PostalCode == model.PostalCode && x.City == model.City && x.Country == model.Country);
            var password = await _context.PassWords.FirstOrDefaultAsync(x => x.Password == model.Password);

            //Om Lösenord matchar i databasen,Lägg in på befintligt id
            if (password != null)
                costumerEntity.PassWordsId = password.Id;
            else
                //om lösenord inte finns i databsen, lägg in en ny id i databas
                costumerEntity.PassWords = new PassWordsEntity(model.Password);
            if (address != null)
                //lägg in i befintlig databas adress
                costumerEntity.CostumersAddressId = address.Id;
            //Skapa adress om den inte finns i databasen
            else
                costumerEntity.CostumersAddress = new CostumersAddressEntity(model.StreetName, model.PostalCode, model.City, model.Country);

            //Lägg till costumer
            _context.Costumers.Add(costumerEntity);
            await _context.SaveChangesAsync();

            //returnera 
            return CreatedAtAction("GetCostumers", new { id = costumerEntity.Id }, new CostumerOutputModel(
                costumerEntity.Id,
                costumerEntity.FirstName,
                costumerEntity.LastName,
                costumerEntity.Email,
                costumerEntity.CostumersAddress.StreetName,
                costumerEntity.CostumersAddress.PostalCode,
                costumerEntity.CostumersAddress.City,
                costumerEntity.CostumersAddress.Country

                ));
        }

        /// <summary>
        /// DELETE: api/Costumers/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [UseAdminApiKey]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCostumersEntity(int id)
        {
            var costumersEntity = await _context.Costumers.FindAsync(id);
            if (costumersEntity == null)
            {
                return NotFound();
            }
            costumersEntity.FirstName = "";
            costumersEntity.LastName = "";
            costumersEntity.Email = "";

            _context.Entry(costumersEntity).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CostumersEntityExists(int id)
        {
            return _context.Costumers.Any(e => e.Id == id);
        }
    }
}
