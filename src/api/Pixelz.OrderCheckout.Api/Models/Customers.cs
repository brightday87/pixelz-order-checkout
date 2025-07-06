using System;
using System.Collections.Generic;

namespace Pixelz.OrderCheckout.Api.Models;

public partial class Customers
{
    public Guid CustomerID { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Orders> Orders { get; set; } = new List<Orders>();
}
