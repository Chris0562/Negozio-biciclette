using System;
using System.Collections.Generic;

namespace Betacomio.Models;

public partial class ViewOrderHistoryDetail
{
    public int SalesOrderId { get; set; }

    public int SalesOrderDetailId { get; set; }

    public int ProductId { get; set; }

    public string Name { get; set; } = null!;

    public short OrderQty { get; set; }

    public decimal LineTotal { get; set; }

    public byte[]? ThumbNailPhoto { get; set; }

    public int CustomerId { get; set; }
}
