namespace Pixelz.OrderCheckout.Api.DTOs
{
    public class ResponseDto
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public bool IsSuccess => StatusCode >= 200 && StatusCode < 300;
        public string? Message { get; set; }
        public string? Data { get; set; }
    }

    public class BatchSubmitResult
    {
        public Dictionary<Guid, ApiResponse> Results { get; set; } = new();
        public int SuccessCount { get; set; }
        public int FailedCount { get; set; }
    }
} 