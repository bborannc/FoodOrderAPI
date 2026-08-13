using System.Text.Json.Serialization;

namespace FoodOrderApi.Core.Dtos
{
    public class CustomResponseDto<T>
    {
        public T? Data { get; set; }

        [JsonIgnore]
        public int StatusCode { get; set; }

        public bool IsSuccess { get; set; }
        public List<string>? Errors { get; set; }

        public static CustomResponseDto<T> Success(int statusCode, T data)
            => new CustomResponseDto<T> { Data = data, StatusCode = statusCode, IsSuccess = true };

        public static CustomResponseDto<T> Success(int statusCode)
            => new CustomResponseDto<T> { StatusCode = statusCode, IsSuccess = true };

        public static CustomResponseDto<T> Fail(int statusCode, List<string> errors)
            => new CustomResponseDto<T> { StatusCode = statusCode, IsSuccess = false, Errors = errors };

        public static CustomResponseDto<T> Fail(int statusCode, string error)
            => new CustomResponseDto<T> { StatusCode = statusCode, IsSuccess = false, Errors = new List<string> { error } };
    }
}
