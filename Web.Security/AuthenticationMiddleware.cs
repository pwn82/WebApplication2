using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Web.Common;

namespace Web.Security
{
    public class AuthenticationMiddleware
    {
        ITokenValidation _tokenValidation;
        RequestDelegate _requestDelegate;

        public AuthenticationMiddleware(ITokenValidation tokenValidation, RequestDelegate requestDelegate)
        {
            _tokenValidation = tokenValidation;
            _requestDelegate = requestDelegate;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            string a = httpContext.Request.Path.ToString();
            if (a.Contains(ApiUrl.Login) || a.Contains(ApiUrl.ForgotPassword) || a.Contains(ApiUrl.ResetPassword) ||
                a.Contains(ApiUrl.AccountVerify) || a.Contains(ApiUrl.AccountActivation) || a.Contains(ApiUrl.EmployeeRegistration) ||
                a.Contains(ApiUrl.PatientRegistration) || a.Contains(ApiUrl.DoctorRegistration))
            {
                await _requestDelegate(httpContext).ConfigureAwait(false);
            }
            else
            {
                string token = GetTokenFromHttpContext(httpContext);
                bool value = _tokenValidation.ValidateToken(token);
                if (value == true)
                {
                    await _requestDelegate(httpContext).ConfigureAwait(false);
                    
                }
                else
                {
                    throw new UnauthorizedAccessException();
                }
            }
        }

        public string GetTokenFromHttpContext(HttpContext httpContext)
        {
            var request = httpContext.Request;
            StringValues authorization;
            string token = null;

            if (request.Headers.TryGetValue("Authorization", out authorization) && authorization.Count > 0)
            {
                var authstring = authorization[0];
                if (authstring == "null")
                {
                    request.Headers.TryGetValue("Token", out authorization);
                    token = authorization[0];
                    return token;
                }
                else
                {
                    var segments = authstring.Split(new char[] { ' ' });
                    return token = segments[1];
                }
            }
            return null;
        }
    }
}
