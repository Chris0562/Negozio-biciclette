using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Betacomio.Models;
using Betacomio.Models.CustomModels;

namespace Betacomio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesOrderHeadersController : ControllerBase
    {
        private readonly AdventureWorksLt2019Context _context;
        private static readonly NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();

        public SalesOrderHeadersController(AdventureWorksLt2019Context context)
        {
            _context = context;
        }

        /// <summary>
        /// GET: api/SalesOrderHeaders SWAGGER
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SalesOrderHeader>>> GetSalesOrderHeaders()
        {
            try
            {
                if (_context.SalesOrderHeaders == null)
                {
                    return NotFound();
                }
                return await _context.SalesOrderHeaders.ToListAsync();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                Console.WriteLine(ex.Message);
                throw;
            }

        }


        /// <summary>
        /// GET: api/SalesOrderHeaders/5 SWAGGER
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<SalesOrderHeader>> GetSalesOrderHeader(int id)
        {
            try
            {
                if (_context.SalesOrderHeaders == null)
                {
                    return NotFound();
                }
                var salesOrderHeader = await _context.SalesOrderHeaders.FindAsync(id);

                if (salesOrderHeader == null)
                {
                    return NotFound();
                }

                return salesOrderHeader;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                Console.WriteLine(ex.Message);
                throw;
            }

        }


        /// <summary>
        /// PUT: api/SalesOrderHeaders/5 NON UTILIZZATO
        /// sarebbe da usare in caso di annullamento ordine, e cambiare la colonna status
        /// </summary>
        /// <param name="id"></param>
        /// <param name="salesOrderHeader"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSalesOrderHeader(int id, SalesOrderHeader salesOrderHeader)
        {
            if (id != salesOrderHeader.SalesOrderId)
            {
                return BadRequest();
            }

            _context.Entry(salesOrderHeader).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                log.Error(ex);
                if (!SalesOrderHeaderExists(id))
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
        /// POST: api/SalesOrderHeaders
        /// 
        /// </summary>
        /// <param name="customOrder"></param>
        /// <returns></returns>
        [HttpPost] //vincolo database che dice: dueDate >= orderDate
        public async Task<ActionResult<SalesOrderHeader>> PostSalesOrderHeader(CustomOrder customOrder)  //model CustomOrder necessario per popolare sia header che details ordine
        {
            //richiede transaction: posta dati in 2 table differenti: SalesOrderHeader e SalesOrderDetail
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (_context.SalesOrderHeaders == null)
                    {
                        return Problem("Entity set 'AdventureWorksLt2019Context.SalesOrderHeaders'  is null.");
                    }

                    SalesOrderHeader salesOrderHeader = new SalesOrderHeader  //popola header e posta nella sua tabella
                    {
                        DueDate = customOrder.dueDate,
                        CustomerId = customOrder.customerId,
                        ShipToAddressId = customOrder.shipAddressId,
                        BillToAddressId = customOrder.billToAddressId,
                        ShipMethod = customOrder.shipMethod,
                        SubTotal = customOrder.subTotal,

                    };

                    _context.SalesOrderHeaders.Add(salesOrderHeader);
                    await _context.SaveChangesAsync();

                    List<SalesOrderDetail> salesOrderDetails = new List<SalesOrderDetail>();  //creo una lista di details perche a un header corrispondono tot detttagli in base al numero
                                                                                               // di prodotti diversi all'interno dell ordine => foreach

                    foreach (CountedCart detail in customOrder.details) // details è un array di prodotti, per ogni prodotto aggiungo un new SalesOrderDetails nella lista
                    {
                        salesOrderDetails.Add(new SalesOrderDetail
                        {
                            SalesOrderId = salesOrderHeader.SalesOrderId,
                            OrderQty = (short)detail.orderQty,
                            ProductId = detail.product.ProductId,
                            UnitPrice = detail.product.StandardCost
                        });


                    }

                    foreach (SalesOrderDetail salesOrderDetail in salesOrderDetails) //ciclo la lista e posto i singoli dettagli
                    {
                        _context.SalesOrderDetails.Add(salesOrderDetail);
                        await _context.SaveChangesAsync();
                    }

                    transaction.Commit();  //commit se è andata a buon fine

                    return salesOrderHeader;
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    Console.WriteLine(ex.Message);
                    try
                    {
                        transaction.Rollback();  //rollback se non è andata a buon fine
                    }
                    catch (Exception ex2)
                    {
                        log.Error(ex2.Message);
                    }
                    throw;
                }
            }
          

        }


        /// <summary>
        /// DELETE: api/SalesOrderHeaders/5  non utilizzato
        /// non si cancellano gli storici degli ordini
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSalesOrderHeader(int id)
        {
            try
            {
                if (_context.SalesOrderHeaders == null)
                {
                    return NotFound();
                }
                var salesOrderHeader = await _context.SalesOrderHeaders.FindAsync(id);
                if (salesOrderHeader == null)
                {
                    return NotFound();
                }

                _context.SalesOrderHeaders.Remove(salesOrderHeader);
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

        private bool SalesOrderHeaderExists(int id)
        {
            return (_context.SalesOrderHeaders?.Any(e => e.SalesOrderId == id)).GetValueOrDefault();
        }
    }
}
