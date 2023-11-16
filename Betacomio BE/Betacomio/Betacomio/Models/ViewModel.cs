using System;
using System.Collections.Generic;

namespace Betacomio.Models;

public partial class ViewModel
{
    public string NomeModello { get; set; } = null!;

    public int ProductModelId { get; set; }

    public string Name { get; set; } = null!;

    public int ProductCategoryId { get; set; }
}
