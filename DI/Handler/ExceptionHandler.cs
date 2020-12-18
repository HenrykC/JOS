using DI.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DI.Handler
{
    public static class ExceptionHandler
    {
        public async static Task Handle(HttpContext context)
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

            await context.Response.WriteAsJsonAsync(new { error = exception.Message });
        }
    }
}
