using System;
using System.Collections.Generic;

namespace Pixelz.OrderCheckout.Api.Models;

public partial class Orders
{
    public Guid OrderID { get; set; }

    public string OrderName { get; set; }

    public Guid CustomerID { get; set; }

    public int Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Customers Customer { get; set; } = null!;

    public virtual ICollection<OrderDetails> OrderDetails { get; set; } = new List<OrderDetails>();
}
