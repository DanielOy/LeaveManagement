using LeaveManagement.Mvc.Contracts;
using LeaveManagement.Mvc.Services.Base;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LeaveManagement.Mvc.Middleware
{
    public class RequestMiddleware
    {
        private readonly RequestDelegate _next;
        ILocalStorageService _localStorageService;

        public RequestMiddleware(RequestDelegate next, ILocalStorageService localStorageService)
        {
            _next = next;
            _localStorageService = localStorageService;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                var endpoint = httpContext.GetEndpoint();
                var authAttribute = endpoint?.Metadata?.GetMetadata<AuthorizeAttribute>();
                if (authAttribute != null)
                {
                    bool tokenExists = _localStorageService.Exists("token");
                    bool tokenIsValid = true;
                    if (tokenExists)
                    {
                        string token = _localStorageService.GetStorageValue<string>("token");
                        var tokenHandler = new JwtSecurityTokenHandler();
                        var tokenContent = tokenHandler.ReadJwtToken(token);
                        var expiry = tokenContent.ValidTo;

                        if (expiry < DateTime.Now)
                            tokenIsValid = false;
                    }

                    if (!tokenIsValid || !tokenExists)
                    {
                        await SignOutAndRedirect(httpContext);
                        return;
                    }

                    if (authAttribute.Roles != null)
                    {
                        var userRole = httpContext.User.FindFirst(ClaimTypes.Role)?.Value;
                        if (authAttribute.Roles.Contains(userRole) == false)
                        {
                            string path = "/home/notauthorized";
                            httpContext.Response.Redirect(path);
                            return;
                        }
                    }

                    await _next(httpContext);
                }
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
        {
            switch (exception)
            {
                case ApiException apiException:
                    await SignOutAndRedirect(httpContext);
                    break;
                default:
                    string path = $"Home/Error";
                    httpContext.Response.Redirect(path);
                    break;
            }
        }

        private async Task SignOutAndRedirect(HttpContext httpContext)
        {
            await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            string path = "/users/login";
            httpContext.Response.Redirect(path);
        }
    }
}
