using System.Security.Claims;

namespace Ambev.DeveloperEvaluation.Common.Security
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(IUser user);
        public ClaimsPrincipal? ValidateToken(string token);
    }
}
