

using Betacomio.Models;
using Betacomio.Models.CustomModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Betacomio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ViewCompleteOrderHistoryController : ControllerBase
    {
        private readonly AdventureWorksLt2019Context _context;
        private static readonly NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();

        public ViewCompleteOrderHistoryController(AdventureWorksLt2019Context context)
        {
            _context = context;
        }

        /// <summary>
        /// Recupera storico ordini
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        [HttpGet("history/{customerID}")]
        public async Task<ActionResult<IEnumerable<CustomOrderHistory>>> GetHistoryByCustomerID(int customerID)
        {
            try
            {
                if (_context.ViewCompleteOrderHistories == null)
                {
                    return NotFound();
                }
                var historyList = await _context.ViewCompleteOrderHistories.Where(m => m.CustomerId == customerID).ToListAsync();  //recupero tutti i dati dalla viewCompleteOrderHistory

                List<Detail> details = new List<Detail>();  //istanzio lista dettagli

                List<CustomOrderHistory> customOrderHistories = new List<CustomOrderHistory>();  //istanzio lista storico ordini

                foreach (var x in historyList)    //popolo details ciclando i dati presi dall view
                {
                    Detail detail = new Detail   
                    {
                        SalesOrderId = x.SalesOrderId,
                        SalesOrderDetailId = x.SalesOrderDetailId,
                        OrderQty = x.OrderQty,
                        ProductId = x.ProductId,
                        LineTotal = x.LineTotal
                    };

                    details.Add(detail);  
                }

                foreach (var y in historyList)   //popolo storico ciclando i dati della view, ma per ognuno aggiungo i suoi dettagli
                {
                    CustomOrderHistory customOrderHistory = new CustomOrderHistory
                    {
                        CustomerId = y.CustomerId,
                        SalesOrderId = y.SalesOrderId,
                        SalesOrderNumber = y.SalesOrderNumber,
                        OrderDate = y.OrderDate,
                        DueDate = y.DueDate,
                        Status = y.Status,
                        TotalDue = y.TotalDue,
                        AddressLine1 = y.AddressLine1,
                        AddressLine2 = y.AddressLine2,
                        City = y.City,
                        CountryRegion = y.CountryRegion,
                        PostalCode = y.PostalCode,
                        StateProvince = y.StateProvince,
                        Details = details.Where(d => d.SalesOrderId == y.SalesOrderId).ToList()  //riesco a prendere i dettagli grazie all'id dell'ordine
                    };

                    customOrderHistories.Add(customOrderHistory);
                    //customOrderHistories = customOrderHistories.DistinctBy(d => d.SalesOrderId).ToList(); 
                    ///* quando ciclo historyList per popolare customOrderHistory, vado a prendere ogni singola row, il problema
                    // è che quando arrivo alla proprietà Details prende 3 righe dalla lista details, al ciclo successivo, quando salesOrderId si ripete
                    //ottengo tot liste identiche. con il distinctBy ritorna elementi distinti per una chiave (salesOrderId) */

                }
                customOrderHistories = customOrderHistories.DistinctBy(d => d.SalesOrderId).ToList();
                /* quando ciclo historyList per popolare customOrderHistory, vado a prendere ogni singola row, il problema
                 è che quando arrivo alla proprietà Details prende 3 righe dalla lista details, al ciclo successivo, quando salesOrderId si ripete
                ottengo tot liste identiche. con il distinctBy ritorna elementi distinti per una chiave (salesOrderId) */
                return customOrderHistories;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                Console.WriteLine(ex.Message);
                throw;
            }

        }


        /// <summary>
        /// Recupera solo l'ultimo ordine effettuato
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        [HttpGet("lastPlaced{customerID}")]
        public async Task<ActionResult<CustomOrderHistory>> GetLastPlacedOrder(int customerID)
        {
            try
            {
                if (_context.ViewCompleteOrderHistories == null)
                {
                    return NotFound();
                }
                var historyList = await _context.ViewCompleteOrderHistories.Where(m => m.CustomerId == customerID).ToListAsync();

                List<Detail> details = new List<Detail>();

                List<CustomOrderHistory> customOrderHistories = new List<CustomOrderHistory>();

                CustomOrderHistory lastPlacedOrder = new();

                foreach (var x in historyList)
                {
                    Detail detail = new Detail
                    {
                        SalesOrderId = x.SalesOrderId,
                        SalesOrderDetailId = x.SalesOrderDetailId,
                        OrderQty = x.OrderQty,
                        ProductId = x.ProductId,
                        LineTotal = x.LineTotal
                    };

                    details.Add(detail);
                }

                foreach (var y in historyList)
                {
                    CustomOrderHistory customOrderHistory = new CustomOrderHistory
                    {
                        CustomerId = y.CustomerId,
                        SalesOrderId = y.SalesOrderId,
                        SalesOrderNumber = y.SalesOrderNumber,
                        OrderDate = y.OrderDate,
                        DueDate = y.DueDate,
                        Status = y.Status,
                        TotalDue = y.TotalDue,
                        AddressLine1 = y.AddressLine1,
                        AddressLine2 = y.AddressLine2,
                        City = y.City,
                        CountryRegion = y.CountryRegion,
                        PostalCode = y.PostalCode,
                        StateProvince = y.StateProvince,
                        Details = details.Where(d => d.SalesOrderId == y.SalesOrderId).ToList()
                    };

                    customOrderHistories.Add(customOrderHistory);
                    
                }
                customOrderHistories = customOrderHistories.DistinctBy(d => d.SalesOrderId).ToList();

                lastPlacedOrder = customOrderHistories.Last();  //prende l'ultimo della lista

                return lastPlacedOrder;
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
