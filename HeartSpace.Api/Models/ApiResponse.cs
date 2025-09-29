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

        public object? MetaData { get; set; }

        public bool IsSuccess => Status == "success";
        public ApiResponse(string status, string message, int code, object? metaData = null)
        {
            Status = status;
            Message = message;
            Code = code;
            MetaData = metaData;

        }
        public ApiResponse() { }

    }

    public class ApiResponse<T> : ApiResponse
    {
        [JsonPropertyName("data")]
        public T? Data { get; set; }
        public ApiResponse(T? data, string status = "success", string message = "", int code = 200, object? metaData = null)
            : base(status, message, code, metaData)
        {

            Data = data;
        }
        public ApiResponse() : base() { }


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
