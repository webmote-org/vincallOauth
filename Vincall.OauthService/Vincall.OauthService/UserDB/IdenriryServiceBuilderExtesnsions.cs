using IdentityServer4.Test;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vincall.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace Vincall.OauthService.UserDB
{
    public static class UserIdenriryServiceBuilderExtesnsions
    {
        public static IIdentityServerBuilder AddVincallUsers(this IIdentityServerBuilder builder, string connectionString)
        {
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            builder.Services.AddDbContextPool<VincallDBContext>((sp, builder) =>
            {
                builder.UseSqlServer(connectionString, b => {
                    b.MigrationsAssembly(migrationsAssembly);
                });
                builder.UseInternalServiceProvider(sp);
            });
            builder.AddProfileService<VincallUserProfileService>();
            builder.AddResourceOwnerValidator<VincallUserResourceOwnerPasswordValidator>();
            return builder;
        }
    }
}
