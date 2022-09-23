using Microsoft.AspNetCore.Diagnostics;
using Shared.Dtos;
using System.Text.Json;

namespace AuthServer.API.Exceptions
{
    public static class CustomExceptionHandler
    {
        public static void UseCustomException(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(c =>
            {
                c.Run(async context =>
                {
                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "application/json";

                    var errorFeature = context.Features.Get<IExceptionHandlerFeature>();

                    if (errorFeature != null)
                    {
                        var ex = errorFeature.Error;                        
                    }
                    ErrorDto errorDto = new ErrorDto();
                    var res = Response<NoDataDto>.Fail(errorDto, 500);

                    await context.Response.WriteAsync(JsonSerializer.Serialize(res));
                });
            });
        }
    }
}
