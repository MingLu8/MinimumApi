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

        public async Task Invoke(HttpContext context, IHttpContextAccessor httpContextAccessor, ILogger<ErrorHandlerMiddleware> logger)
        {
            var path = httpContextAccessor.HttpContext?.Request.Path;
            var method = httpContextAccessor.HttpContext?.Request.Method;
            var startTime = DateTime.Now;
            try
            {
                logger.LogDebug($"==> {method}:{path} on {startTime}.");
                await _next(context);
                logger.LogDebug($"<== {method}:{path} on {DateTime.Now}, duration: {DateTime.Now.Subtract(startTime).TotalMilliseconds}ms.");
            }
            catch (Exception error)
            {
                logger.LogError(error, $"==X {method}:{path} on {DateTime.Now}, duration: {DateTime.Now.Subtract(startTime).TotalMilliseconds}ms, error: {error.Message}.");

                switch (error)
                {
                    case ValidationFailedException e:
                       await TypedResults.ValidationProblem(errors: e.Errors ?? new Dictionary<string, string[]>(), detail: e.Message, title: HttpStatusCode.BadRequest.ToString(), type: "Validation Error")
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
