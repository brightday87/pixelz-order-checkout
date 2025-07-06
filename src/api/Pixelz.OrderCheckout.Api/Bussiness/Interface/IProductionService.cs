using Pixelz.OrderCheckout.Api.DTOs;

namespace Pixelz.OrderCheckout.Api.Bussiness
{
    public interface IProductionService
    {
        Task<ApiResponse> SubmitOrderToProductionAsync(Guid customerId, Guid orderId);
    }
} 