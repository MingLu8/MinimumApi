using Azure;
using MinimumApi.Exceptions;
using System.Net;
using System.Text.Json;

namespace MinimumApi.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {   
                switch (error)
                {
                    case ValidationException e:
                        await Results.Problem(detail: error?.Message, statusCode: (int)HttpStatusCode.BadRequest, title: HttpStatusCode.BadRequest.ToString())
                            .ExecuteAsync(context);
                        break;
                    case ResourceNotFoundException e:
                        await Results.Problem(detail: error?.Message, statusCode: (int)HttpStatusCode.NotFound, title: HttpStatusCode.NotFound.ToString())
                           .ExecuteAsync(context);
                        break;
                    default:
                        await Results.Problem(detail: error?.Message, statusCode: (int)HttpStatusCode.InternalServerError, title: HttpStatusCode.InternalServerError.ToString())
                            .ExecuteAsync(context);
                        break;
                }
            }
        }
    }
}
