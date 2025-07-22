using System.Net;

namespace UKParliament.CodeTest.Web
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        // Field-level validation errors for frontend display
        public Dictionary<string, string[]>? Errors { get; set; }
        public T? Data { get; set; }
        public HttpStatusCode StatusCode { get; set; }

        public static ApiResponse<T> SuccessResponse(T data, string? message = null, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            return new ApiResponse<T>
            {
                Success = true,
                Message = message,
                Data = data,
                StatusCode = statusCode
            };
        }

        public static ApiResponse<T> Failure(string message, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                Data = default,
                StatusCode = statusCode
            };
        }
    }

}
