using System;
using System.Collections.Generic;

namespace Betacomio.Models;

public partial class ViewOrderHistoryHeader
{
    public int SalesOrderId { get; set; }

    public DateTime OrderDate { get; set; }

    public byte Status { get; set; }

    public decimal SubTotal { get; set; }

    public int CustomerId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public int AddressId { get; set; }

    public string AddressLine1 { get; set; } = null!;

    public string? AddressLine2 { get; set; }

    public string City { get; set; } = null!;

    public string StateProvince { get; set; } = null!;

    public string CountryRegion { get; set; } = null!;

    public string PostalCode { get; set; } = null!;

    public DateTime DueDate { get; set; }

    public string SalesOrderNumber { get; set; } = null!;

    public string? MiddleName { get; set; }
}
