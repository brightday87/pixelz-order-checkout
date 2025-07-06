using Pixelz.OrderCheckout.Api.DTOs;

namespace Pixelz.OrderCheckout.Api.Bussiness.Interface
{
    public interface IOrderService
    {
        //get Orders by User
        Task<IEnumerable<OrderDto>> GetOrdersAsync(Guid idCustomer, string? orderName);

        //submit Orders
        Task<BatchSubmitResult> SubmitOrdersAsync(Guid idCustomer, IEnumerable<Guid> idOrders);

        Task<ApiResponse> SubmitOrderAsync(Guid idCustomer, Guid idOrder);
    }
} 