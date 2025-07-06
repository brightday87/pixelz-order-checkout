namespace Pixelz.OrderCheckout.Api.DTOs
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime? CreateAt { get; set; }
        public decimal? Price { get; set; }
        public decimal? TaxRate { get; set; }
    }
} 