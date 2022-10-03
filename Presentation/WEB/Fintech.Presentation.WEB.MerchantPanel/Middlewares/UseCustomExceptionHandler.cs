using Microsoft.AspNetCore.Diagnostics;
using Fintech.Library.Core.Utilities.Exceptions;
using Fintech.Library.Entities.Dto;
using System.Text.Json;

namespace Fintech.Presentation.WEB.MerchantPanel.Middlewares
{
    public static class UseCustomExceptionHandler
    {
        public static void UseCustomException(this IApplicationBuilder builder)
        {

            builder.UseExceptionHandler(config =>
            {
                config.Run(async context =>
                {
                    context.Response.ContentType = "application/json";

                    var exceptionFuture = context.Features.Get<IExceptionHandlerFeature>();

                    var statusCode = exceptionFuture?.Error switch
                    {
                        CustomException => 400,
                        _ => 500
                    };

                    context.Response.StatusCode = statusCode;

                    var response = new GlobalExceptionResponseDto(exceptionFuture?.Error.Message);

                    await context.Response.WriteAsync(JsonSerializer.Serialize(response));

                });
            });
        }
    }
}
