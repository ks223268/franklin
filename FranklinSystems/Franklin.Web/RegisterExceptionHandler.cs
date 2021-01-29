using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;

using Franklin.Common.Model;

namespace Franklin.Web {

    /// <summary>
    /// Handle all the thrown errors here.
    /// </summary>
    public static class RegisterExceptionHandler {
                
        public static void UseExceptionHandler(this IApplicationBuilder app, ILoggerFactory loggerFactory) {
            
            app.UseExceptionHandler(appError => {
                appError.Run(async context => {
                    ILogger logger = loggerFactory.CreateLogger("ExceptionHandler");

                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null) {                        
                        logger.LogError($"Something went wrong: {contextFeature.Error}");
                        
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(new ErrorModel() {                            
                            Message = "An error has occurred."
                        }));
                    }
                });
            });
        }
    }        
}
