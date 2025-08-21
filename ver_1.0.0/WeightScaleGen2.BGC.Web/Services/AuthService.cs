using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WeightScaleGen2.BGC.Models.ServicesModels;
using WeightScaleGen2.BGC.Models.ViewModels.User;

namespace WeightScaleGen2.BGC.Web.Services
{
    public class AuthService
    {
        private IConfiguration _config;
        private Uri _baseUri;
        private readonly UserService _userService;
        private readonly IHttpContextAccessor _context;
        public AuthService(IConfiguration config, UserService userService, IHttpContextAccessor context)
        {
            _userService = userService;
            _baseUri = new Uri(config.GetSection("Api").GetSection("BaseUrl").Value);
            this._config = config;
            _context = context;
        }

        public Task<Token> GenerateToken(string username)
        {
            var result = _userService.GetUserByUsername(username);
            var user = new ResultGetUserInfo();
            if (result != null)
            {
                user.username = result.data.email;
                user.name = result.data.name;
                user.emp_code = result.data.emp_code;

                var authClaims = new List<Claim>
                    {
                        new Claim("username", user.username),
                        new Claim("name", user.name),
                        new Claim("emp_code", user.emp_code),
                    };
                var tokengen = GetToken(authClaims);
                var token = new Token();
                token.token = new JwtSecurityTokenHandler().WriteToken(tokengen);
                token.username = result.data.email;
                token.name = result.data.name;
                token.emp_code = result.data.emp_code;
                return Task.FromResult(token);
            }
            else
            {
                var token = new Token();
                token.token = "Username or password is incorrect";
                return Task.FromResult(token);
            }
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this._config["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: this._config["JWT:ValidIssuer"],
                audience: this._config["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }

        public void SetTokenToCookie(string accessToken)
        {
            DateTime expire = DateTime.Now;
            try
            {
                string cookieName = "token";
                string cookieValue = accessToken;

                if (_context.HttpContext.Request.Cookies.ContainsKey(cookieName))
                {
                    _context.HttpContext.Response.Cookies.Delete(cookieName);
                }

                CookieOptions opts = new CookieOptions();
                opts.Expires = expire.AddMonths(1);
                opts.HttpOnly = true;
                opts.Secure = true;

                _context.HttpContext.Response.Cookies.Append(cookieName, cookieValue, opts);
            }
            catch (Exception ex)
            {

            }
        }

        public void ClearCookie()
        {
            foreach (var cookie in _context.HttpContext.Request.Cookies.Keys)
            {
                _context.HttpContext.Response.Cookies.Delete(cookie);
            }
        }
    }
}
