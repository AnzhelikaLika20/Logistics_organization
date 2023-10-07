using System.ComponentModel.DataAnnotations;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ValidationException = FluentValidation.ValidationException;

namespace Workshop.Api.ActionFilters;

public class ExceptionFilterAttribute : Attribute, IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        switch (context.Exception)
        {
            case FluentValidation.ValidationException exception:
                HandleBadRequest(context, exception);
                return;
            default:
                HandleInternalError(context);
                return;
        }
    }

    private static void HandleInternalError(ExceptionContext context)
    {
        var jsonResult = new JsonResult(new ErrorResponse(HttpStatusCode.InternalServerError, "возникла ошибка, уже чиним"));
        jsonResult.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Result = jsonResult;
    }

    private static void HandleBadRequest(ExceptionContext context, ValidationException exception)
    {
        var jsonResult = new JsonResult(new ErrorResponse(HttpStatusCode.BadRequest, exception.Message));
        jsonResult.StatusCode = (int)HttpStatusCode.BadRequest;
        context.Result = jsonResult;
    }
}