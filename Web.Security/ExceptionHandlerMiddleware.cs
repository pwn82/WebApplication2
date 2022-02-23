using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Web.Common;
using static Web.Common.CustomException;

namespace Web.Security
{
    [ExcludeFromCodeCoverage]
    public class ExceptionHandlerMiddleware
    {
        IConfiguration _configuration;
        RequestDelegate _requestDelegate;

        public ExceptionHandlerMiddleware(IConfiguration configuration, RequestDelegate requestDelegate)
        {
            _configuration = configuration;
            _requestDelegate = requestDelegate;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _requestDelegate(httpContext).ConfigureAwait(false);
            }
            catch (UnauthorizedAccessException ex)
            {
                Log.Logger.Error(ex, ex.Message);
                httpContext.Response.StatusCode = Convert.ToInt32(HttpStatusCode.Unauthorized);
                httpContext.Response.ContentType = "application / json";
                await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(
                    new CustomException(ErrorCodes.Unauthorized.ToString(), _configuration).GetErrorObject()));
            }
            catch (CustomException ex)
            {
                Log.Logger.Error(ex, ex.Message);
                httpContext.Response.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
                httpContext.Response.ContentType = "application / json";
                await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(
                    ex.GetErrorObject())).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, ex.Message);
                httpContext.Response.StatusCode = Convert.ToInt32(HttpStatusCode.InternalServerError);
                httpContext.Response.ContentType = "application / json";
                await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(new CustomException(
                    ErrorCodes.Default.ToString(), _configuration).GetErrorObject())).ConfigureAwait(false);
            }
        }

    }
}
