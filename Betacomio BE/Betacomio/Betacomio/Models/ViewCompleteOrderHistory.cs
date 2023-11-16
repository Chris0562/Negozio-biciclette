using System;
using System.Collections.Generic;

namespace Betacomio.Models;

public partial class ViewCompleteOrderHistory
{
    public int SalesOrderId { get; set; }

    public DateTime OrderDate { get; set; }

    public DateTime DueDate { get; set; }

    public byte Status { get; set; }

    public string SalesOrderNumber { get; set; } = null!;

    public int CustomerId { get; set; }

    public int SalesOrderDetailId { get; set; }

    public decimal TotalDue { get; set; }

    public short OrderQty { get; set; }

    public int ProductId { get; set; }

    public decimal LineTotal { get; set; }

    public string AddressLine1 { get; set; } = null!;

    public string? AddressLine2 { get; set; }

    public string City { get; set; } = null!;

    public string StateProvince { get; set; } = null!;

    public string CountryRegion { get; set; } = null!;

    public string PostalCode { get; set; } = null!;
}
