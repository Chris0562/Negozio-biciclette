using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Betacomio.Models;

namespace Betacomio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerAddressesController : ControllerBase
    {
        private readonly AdventureWorksLt2019Context _context;
        private static readonly NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();

        public CustomerAddressesController(AdventureWorksLt2019Context context)
        {
            _context = context;
        }

        /// <summary>
        /// GET: api/Addresses  non utilizzato getAll per SWAGGER
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerAddress>>> GetAddresses()
        {
            try
            {
                if (_context.CustomerAddresses == null)
                {
                    return NotFound();
                }
                return await _context.CustomerAddresses.ToListAsync();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                Console.WriteLine(ex.Message);
                throw;
            }

        }


        /// <summary>
        /// abbiamo gestito 2 get da front end, avremmo dovuto fare un solo get da front end, e gestire i due 2 get da backend, possibilmente 
        /// in un solo controller, questo metodo recupera solo addressType
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        [HttpGet("{customerID}")]
        public async Task<ActionResult<CustomerAddress>> GetAddress(int customerID)
        {
            try
            {
                if (_context.Addresses == null)
                {
                    return NotFound();
                }
                var address = await _context.CustomerAddresses.Include(ca => ca.Address).Where(a => a.CustomerId.Equals(customerID)).FirstOrDefaultAsync();

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
        /// POSTA ADDRESS NUOVO UTENTE
        /// abbiamo gestito 2 post da front end, avremmo dovuto fare un solo post da front end, e gestire i due 2 post da backend, possibilmente 
        /// in un solo controller, cosi da poter applicare una transaction che prevenga post una sola tabella
        /// </summary>
        /// <param name="customerAddress"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<CustomerAddress>> PostCustomerAddress(CustomerAddress customerAddress)
        {            
            try
            {
                if (_context.CustomerAddresses == null)
                {
                    return Problem("Entity set 'AdventureWorksLt2019Context.CustomerAddresses'  is null.");
                }
                _context.CustomerAddresses.Add(customerAddress);
                await _context.SaveChangesAsync();

                return customerAddress;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                Console.WriteLine(ex.Message);
                throw;
            }

        }


        /// <summary>
        /// abbiamo gestito 2 post da front end, avremmo dovuto fare un solo post da front end, e gestire i due 2 post da backend, possibilmente 
        /// in un solo controller, cosi da poter applicare una transaction che prevenga post una sola tabella
        /// </summary>
        /// <param name="id"></param>
        /// <param name="customerAddress"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<CustomerAddress>> PutCustomerAddress(int id, CustomerAddress customerAddress)
        {
            if (id != customerAddress.AddressId)
            {
                return BadRequest();
            }

            _context.Entry(customerAddress).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                log.Error(ex);
                throw;

            }

            return customerAddress;
        }
    }
}