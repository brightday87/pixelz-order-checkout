namespace Pixelz.OrderCheckout.Api.DTOs
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Status { get; set; }
        public List<OrderDetailDto> Details { get; set; }

        //Auto calculate
        public decimal? TotalAmount
        {
            get
            {
                if (this.Details == null || this.Details.Count == 0)
                    return 0;

                return this.Details.Sum(x => x.OrderAmount);
            }
        }

        public decimal? TotalTax
        {
            get
            {
                if (this.Details == null || this.Details.Count == 0)
                    return 0;

                return this.Details.Sum(x => x.OrderTax);
            }
        }

        public decimal? TotalAmountWithTax
        {
            get
            {
                return this.TotalAmount + (this.TotalTax ?? 0);
            }
        }
    }

    public class OrderDetailDto
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public string? Note { get; set; } = string.Empty;
        public ProductDto? Product { get; set; }
        public decimal? Quantity { get; set; }

        public decimal? OrderAmount
        {
            get
            {
                return this.Product.Price * this.Quantity;
            }
        }

        public decimal? OrderTax
        {
            get
            {
                return this.Product.Price * this.Quantity * this.Product.TaxRate;
            }
        }

        public decimal? OrderAmountWithTax
        {
            get
            {
                return this.OrderAmount + (this.OrderTax ?? 0);
            }
        }
    }
}