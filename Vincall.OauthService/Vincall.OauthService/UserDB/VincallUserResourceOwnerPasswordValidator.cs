using IdentityServer4.Test;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using Vincall.Infrastructure;

namespace Vincall.OauthService.UserDB
{
    public class VincallUserResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly VincallDBContext _dbContext;

        private readonly ISystemClock _clock;
        private readonly IMemoryCache _cache;

        public VincallUserResourceOwnerPasswordValidator(VincallDBContext dbContext, ISystemClock clock, IMemoryCache cache)
        {
            _dbContext = dbContext;
            _clock = clock;
            _cache = cache;
        }

      
        private Task<User> GetUserByAccountAsync(string userAccount)
        {
            return _cache.GetOrCreateAsync<User>(userAccount, c =>
            {
                c.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30);
                return _dbContext.Set<User>().AsQueryable().Where(x => x.Account == userAccount).FirstOrDefaultAsync();
            });
        }

        private bool ValidateUser(User user,string password)
        {
            if (user?.Password == null) return false;
            return user.Password.Equals(Md5Helper.Md5(password), StringComparison.CurrentCulture);
        }


        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var user = await this.GetUserByAccountAsync(context.UserName);            
            var bValid = ValidateUser(user, context.Password);
            if (bValid)
            {
                List<Claim> authClaims = user.GetCliams();

                context.Result = new GrantValidationResult(user.Account, "pwd", _clock.UtcNow.UtcDateTime, authClaims);
            }
           
        }
    }
}
