using Betacomio.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NLog;
using System.Data;

namespace Betacomio.Controllers
{
    /// <summary>
    /// PRIMO CONTROLLER HOMEPAGE /// PRELEVA LE MACROCATEGORIE PER LE CARD INIZIALI (accessories, bikes ...)
    /// </summary>

    [Route("api/[controller]")]
    [ApiController]
    public class ViewParentCategoriesController : ControllerBase
    {
        private readonly AdventureWorksLt2019Context _context;
        private static readonly NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();

        public ViewParentCategoriesController(AdventureWorksLt2019Context context)
        {
            _context = context;
        }



        /// <summary>
        /// GET: ViewParentCategoriesController
        /// prende le 4 macro
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ViewParentCategory>>> GetViewParentCategories()
        {
            try
            {
                if (_context.ViewParentCategories == null)
                {
                    return NotFound();
                }
                return await _context.ViewParentCategories.ToListAsync();
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
