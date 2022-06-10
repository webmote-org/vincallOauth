using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vincall.Infrastructure;

namespace Vincall.OauthService.UserDB
{
    public class VincallUserProfileService : IProfileService
    {
      
        protected readonly ILogger Logger;
        private readonly IMemoryCache _cache;
        protected readonly VincallDBContext _dbContext;


        public VincallUserProfileService(VincallDBContext dbContext, ILogger<VincallUserProfileService> logger, IMemoryCache cache)
        {
            _dbContext = dbContext;
            Logger = logger;
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
        public virtual  async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            context.LogProfileRequest(Logger);
            var user = await GetUserByAccountAsync(context.Subject.GetSubjectId());
            if (user != null)
            {
                context.IssuedClaims.AddRange(user.GetCliams());
                //context.AddRequestedClaims(user.GetCliams());
            }
            context.LogIssuedClaims(Logger);           
        }

        public virtual async Task IsActiveAsync(IsActiveContext context)
        {
            LoggerExtensions.LogDebug(Logger, "IsActive called from: {caller}", new object[1]
            {
                context.Caller
            });
            var user = await GetUserByAccountAsync(context.Subject.GetSubjectId());
            context.IsActive = (user!=null);            
        }
    }
}
