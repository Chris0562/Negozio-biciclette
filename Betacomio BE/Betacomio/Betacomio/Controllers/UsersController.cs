using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Betacomio.Models;
using Betacomio.Models.CustomModels;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using Microsoft.Data.SqlClient;
using System.Runtime.ExceptionServices;
using Betacomio.Paginators;
using Betacomio.Authentication;
using Microsoft.AspNetCore.Authorization;
using NuGet.Versioning;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Betacomio.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UsersBetacomioContext _context;
        private readonly AdventureWorksLt2019Context _adventure;
        private static readonly NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();

        public UsersController(UsersBetacomioContext context, AdventureWorksLt2019Context adventure)
        {
            _context = context;
            _adventure = adventure;
        }

        /// <summary>
        /// GET: api/Users  
        /// utilizzato da admin per visualizzare lista utenti
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            try
            {
                if (_context.Users == null)
                {
                    return NotFound();
                }
                return await _context.Users.ToListAsync();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                Console.WriteLine(ex.Message);
                throw;
            }

        }


        /// <summary>
        /// GET: api/Users/5  
        /// prende dati utente loggato sostituito da metodo "ByEmail"
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            try
            {
                if (_context.Users == null)
                {
                    return NotFound();
                }
                var user = await _context.Users.FindAsync(id);

                if (user == null)
                {
                    return NotFound();
                }

                return user;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                Console.WriteLine(ex.Message);
                throw;
            }

        }


        /// <summary>
        /// prende dati utente loggato 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [Route("ByEmail/{email}")]
        [HttpGet]
        public async Task<ActionResult<User>> GetUserByEmail(string email)
        {
            try
            {
                if (_context.Users == null)
                {
                    return NotFound();
                }
                return await _context.Users.Where(m => m.EmailAddress == email).FirstAsync();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                Console.WriteLine(ex.Message);
                throw;
            }

        }


        /// <summary>
        /// Metodo per aggiornare dati utente db UsersBetacomio
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<User>> PutUser(UpdateUser form) //form da FE
        {
            if (await _context.Users.AnyAsync(u => u.UserId.Equals(form.UserId)))
            {
                User user = new User  //popolo user con dati del form e recuperando dati sensibili da db per non alterarli
                {
                    EmailAddress = form.EmailAddress,
                    FirstName = form.FirstName,
                    MiddleName = form.MiddleName,
                    LastName = form.LastName,
                    Phone = form.Phone,
                    UserName = form.UserName,
                    UserId = await _context.Users.Where(u => u.UserId.Equals(form.UserId)).Select(u => u.UserId).FirstAsync(),
                    PasswordHash = await _context.Users.Where(u => u.UserId.Equals(form.UserId)).Select(u => u.PasswordHash).FirstAsync(),
                    PasswordSalt = await _context.Users.Where(u => u.UserId.Equals(form.UserId)).Select(u => u.PasswordSalt).FirstAsync(),
                    OldCustomerId = await _context.Users.Where(u => u.UserId.Equals(form.UserId)).Select(u => u.OldCustomerId).FirstAsync(),
                    OldCustomerId2 = await _context.Users.Where(u => u.UserId.Equals(form.UserId)).Select(u => u.OldCustomerId2).FirstAsync(),
                    IsAdmin = await _context.Users.Where(u => u.UserId.Equals(form.UserId)).Select(u => u.IsAdmin).FirstAsync(),
                    ModifiedDate = DateTime.Now
                };

                _context.Entry(user).State = EntityState.Modified;
                try
                {
                    await _context.SaveChangesAsync();
                    return user;
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    log.Error(ex);
                    throw;
                }
            }
            return BadRequest();
        }


        /// <summary>
        /// Metodo di login
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [Route("login")]
        [HttpPost]
        public async Task<ActionResult> login(LoginForm form) //form da FE
        {
            try
            {
                if (_context.Users == null)
                {
                    return Problem("Entity set 'UsersBetacomioContext.Users'  is null.");
                }

                if (await _context.Users.AnyAsync(c => c.EmailAddress.Equals(form.Email)))  //se l'utente esiste nella tabella 
                {
                    KeyValuePair<string, string> keyValuePair = new KeyValuePair<string, string>();  
                    string saltCode = string.Empty;
                    string sHashed = string.Empty;

                    saltCode = await _context.Users.Where(c => c.EmailAddress.Equals(form.Email)).Select(c => c.PasswordSalt).FirstOrDefaultAsync();  //recupero salt

                    byte[] bytes = Convert.FromBase64String(saltCode);  //converto in array di bytes

                    sHashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(   //creo l'hashed con la password che arriva dal form
                        password: form.Password,
                        salt: bytes,
                        prf: KeyDerivationPrf.HMACSHA256,
                        iterationCount: 100000,
                        numBytesRequested: 32
                        ));

                    keyValuePair = new KeyValuePair<string, string>(sHashed, Convert.ToBase64String(bytes));  //popolo keyValuePair con hashed e salt riconvertito

                    if (await _context.Users.AnyAsync(c => c.PasswordHash.Equals(keyValuePair.Key))) //se hashed nel db corrisponde alla key in keyvaluepair loggo
                    {
                        return Ok();
                    }

                    return Problem("Wrong Credentials");  

                }

                else if (await _adventure.Customers.AnyAsync(c => c.EmailAddress.Equals(form.Email))) //se non trova utente in usersBetacomio controlla in adventure works
                {
                    return Problem("The account is too old, create a new one");  //trova utente vecchio, FE avvisa di fare nuovo account
                }

                return Problem("Account not found, create a new one"); //non trova, FE AVVISA DI CREARE ACCOUNT
            }
            catch (Exception ex)
            {
                log.Error(ex);
                Console.WriteLine(ex.Message);
                throw;
            }

        }


        /// <summary>
        /// Metodo di registrazione
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<User>> PostNewUser(RegisterForm form) //form da FE
        {
            int oldID = 0;
            int oldID_2 = 0;    //istanzio variabili
            bool isOld = false;

            try
            {
                if (_context.Users == null)
                {
                    return Problem("Entity set 'UsersBetacomioContext.Users'  is null.");
                }

                if (_context.Users.Where(c => c.EmailAddress.Equals(form.Email)).Any())  //controllo se mail è gia utilizzata nel nuovon db (quindi mail deve essere univoca)
                {
                    return Problem("Mail is already used");
                }

                if (_adventure.Customers.Where(c => c.EmailAddress.Equals(form.Email)).Any())   //se la mail esiste nel vecchio db allora vado a recuper i vecchi customer id 
                {
                    oldID = _adventure.Customers.Where(c => c.EmailAddress.Equals(form.Email)).OrderBy(c => c.CustomerId).Select(c => c.CustomerId).First();
                    oldID_2 = _adventure.Customers.Where(c => c.EmailAddress.Equals(form.Email)).OrderBy(c => c.CustomerId).Select(c => c.CustomerId).Last();
                    isOld = true;   // se utente vecchio
                }

                /////////////ENCRYPT PASS/////////
                KeyValuePair<string, string> keyValuePair = new KeyValuePair<string, string>();

                byte[] byteSalt = new byte[16];

                using (RNGCryptoServiceProvider rng = new())  //salt
                {
                    rng.GetNonZeroBytes(byteSalt);
                }

                string sHashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(  //hash
                    password: form.Password,
                    salt: byteSalt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 100000,
                    numBytesRequested: 32
                ));

                keyValuePair = new KeyValuePair<string, string>(sHashed, Convert.ToBase64String(byteSalt));   //hash + salt

                User user = new User  //popolo user con dati ddel form + keyValuePair + vecchi customer id se isOld è true + eventuale ruolo admin
                {
                    EmailAddress = form.Email,
                    LastName = form.LastName,
                    MiddleName = form.MiddleName,
                    FirstName = form.FirstName,
                    Phone = form.PhoneNumber,
                    UserName = form.UserName,
                    PasswordHash = keyValuePair.Key,
                    PasswordSalt = keyValuePair.Value,
                    OldCustomerId = isOld ? oldID : null,
                    OldCustomerId2 = isOld ? oldID_2 : null,
                    IsAdmin = form.isAdmin
                };

                _context.Users.Add(user);

                await _context.SaveChangesAsync();

                return CreatedAtAction("GetUser", new { id = user.UserId }, user);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }


        }


        /// <summary>
        /// utilizzato da admin
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                if (_context.Users == null)
                {
                    return NotFound();
                }
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    return NotFound();
                }

                _context.Users.Remove(user);
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

        private bool UserExists(int id)
        {
            return (_context.Users?.Any(e => e.UserId == id)).GetValueOrDefault();
        }
    }
}

