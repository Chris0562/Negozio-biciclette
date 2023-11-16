using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Betacomio.Models;
using Betacomio.Paginators;
using NLog.Fluent;

namespace Betacomio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressesController : ControllerBase
    {
        private readonly AdventureWorksLt2019Context _context;
        private static readonly NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();

        public AddressesController(AdventureWorksLt2019Context context)
        {
            _context = context;
        }

      
        /// <summary>
        /// GET: api/Addresses  UTILIZZATO PER MOSTRARE NLog  (SWAGGER error log)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Address>>> GetAddresses()
        {
            try
            {
                int x = 1;
                Console.WriteLine(x/0);
                if (_context.Addresses == null)
                {
                    return NotFound();
                }
                return await _context.Addresses.ToListAsync();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                Console.WriteLine(ex.Message);
                throw;
            }

        }

       
        /// <summary>
        /// GET: api/Addresses/5 PRENDE ADDRESS UTENTE LOGGATO
        /// </summary>
        /// <param name="addressID"></param>
        /// <returns></returns>
        [HttpGet("{addressID}")]
        public async Task<ActionResult<Address>> GetAddressByID(int addressID)
        {

            try
            {
                if (_context.Addresses == null)
                {
                    return NotFound();
                }
                var address = await _context.Addresses.FindAsync(addressID);

                if (address == null)
                {
                    return NotFound();
                }

                return address;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                Console.WriteLine(ex.Message);
                throw;
            }


        }


        /// <summary>
        /// MODIFICA ADDRESS UTENTE LOGGATO
        /// </summary>
        /// <param name="id"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<Address>> PutAddress(int id, Address address)
        {
            if (id != address.AddressId)
            {
                return BadRequest();
            }

            _context.Entry(address).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                log.Error(ex);
                if (!AddressExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return address;
        }


        /// <summary>
        /// POSTA ADDRESS NUOVO UTENTE
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Address>> PostAddress(Address address)
        {
             // guardare commento in customerAddressController [post]
            try
            {
                if (_context.Addresses == null)
                {
                    return Problem("Entity set 'AdventureWorksLt2019Context.Addresses'  is null.");
                }
                _context.Addresses.Add(address);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                Console.WriteLine(ex.Message);
                throw;
            }



            return address;
        }


        // DELETE: api/Addresses/5
        /// <summary>
        /// DELETE ADDRESS USER, NON UTILIZZATO
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAddress(int id)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (_context.Addresses == null)
                    {
                        return NotFound();
                    }
                    var address = await _context.Addresses.FindAsync(id);
                    if (address == null)
                    {
                        return NotFound();
                    }

                    _context.Addresses.Remove(address);
                    await _context.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    Console.WriteLine(ex.Message);
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception ex2)
                    {
                        Console.WriteLine(ex2.GetType());
                        Console.WriteLine(ex2.Message);
                        log.Error(ex2.Message);
                    }
                }

                return NoContent();
            }
          
        }


        private bool AddressExists(int id)
        {
            return (_context.Addresses?.Any(e => e.AddressId == id)).GetValueOrDefault();
        }
    }
}
