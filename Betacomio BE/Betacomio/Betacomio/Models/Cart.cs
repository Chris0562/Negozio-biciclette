using System;
using System.Collections.Generic;

namespace Betacomio.Models;

public partial class Cart
{
    public int CartId { get; set; }

    public byte[]? CartItems { get; set; }

    public int UserId { get; set; }

    public virtual User User { get; set; } = null!;
}
