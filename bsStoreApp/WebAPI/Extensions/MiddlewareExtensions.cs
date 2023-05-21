using System.Net;

using Entities.ErrorModel;

using Microsoft.AspNetCore.Diagnostics;

using Services.Contracts;

namespace WebAPI.Extensions;

public static class MiddlewareExtensions
{
    public static void ConfigureExceptionHandler(this WebApplication app, ILoggerService loggerService)
    {
        app.UseExceptionHandler(appError =>
        {
            appError.Run(async context =>
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                if (contextFeature is not null)
                {
                    loggerService.LogError($"Something went wrong: {contextFeature.Error}");
                    await context.Response.WriteAsync(new ErrorDetails()
                    {
                        StatusCode = context.Response.StatusCode,
                        Message = "Internal Server Error"
                    }.ToString());
                }
            });
        });
    }
}
