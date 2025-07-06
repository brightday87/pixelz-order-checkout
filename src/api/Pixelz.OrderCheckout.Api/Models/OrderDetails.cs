using System;
using System.Collections.Generic;

namespace Pixelz.OrderCheckout.Api.Models;

public partial class OrderDetails
{
    public Guid OrderDetailID { get; set; }

    public Guid OrderID { get; set; }

    public Guid ProductID { get; set; }

    public int Quantity { get; set; }

    public string? Note { get; set; }

    public virtual Orders Order { get; set; } = null!;

    public virtual Products Product { get; set; } = null!;
}
