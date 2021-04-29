using System.Net;
using System.Threading.Tasks;
using Global.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace Global.Handler
{
    public static class ExceptionHandler
    {
        public static async Task Handle(HttpContext context)
        {
            var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
            var exception = exceptionHandlerPathFeature.Error;

                      HttpStatusCode statusCode = HttpStatusCode.InternalServerError;

            if (exception as UserException != null)
            {
                statusCode = HttpStatusCode.OK;
            }
            //else
            //{
            //    exception = new Exceptions.CommonException(exception, context);
            //}
            context.Response.StatusCode = (int)statusCode;

            //await context.Response.WriteAsJsonAsync(new { error = exception.Message });
            await context.Response.WriteAsync(exception.Message );
        }
    }
}
