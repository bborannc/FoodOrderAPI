using System.Net;
using System.Text.Json;
using FluentValidation;
using FoodOrderApi.Core.Dtos;

namespace FoodOrderApi.API.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "İşlem sırasında bir hata oluştu: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var statusCode = (int)HttpStatusCode.InternalServerError;
            CustomResponseDto<NoContentDto> response;

            if (exception is ValidationException validationException)
            {
                statusCode = (int)HttpStatusCode.BadRequest;
                var errors = validationException.Message.Split(" | ").ToList();
                response = CustomResponseDto<NoContentDto>.Fail(statusCode, errors);
            }
            else
            {
                response = CustomResponseDto<NoContentDto>.Fail(statusCode, "Sunucuda beklenmeyen bir hata oluştu.");
            }

            context.Response.StatusCode = statusCode;
            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
