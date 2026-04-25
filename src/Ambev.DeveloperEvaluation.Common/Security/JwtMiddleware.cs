using Ambev.DeveloperEvaluation.Common.Entity;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Common.Security
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IJwtTokenGenerator jwtService)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (!string.IsNullOrEmpty(token))
            {
                var principal = jwtService.ValidateToken(token);

                if (principal == null)
                {
                    var user = GetUserIdFromExpiredToken(token);
                    if (user != null)
                    {
                        var newToken = jwtService.GenerateToken(user);
                        context.Response.Headers.Add("X-New-Token", newToken);
                    }
                }
                else
                {
                    context.User = principal;
                }
            }
            await _next(context);
        }

        private IUser? GetUserIdFromExpiredToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();

            // Lê o token sem validar assinatura ou expiração
            var jwtToken = handler.ReadJwtToken(token);

            var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == "nameid")?.Value;
            var username = jwtToken.Claims.FirstOrDefault(c => c.Type == "unique_name")?.Value;
            var role = jwtToken.Claims.FirstOrDefault(c => c.Type == "role")?.Value;


            if (jwtToken.ValidTo > DateTime.UtcNow)
                return null;

            return new JwtUser
            {
                Id = userId,
                Username = username,
                Role = role
            };

        }

    }
}
