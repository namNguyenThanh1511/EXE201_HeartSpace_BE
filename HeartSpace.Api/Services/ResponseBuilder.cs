using HeartSpace.Api.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HeartSpace.Api.Services
{
    public static class ResponseBuilder
    {
        // Success responses
        public static ApiResponse<T> SuccessWithData<T>(T data, string message = "Thao tác thành công", int code = 200)
        {
            return new ApiResponse<T>
            {
                Status = "success",
                Data = data,
                Message = message,
                Code = code
            };
        }

        public static ApiResponse SuccessWithMessage(string message = "Thao tác thành công", int code = 200)
        {
            return new ApiResponse
            {
                Status = "success",
                Message = message,
                Code = code
            };
        }

        // Error responses
        public static ApiResponse<T> Error<T>(string message, int code = 400, List<ApiError>? errors = null)
        {
            return new ApiResponse<T>
            {
                Status = "error",
                Message = message,
                Code = code,
                Errors = errors
            };
        }

        public static ApiResponse Error(string message, int code = 400, List<ApiError>? errors = null)
        {
            return new ApiResponse
            {
                Status = "error",
                Message = message,
                Code = code,
                Errors = errors
            };
        }

        // Specific error responses
        public static ApiResponse<T> BadRequest<T>(string message = "Yêu cầu không hợp lệ", List<ApiError>? errors = null)
        {
            return Error<T>(message, 400, errors);
        }

        public static ApiResponse BadRequest(string message = "Yêu cầu không hợp lệ", List<ApiError>? errors = null)
        {
            return Error(message, 400, errors);
        }

        public static ApiResponse<T> NotFound<T>(string message = "Không tìm thấy dữ liệu")
        {
            return Error<T>(message, 404);
        }

        public static ApiResponse NotFound(string message = "Không tìm thấy dữ liệu")
        {
            return Error(message, 404);
        }

        public static ApiResponse<T> Unauthorized<T>(string message = "Không có quyền truy cập")
        {
            return Error<T>(message, 401);
        }

        public static ApiResponse Unauthorized(string message = "Không có quyền truy cập")
        {
            return Error(message, 401);
        }

        public static ApiResponse<T> Forbidden<T>(string message = "Bị cấm truy cập")
        {
            return Error<T>(message, 403);
        }

        public static ApiResponse Forbidden(string message = "Bị cấm truy cập")
        {
            return Error(message, 403);
        }

        public static ApiResponse<T> InternalServerError<T>(string message = "Lỗi máy chủ nội bộ")
        {
            return Error<T>(message, 500);
        }

        public static ApiResponse InternalServerError(string message = "Lỗi máy chủ nội bộ")
        {
            return Error(message, 500);
        }

        // Validation error response
        public static ApiResponse ValidationError(ModelStateDictionary modelState, string message = "Dữ liệu không hợp lệ")
        {
            var errors = new List<ApiError>();

            foreach (var (field, state) in modelState)
            {
                foreach (var error in state.Errors)
                {
                    errors.Add(new ApiError
                    {
                        Field = field,
                        Message = error.ErrorMessage,
                        Code = GetValidationErrorCode(field)
                    });
                }
            }

            return BadRequest(message, errors);
        }

        private static int GetValidationErrorCode(string field)
        {
            // Define validation error codes based on field
            return field.ToLower() switch
            {
                "email" => 34,
                "phonenumber" => 35,
                "password" => 36,
                "username" => 37,
                "fullname" => 38,
                "identifier" => 39,
                _ => 30 // Generic validation error
            };
        }
    }
}
