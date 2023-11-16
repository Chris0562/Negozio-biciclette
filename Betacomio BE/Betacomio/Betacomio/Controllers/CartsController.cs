using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Betacomio.Models;
using Betacomio.Models.CustomModels;
using System.Text;
using System.Transactions;

namespace Betacomio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly UsersBetacomioContext _context;
        private static readonly NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();

        public CartsController(UsersBetacomioContext context)
        {
            _context = context;
        }

        /// <summary>
        /// GET: api/Carts  (SWAGGER)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cart>>> GetCarts()
        {
            try
            {
                if (_context.Carts == null)
                {
                    return NotFound();
                }
                return await _context.Carts.ToListAsync();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                Console.WriteLine(ex.Message);
                throw;
            }

        }


        /// <summary>
        /// GET: api/Carts/5 PRENDE CART UTENTE LOGGATO
        /// crea variabile cart, controlla nel db se esiste un cart dell'utente loggato,
        /// se esiste popola cart e lo ritorna al front end, altrimenti notFound
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        [HttpGet("{userID}")]
        public async Task<ActionResult<Cart>> GetCart(int userID)
        {
            try
            {
                if (_context.Carts == null)
                {
                    return NotFound();
                }

                Cart? cart = null;

                if (_context.Carts.Where(c => c.UserId == userID).Any())
                {
                    cart = await _context.Carts.Where(c => c.UserId == userID).FirstAsync();
                }

                if (cart == null)
                {
                    return NotFound(cart);
                }

                return cart;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                Console.WriteLine(ex.Message);
                throw;
            }


        }


        /// <summary>
        /// PUT: api/Carts/5  
        /// aggiorna carrello utente
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cart"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCart(int id, Cart cart)
        {
            if (id != cart.CartId)
            {
                return BadRequest();
            }

            _context.Entry(cart).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                log.Error(ex);
                if (!CartExists(id))
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
        /// POST: api/Carts  
        /// da front end arriva customCart in formato Base64String che contiene l'array di prodotti e user Id. Vedi customCart in Models/CustomModels.
        /// </summary>
        /// <param name="customCart"></param>
        /// <returns></returns>       
        [HttpPost]
        public async Task<ActionResult<Cart>> PostCart(CustomCart customCart)
        {
            try
            {
                if (_context.Carts == null)
                {
                    return Problem("Entity set 'UsersBetacomioContext.Carts'  is null.");
                }

                byte[] byteCart = Convert.FromBase64String(customCart.cartItems);// converte la stringa in array di byte

                if (_context.Carts.Where(i => i.UserId == customCart.userID).Any())  //controlla se l'utente ha già un carrello
                {
                    Cart cart = new Cart //popola l'oggetto cart 
                    {
                        UserId = customCart.userID,
                        CartItems = byteCart,
                        CartId = _context.Carts.Where(c => c.UserId == customCart.userID).Select(c => c.CartId).First()  //recupera l'id del carrello esistente
                    };

                    await PutCart(cart.CartId, cart); //richiama l'update del carrello gia esistente (metodo a riga 102)
                    return Ok(cart);
                }
                else 
                {
                    Cart cart = new Cart  //altrimenti crea un nuovo carrello
                    {
                        UserId = customCart.userID,
                        CartItems = byteCart
                    };

                    _context.Carts.Add(cart);
                    await _context.SaveChangesAsync();

                    return cart;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                Console.WriteLine(ex.Message);
                throw;
            }
        }


        /// <summary>
        /// DELETE: api/Carts/5 
        /// carrelli vuoti vengono cancellati per non intesare il DB
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCart(int id)
        {

            try
            {
                if (_context.Carts == null)
                {
                    return NotFound();
                }
                var cart = await _context.Carts.Where(u => u.UserId == id).FirstOrDefaultAsync();
                if (cart == null)
                {
                    return NotFound();
                }

                _context.Carts.Remove(cart);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                Console.WriteLine(ex.Message);
                throw;
            }
        }


        private bool CartExists(int id)
        {
            return (_context.Carts?.Any(e => e.CartId == id)).GetValueOrDefault();
        }
    }
}
