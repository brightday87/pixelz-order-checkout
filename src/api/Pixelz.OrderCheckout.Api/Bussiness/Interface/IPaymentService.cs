using Pixelz.OrderCheckout.Api.DTOs;

namespace Pixelz.OrderCheckout.Api.Bussiness
{
    public interface IPaymentService
    {
        Task<ApiResponse> ProcessPaymentAsync(Guid orderId);
    }
} 