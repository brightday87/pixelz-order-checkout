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
    public class EmailService : IEmailService
    {
        private readonly ApiConfiguration _apiSettings;

        public EmailService(IOptions<ApiConfiguration> options)
        {
            _apiSettings = options.Value;
        }

        public async Task<ApiResponse> SendOrderConfirmationEmailAsync(Guid customerId, Guid orderId)
        {
            try
            {
                var _params = new
                {
                    customerId = customerId,
                    orderId = orderId
                };

                string jsonData = JsonConvert.SerializeObject(_params);

                return await ApiHelper.PostJsonAsync(_apiSettings.ApiEmailUrl, jsonData);
            }
            catch (Exception ex)
            {
                Log.Error($"SendOrderConfirmationEmailAsync :: {ex}");
                return new ApiResponse
                {
                    StatusCode = 500,
                    Message = $"Exception when calling Email API: {ex.Message}",
                    Data = null
                };
            }
        }
    }
}