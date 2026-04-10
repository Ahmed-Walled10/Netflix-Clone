using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NetflixClone.Application.Common.Models;
using NetflixClone.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ValidationException = NetflixClone.Domain.Exceptions.ValidationException;

namespace NetflixClone.Infrastructure.Middleware
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "Exception occurred: {Message}", exception.Message);

            var (statusCode, title, detail, errors) = MapException(exception);

            httpContext.Response.StatusCode = statusCode;

            var errorResponse = new ErrorResponse(
                Status: statusCode,
                Title: title,
                Detail: detail,
                Errors: errors
            );

            await httpContext.Response.WriteAsJsonAsync(errorResponse, cancellationToken);

            return true;
        }

        private static (int StatusCode, string Title, string Detail, IDictionary<string, string[]>? Errors) MapException(Exception exception)
        {
            return exception switch
            {
                ValidationException validationEx => (
                    StatusCodes.Status400BadRequest,
                    "Validation Error",
                    "One or more validation failures have occurred.",
                    validationEx.Errors),

                NotFoundException notFoundEx => (
                    StatusCodes.Status404NotFound,
                    "Not Found",
                    notFoundEx.Message,
                    null),

                ForbiddenException forbiddenEx => (
                    StatusCodes.Status403Forbidden,
                    "Forbidden",
                    forbiddenEx.Message,
                    null),

                ConflictException conflictEx => (
                    StatusCodes.Status409Conflict,
                    "Conflict",
                    conflictEx.Message,
                    null),

                AppException appEx => (
                    StatusCodes.Status400BadRequest,
                    "Bad Request",
                    appEx.Message,
                    null),

                _ => (
                    StatusCodes.Status500InternalServerError,
                    "Internal Server Error",
                    "An unexpected error occurred.",
                    null)
            };
        }
    }
}
