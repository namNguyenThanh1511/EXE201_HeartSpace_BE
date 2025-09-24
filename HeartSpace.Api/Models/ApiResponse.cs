using System.Text.Json.Serialization;

namespace HeartSpace.Api.Models
{
    public class ApiResponse
    {
        [JsonPropertyName("status")]
        public string Status { get; set; } = "success";

        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;

        [JsonPropertyName("code")]
        public int Code { get; set; } = 200;

        [JsonPropertyName("errors")]
        public List<ApiError>? Errors { get; set; }

        public bool IsSuccess => Status == "success";
    }

    public class ApiResponse<T> : ApiResponse
    {
        [JsonPropertyName("data")]
        public T? Data { get; set; }
    }

    public class ApiError
    {
        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;

        [JsonPropertyName("code")]
        public int Code { get; set; }

        [JsonPropertyName("field")]
        public string? Field { get; set; }
    }
}
