namespace CRUD_API.DTOs
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }

        public string Message { get; set; } = string.Empty;

        public int StatusCode { get; set; }

        public T Data { get; set; }

        public static ApiResponse<T> SuccessResponse(T data, string message = "", int statuscode = 200)
        {

            return new ApiResponse<T>
            {
                Success = true,
                Message = message,
                StatusCode = statuscode,
                Data = data
            };

        }

        public static ApiResponse<T> ErrorResponse(string message, int statuscode = 500)
        {

            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                StatusCode = statuscode
            };

        }

    }   

}
