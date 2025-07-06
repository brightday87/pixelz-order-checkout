using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Pixelz.OrderCheckout.Api.Configurations;
using Pixelz.OrderCheckout.Api.DTOs;
using Pixelz.OrderCheckout.Api.Helpers;
using Pixelz.OrderCheckout.Api.Models;
using Serilog;

namespace Pixelz.OrderCheckout.Api.Bussiness
{
    public class PaymentService : IPaymentService
    {
        private readonly ApiConfiguration _apiSettings;

        public PaymentService(IOptions<ApiConfiguration> options)
        {
            _apiSettings = options.Value;
        }

        public async Task<ApiResponse> ProcessPaymentAsync(Guid orderId)
        {
            try
            {
                var _params = new
                {
                    orderId = orderId
                };

                string jsonData = JsonConvert.SerializeObject(_params);

                return await ApiHelper.PostJsonAsync(_apiSettings.ApiPaymentUrl, jsonData);
            }
            catch (Exception ex)
            {
                Log.Error($"ProcessPaymentAsync :: {ex}");
                return new ApiResponse
                {
                    StatusCode = 500,
                    Message = $"Exception when calling Payment API: {ex.Message}",
                    Data = null
                };
            }
        }
    }
}