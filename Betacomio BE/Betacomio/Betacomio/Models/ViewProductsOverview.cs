using System;
using System.Collections.Generic;

namespace Betacomio.Models;

public partial class ViewProductsOverview
{
    public int ProductId { get; set; }

    public string Name { get; set; } = null!;

    public string ProductNumber { get; set; } = null!;

    public string? Color { get; set; }

    public decimal StandardCost { get; set; }

    public decimal ListPrice { get; set; }

    public string? Size { get; set; }

    public decimal? Weight { get; set; }

    public byte[]? ThumbNailPhoto { get; set; }

    public string? ThumbnailPhotoFileName { get; set; }

    public string CategoryName { get; set; } = null!;

    public string ModelName { get; set; } = null!;

    public string Description { get; set; } = null!;
}
