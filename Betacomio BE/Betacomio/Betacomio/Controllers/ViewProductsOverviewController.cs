using Betacomio.Models;
using Betacomio.Paginators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Betacomio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ViewProductsOverviewController : ControllerBase
    {
        private readonly AdventureWorksLt2019Context _context;
        private static readonly NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();

        public ViewProductsOverviewController(AdventureWorksLt2019Context context)
        {
            _context = context;
        }

        /// <summary>
        /// GET: api/Products SWAGGER
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ViewProductsOverview>>> GetCustomers()
        {
            try
            {
                if (_context.ViewProductsOverviews == null)
                {
                    return NotFound();
                }
                return await _context.ViewProductsOverviews.ToListAsync();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                Console.WriteLine(ex.Message);
                throw;
            }

        }


        /// <summary>
        /// QUARTO CONTROLLER /// PRELEVA I PRODOTTI IN BASE AL NOME DEL MODELLO 
        /// </summary>
        /// <param name="modelName"></param>
        /// <returns></returns>  
        [HttpGet("{modelName}")]
        public async Task<ActionResult<IEnumerable<ViewProductsOverview>>> GetProductByName(string modelName)
        {
            try
            {
                if (_context.ViewParentCategories == null)
                {
                    return NotFound();
                }
                return await _context.ViewProductsOverviews.Where(m => m.ModelName.Contains(modelName)).ToListAsync();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                Console.WriteLine(ex.Message);
                throw;
            }

        }

    }
}
