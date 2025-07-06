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
    public class ProductionService : IProductionService
    {
        private readonly ApiConfiguration _apiSettings;

        public ProductionService(IOptions<ApiConfiguration> options)
        {
            _apiSettings = options.Value;
        }

        public async Task<ApiResponse> SubmitOrderToProductionAsync(Guid customerId, Guid orderId)
        {
			try
			{
                var _params = new
                {
                    customerId = customerId,
                    orderId = orderId
                };

                string jsonData = JsonConvert.SerializeObject(_params);

                return await ApiHelper.PostJsonAsync(_apiSettings.ApiProductUrl, jsonData);
			}
			catch (Exception ex)
			{
                Log.Error($"SubmitOrderToProductionAsync :: {ex}");
                return new ApiResponse
                {
                    StatusCode = 500,
                    Message = $"Exception when calling Production API: {ex.Message}",
                    Data = null
                };
            }
        }
    }
}