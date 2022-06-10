using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Vincall.Infrastructure;

namespace Vincall.OauthService.UserDB
{
    public static class UserExtensions
    {
        public static List<Claim> GetCliams(this User user)
        {
            List<Claim> authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user?.UserName ?? string.Empty),
                new Claim("UserAccount", user.Account),
                new Claim(ClaimTypes.Role,user.IsAdmin== true ? "admin" : "user"),

            };
            return authClaims;
        }
    }
}
