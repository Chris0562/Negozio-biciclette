using System;
using System.Collections.Generic;

namespace Betacomio.Models;

public partial class User
{
    public int UserId { get; set; }

    public string FirstName { get; set; } = null!;

    public string? MiddleName { get; set; }

    public string LastName { get; set; } = null!;

    public string EmailAddress { get; set; } = null!;

    public string? Phone { get; set; }

    public string PasswordHash { get; set; } = null!;

    public string PasswordSalt { get; set; } = null!;

    public DateTime? ModifiedDate { get; set; }

    public string? UserName { get; set; }

    public int? OldCustomerId { get; set; }

    public int? OldCustomerId2 { get; set; }

    public bool IsAdmin { get; set; }

    public virtual ICollection<Cart>? Carts { get; set; } = new List<Cart>();
}
