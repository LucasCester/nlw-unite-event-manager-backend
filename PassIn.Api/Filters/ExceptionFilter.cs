﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PassIn.Communication.Responses;
using PassIn.Exceptions;
using System.Net;

namespace PassIn.Api.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is PassInException)
                HandlerProjectException(context);
            else
                ThrowUnknowError(context);
        }

        private void HandlerProjectException(ExceptionContext context)
        {
            switch (context.Exception)
            {
                case NotFoundException:

                    context.HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                    context.Result = new NotFoundObjectResult(new ResponseErrorJson(context.Exception.Message));

                break;

                case ErrorOnValidationException:

                    context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                    context.Result = new BadRequestObjectResult(new ResponseErrorJson(context.Exception.Message));

                break;

                case ConflictException:

                    context.HttpContext.Response.StatusCode = StatusCodes.Status409Conflict;
                    context.Result = new ConflictObjectResult(new ResponseErrorJson(context.Exception.Message));

                    break;
            }
        }

        private void ThrowUnknowError(ExceptionContext context)
        {
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Result = new ObjectResult(new ResponseErrorJson("Unknown error"));
        }
    }
}
