﻿using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace FundooNotesApplication.Middleware
{
    public class ErrorHandlerMiddleware
    {
        //private readonly RequestDelegate _next;

        //public ErrorHandlerMiddleware(RequestDelegate next)
        //{
        //    _next = next;
        //}

        //public async Task Invoke(HttpContext context)
        //{
        //    try
        //    {
        //        await _next(context);
        //    }
        //    catch(Exception error)
        //    {
        //        var response = context.Response;
        //        response.ContentType = "application/json";

        //        switch(error)
        //        {
        //            case AppException e:
        //                response.StatusCode = (int)HttpStatusCode.BadRequest; break;
        //            case KeyNotFoundException e:
        //                response.StatusCode = (int)HttpStatusCode.NotFound; break;
        //            case UnauthorizedAccessException e:
        //                response.StatusCode = (int)HttpStatusCode.Unauthorized; break;
        //            default:
        //                response.StatusCode = (int)HttpStatusCode.InternalServerError; break;
        //        }

        //        var result = JsonSerializer.Serialize(new { Success = false, message = error?.Message });
        //        await response.WriteAsync(result);
        //    }
        //}
    }
}
