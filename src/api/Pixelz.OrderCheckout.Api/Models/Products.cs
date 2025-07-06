using System;
using System.Collections.Generic;

namespace Pixelz.OrderCheckout.Api.Models;

public partial class Products
{
    public Guid ProductID { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public decimal TaxRate { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<OrderDetails> OrderDetails { get; set; } = new List<OrderDetails>();
}
