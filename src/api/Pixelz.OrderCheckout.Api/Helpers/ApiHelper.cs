using Pixelz.OrderCheckout.Api.DTOs;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Pixelz.OrderCheckout.Api.Helpers
{
    public static class ApiHelper
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        public static async Task<ApiResponse> GetAsync(string url)
        {
            try
            {
                var response = await _httpClient.GetAsync(url);
                var content = await response.Content.ReadAsStringAsync();

                return new ApiResponse
                {
                    StatusCode = (int)response.StatusCode,
                    Message = response.IsSuccessStatusCode ? "Success" : "Request failed",
                    Data = content
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse
                {
                    StatusCode = 500,
                    Message = $"Exception: {ex.Message}",
                    Data = null
                };
            }
        }

        public static async Task<ApiResponse> PostJsonAsync(string url, string jsonContent)
        {
            try
            {
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(url, content);
                var result = await response.Content.ReadAsStringAsync();

                return new ApiResponse
                {
                    StatusCode = (int)response.StatusCode,
                    Message = response.IsSuccessStatusCode ? "Success" : "Request failed",
                    Data = result
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse
                {
                    StatusCode = 500,
                    Message = $"Exception: {ex.Message}",
                    Data = null
                };
            }
        }
    }
}
