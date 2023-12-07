using FoodAPI.Models;
using Newtonsoft.Json;
using System.Net;

namespace FoodAPI.Helpers
{
    public class ExceptionHandler
    {
        private readonly RequestDelegate _next; 
        public ExceptionHandler(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            var response = new ApiResponse<string>{ Success = false };

            switch (ex)
            {
                case ApplicationException:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.Message = "Application Exception Occured, please retry after sometinme.";
                    break;
                case FileNotFoundException:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    response.Message = "The requested resource is not found.";
                    break;
                default:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    response.Message = "Internal Server Error, please retry after sometime.";
                    break;

            }

            return context.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }
    }
}
