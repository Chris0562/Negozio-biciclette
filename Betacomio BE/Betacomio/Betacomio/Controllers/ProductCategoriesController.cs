using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Betacomio.Models;

namespace Betacomio.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class ProductCategoriesController : ControllerBase
    {
        private readonly AdventureWorksLt2019Context _context;
        private static readonly NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();

        public ProductCategoriesController(AdventureWorksLt2019Context context)
        {
            _context = context;
        }

        /// <summary>
        /// GET: api/ProductCategories 
        /// prende tutte le categorie   (PARENT CATEGORIES => CATEGORIES => MODELS = PRODUCTS)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductCategory>>> GetProductCategories()
        {
            try
            {
                if (_context.ProductCategories == null)
                {
                    return NotFound();
                }
                return await _context.ProductCategories.ToListAsync();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                Console.WriteLine(ex.Message);
                throw;
            }

        }


        /// <summary>
        /// prende tutte le categorie di una specifica macro categoria
        /// </summary> 
        [HttpGet("{ParentProductCategory}")]
        public async Task<ActionResult<IEnumerable<ProductCategory>>> GetByParentPC(int ParentProductCategory)
        {
            try
            {
                if (_context.ProductCategories == null)
                {
                    return NotFound();
                }
                return await _context.ProductCategories.Where(e => e.ParentProductCategoryId == ParentProductCategory).ToListAsync();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                Console.WriteLine(ex.Message);
                throw;
            }

        }

        /// <summary>
        /// PUT: api/ProductCategories/5 non implementato nel FE 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="productCategory"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductCategory(int id, ProductCategory productCategory)
        {
            if (id != productCategory.ProductCategoryId)
            {
                return BadRequest();
            }

            _context.Entry(productCategory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                log.Error(ex);
                if (!ProductCategoryExists(id))
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
        /// POST: api/ProductCategories non implementato nel FE
        /// </summary>
        /// <param name="productCategory"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ProductCategory>> PostProductCategory(ProductCategory productCategory)
        {
            try
            {
                if (_context.ProductCategories == null)
                {
                    return Problem("Entity set 'AdventureWorksLt2019Context.ProductCategories'  is null.");
                }
                _context.ProductCategories.Add(productCategory);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetProductCategory", new { id = productCategory.ProductCategoryId }, productCategory);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                Console.WriteLine(ex.Message);
                throw;
            }

        }


        /// <summary>
        /// DELETE: api/ProductCategories/5 non implementato nel FE
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductCategory(int id)
        {
            try
            {
                if (_context.ProductCategories == null)
                {
                    return NotFound();
                }
                var productCategory = await _context.ProductCategories.FindAsync(id);
                if (productCategory == null)
                {
                    return NotFound();
                }

                _context.ProductCategories.Remove(productCategory);
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

        private bool ProductCategoryExists(int id)
        {
            return (_context.ProductCategories?.Any(e => e.ProductCategoryId == id)).GetValueOrDefault();
        }
    }
}
