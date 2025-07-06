using Microsoft.EntityFrameworkCore;
using Pixelz.OrderCheckout.Api.Bussiness.Interface;
using Pixelz.OrderCheckout.Api.Constants;
using Pixelz.OrderCheckout.Api.DTOs;
using Pixelz.OrderCheckout.Api.Models;
using System.Collections.Concurrent;

namespace Pixelz.OrderCheckout.Api.Bussiness.Implement
{
    public class OrderService : IOrderService
    {
        private readonly PixelzDbContext _context;
        private readonly IPaymentService _paymentService;
        private readonly IEmailService _emailService;
        private readonly IProductionService _productionService;

        public OrderService(PixelzDbContext context,
                            IPaymentService paymentService,
                            IEmailService emailService,
                            IProductionService productionService)
        {
            _context = context;
            _paymentService = paymentService;
            _emailService = emailService;
            _productionService = productionService;
        }

        public async Task<IEnumerable<OrderDto>> GetOrdersAsync(Guid idCustomer, string? orderName)
        {
            var query = _context.Orders.Where(x => x.CustomerID == idCustomer && x.Status == (int)OrderStatus.Draft);
            if (!string.IsNullOrWhiteSpace(orderName))
            {
                query = query.Where(x => x.OrderName.ToLower().Contains(orderName.ToLower()));
            }

            var orders = await query.OrderByDescending(x => x.CreatedAt)
                                    .Select(x => new OrderDto
                                    {
                                        Id = x.OrderID,
                                        Name = x.OrderName,
                                        Status = x.Status,
                                        Details = x.OrderDetails.Select(od => new OrderDetailDto
                                        {
                                            Id = od.OrderDetailID,
                                            OrderId = od.OrderID,
                                            Note = od.Note,
                                            Quantity = od.Quantity,
                                            Product = new ProductDto
                                            {
                                                Id = od.Product.ProductID,
                                                Name = od.Product.Name,
                                                Price = od.Product.Price,
                                                TaxRate = od.Product.TaxRate,
                                                Description = od.Product.Description,
                                                CreateAt = od.Product.CreatedAt
                                            }
                                        }).ToList()
                                    })
                                    .ToListAsync();
            return orders;
        }

        public async Task<BatchSubmitResult> SubmitOrdersAsync(Guid customerId, IEnumerable<Guid> idOrders)
        {
            var resultDict = new ConcurrentDictionary<Guid, ApiResponse>();

            var tasks = idOrders.Select(async idOrder =>
            {
                try
                {
                    var result = await SubmitOrderAsync(customerId, idOrder);
                    resultDict[idOrder] = result;
                }
                catch (Exception ex)
                {
                    resultDict[idOrder] = new ApiResponse
                    {
                        StatusCode = 500,
                        Message = $"Exception for Order {idOrder}: {ex.Message}",
                        Data = null
                    };
                }
            });

            await Task.WhenAll(tasks);

            var summary = new BatchSubmitResult
            {
                Results = resultDict.ToDictionary(kvp => kvp.Key, kvp => kvp.Value),
                SuccessCount = resultDict.Count(x => x.Value.IsSuccess),
                FailedCount = resultDict.Count(x => !x.Value.IsSuccess)
            };

            return summary;
        }

        public async Task<ApiResponse> SubmitOrderAsync(Guid customerId, Guid idOrder)
        {
            var order = _context.Orders.FirstOrDefault(x => x.OrderID == idOrder);
            if (order == null)
                return new ApiResponse { StatusCode = 404, Message = "Order not found" };

            if (order.Status != (int)OrderStatus.Draft)
                return new ApiResponse { StatusCode = 400, Message = "Order is not in a valid state for submission" };

            // Step 1: Process Payment
            var paymentResult = await _paymentService.ProcessPaymentAsync(idOrder);
            if (!paymentResult.IsSuccess)
                return new ApiResponse { StatusCode = 402, Message = "Payment failed" };

            // Step 2: Send Email
            await _emailService.SendOrderConfirmationEmailAsync(customerId, idOrder);

            // Step 3: Submit to Production System
            var productionResult = await _productionService.SubmitOrderToProductionAsync(customerId, idOrder);
            if (!productionResult.IsSuccess)
                return new ApiResponse { StatusCode = 500, Message = "Failed to send order to production" };

            // Step 4: Update status
            order.Status = (int)OrderStatus.Submitted;
            order.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return new ApiResponse { StatusCode = 200, Message = "Order submitted successfully" };
        }
    }
}