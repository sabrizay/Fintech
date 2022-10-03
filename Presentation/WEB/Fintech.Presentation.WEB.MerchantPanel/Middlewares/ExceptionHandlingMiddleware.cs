using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Fintech.Library.Core.Utilities.Results;
using System.Net;
using System.Text.Json;

namespace Fintech.Presentation.WEB.MerchantPanel.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{message}", ex.Message);
                await HandleExceptionAsync(httpContext, ex);
            }
        }
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var response = context.Response;


            string errorMessage;
            switch (exception)
            {
                case ApplicationException ex:
                    if (ex.Message.Contains("authorize"))
                    {
                        response.StatusCode = (int)HttpStatusCode.Forbidden;
                        errorMessage = ex.Message;
                        break;
                    }
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorMessage = ex.Message;
                    break;
                case KeyNotFoundException ex:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    errorMessage = ex.Message;
                    break;
                case ValidationException ex:
                    response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                    errorMessage = ex.Message;
                    break;
                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    errorMessage = "Internal Server errors.";
                    break;
            }

            _logger.LogError(exception, "{message}", errorMessage);
            var result = JsonSerializer.Serialize(new ErrorResult(errorMessage));
            await context.Response.WriteAsync(result);
        }

    }
}
