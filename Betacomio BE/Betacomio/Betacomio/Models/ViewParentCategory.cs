using System;
using System.Collections.Generic;

namespace Betacomio.Models;

public partial class ViewParentCategory
{
    public int ProductCategoryId { get; set; }

    public string ParentProductCategoryName { get; set; } = null!;
}
