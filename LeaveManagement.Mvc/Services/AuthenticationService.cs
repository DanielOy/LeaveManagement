using AutoMapper;
using LeaveManagement.Mvc.Contracts;
using LeaveManagement.Mvc.Models;
using LeaveManagement.Mvc.Services.Base;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IAuthenticationService = LeaveManagement.Mvc.Contracts.IAuthenticationService;

namespace LeaveManagement.Mvc.Services
{
    public class AuthenticationService : BaseHttpService, IAuthenticationService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly JwtSecurityTokenHandler _tokenHandler;
        private readonly IMapper _mapper;
        public AuthenticationService(ILocalStorageService localStorage, IClient client, IHttpContextAccessor contextAccessor, IMapper mapper) : base(localStorage, client)
        {
            _contextAccessor = contextAccessor;
            _tokenHandler = new JwtSecurityTokenHandler();
            _mapper = mapper;
        }

        public async Task<bool> Register(RegisterVM register)
        {
            var request = _mapper.Map<RegistrationRequest>(register);
            var response = await _client.RegisterAsync(request);

            if (!string.IsNullOrEmpty(response.UserId))
            {
                await Login(register.Email, register.Password);
                return true;
            }

            return false;
        }

        public async Task<bool> Login(string email, string password)
        {
            try
            {
                var loginParams = new AuthRequest { Email = email, Password = password };
                var response = await _client.LoginAsync(loginParams);

                if (!string.IsNullOrEmpty(response.Token))
                {
                    var tokenContent = _tokenHandler.ReadJwtToken(response.Token);
                    var claims = ParseClaims(tokenContent);
                    var user = new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme));
                    var login = _contextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, user);
                    _localStorage.SetStorageValue("token", response.Token);

                    return true;
                }
            }
            catch
            { }

            return false;
        }

        private IList<Claim> ParseClaims(JwtSecurityToken tokenContent)
        {
            var claims = tokenContent.Claims.ToList();
            claims.Add(new Claim(ClaimTypes.Name, tokenContent.Subject));
            return claims;
        }

        public async Task Logout()
        {
            _localStorage.ClearStorage(new List<string> { "token" });
            await _contextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
