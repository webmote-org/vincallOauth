using IdentityServer4.Models;
using System.Collections.Generic;

namespace Vincall.OauthService.Models
{
    public class Configs
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
            };
        }

        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>
            {
                // backward compat
                new ApiScope("api"),
                new ApiScope("connectapi"),
                
                // more formal
                new ApiScope("api.scope1"),
                new ApiScope("api.scope2"),
                
                // scope without a resource
                new ApiScope("connectapi"),
                
                // policyserver
                new ApiScope("policyserver.runtime"),
                new ApiScope("policyserver.management")
            };
        }

        public static IEnumerable<ApiResource> GetApis()
        {
            return new List<ApiResource>
            {
                new ApiResource("api", "Demo API")
                {
                    ApiSecrets = { new Secret("secret".Sha256()) },

                    Scopes = { "api", "api.scope1", "api.scope2" }
                }
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {               
                new Client
                {
                    ClientId = "vincall",
                    ClientName = "vincall client ",

                    RedirectUris = { "https://notused" },
                    PostLogoutRedirectUris = { "https://notused" },
                    ClientSecrets = { new Secret("vincall".Sha256()) },
                    RequireClientSecret = true,

                    AllowedGrantTypes = GrantTypes.Code,
                    AllowedScopes = { "openid", "profile", "email", "api" },

                    AllowOfflineAccess = true,
                    RefreshTokenUsage = TokenUsage.OneTimeOnly,
                    RefreshTokenExpiration = TokenExpiration.Sliding,
                    RequirePkce = false,
                },                
                // oidc login only
                new Client
                {
                    ClientId = "login",
                    RedirectUris = { "https://vincall.net" },
                    PostLogoutRedirectUris = { "https://vincall.net" },
                    RequireClientSecret = false,
                    AllowOfflineAccess = true,
                    RefreshTokenUsage = TokenUsage.OneTimeOnly,
                    RefreshTokenExpiration = TokenExpiration.Sliding,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    AlwaysSendClientClaims = true,

                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowedScopes = { "openid", "profile", "email", "api" },
                },
                //openapi token
                new Client
                {
                    ClientId = "connect",
                    RedirectUris = { "https://notused" },
                    ClientSecrets = { new Secret("vincall.net.2022".Sha256()) },                
                    RequirePkce =false,
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = { "openid", "profile", "email", "connectapi" },
                },

            };
        }
    }
}
