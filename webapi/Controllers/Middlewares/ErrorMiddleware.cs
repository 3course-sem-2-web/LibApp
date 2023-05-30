using System.Net;
using System.Text.Json;
using FluentValidation;
using task2.BLL.Exceptions;

namespace task2.Controllers.Middlewares;

public class ErrorMiddleware
{
    
    private readonly RequestDelegate _next;

    public ErrorMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception e)
        {
            await HandleError(context, e);
        }
    }

    private Task HandleError(HttpContext context, Exception occured)
    {
        var responseCodeWhenErrorOccured = HttpStatusCode.InternalServerError;
        var responseMessage = string.Empty;
        
        switch (occured)
        {
            case BookNotFoundException:
                responseCodeWhenErrorOccured = HttpStatusCode.NoContent;
                responseMessage = "The book has not been found";
                break;
            case DeleteAccessDeniedException:
                responseCodeWhenErrorOccured = HttpStatusCode.Forbidden;
                responseMessage = "Invalid secret when attempting to delete";
                break;
            case ValidationException validationException:
                responseCodeWhenErrorOccured = HttpStatusCode.BadRequest;
                responseMessage = JsonSerializer.Serialize(validationException.Errors);
                break;
        }
        
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)responseCodeWhenErrorOccured;

        if (responseMessage == string.Empty)
        {
            responseMessage = JsonSerializer.Serialize(new { unHandledServerErrorMessage = occured.Message });
        }

        return context.Response.WriteAsync(responseMessage);
    }
}