using Pixelz.OrderCheckout.Api.DTOs;

namespace Pixelz.OrderCheckout.Api.Bussiness
{
    public interface IEmailService
    {
        Task<ApiResponse> SendOrderConfirmationEmailAsync(Guid customerId, Guid orderId);
    }
} 