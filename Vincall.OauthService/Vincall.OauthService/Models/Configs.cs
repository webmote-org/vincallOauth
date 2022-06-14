using IdentityServer4.Models;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
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
        public static IEnumerable<DataProtectionKey> GetDataProtectionKeys()
        {
            return new List<DataProtectionKey>
            {
                new DataProtectionKey()
                {
                    Id=1,
                    FriendlyName="key-b9f7b7e9-9a43-440e-a847-51ce261472a0",
                    Xml="<key id=\"b9f7b7e9-9a43-440e-a847-51ce261472a0\" version=\"1\"><creationDate>2022-05-26T07:27:32.9831955Z</creationDate><activationDate>2022-05-26T07:27:32.5832188Z</activationDate><expirationDate>2022-08-24T07:27:32.5832188Z</expirationDate><descriptor deserializerType=\"Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel.AuthenticatedEncryptorDescriptorDeserializer, Microsoft.AspNetCore.DataProtection, Version=3.1.25.0, Culture=neutral, PublicKeyToken=adb9793829ddae60\"><descriptor><encryption algorithm=\"AES_256_CBC\" /><validation algorithm=\"HMACSHA256\" /><encryptedSecret decryptorType=\"Microsoft.AspNetCore.DataProtection.XmlEncryption.EncryptedXmlDecryptor, Microsoft.AspNetCore.DataProtection, Version=3.1.25.0, Culture=neutral, PublicKeyToken=adb9793829ddae60\" xmlns=\"http://schemas.asp.net/2015/03/dataProtection\"><EncryptedData Type=\"http://www.w3.org/2001/04/xmlenc#Element\" xmlns=\"http://www.w3.org/2001/04/xmlenc#\"><EncryptionMethod Algorithm=\"http://www.w3.org/2001/04/xmlenc#aes256-cbc\" /><KeyInfo xmlns=\"http://www.w3.org/2000/09/xmldsig#\"><EncryptedKey xmlns=\"http://www.w3.org/2001/04/xmlenc#\"><EncryptionMethod Algorithm=\"http://www.w3.org/2001/04/xmlenc#rsa-1_5\" /><KeyInfo xmlns=\"http://www.w3.org/2000/09/xmldsig#\"><X509Data><X509Certificate>MIIC9zCCAd+gAwIBAgIQlUTJYUORc79ADucokjZfSjANBgkqhkiG9w0BAQsFADASMRAwDgYDVQQDEwd2aW5jYWxsMCAXDTIyMDUyNjAzMTAyMVoYDzIwOTkxMjMwMTYwMDAwWjASMRAwDgYDVQQDEwd2aW5jYWxsMIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAlOheA/QSsVHh8vf6P+NOCtuKgNO41HrF4OraPnGf0r/AL42A37TnlIQfb0Nr2kQIFuKJ7squTiIzjT07K9ZRTi5Gn46B+r8USVZfdp+lo7KkYxRSdYqJowu1mC8tqz/3jjA053dFwklJ/KGt/w+l+82dbh+JtL74gQiZOScx3fRR6RTD7Vyvj/Smn+IDp4Qd4/A14IdWsiljx3Lw4KDZ+ehsE4GMthlZzadOgncDnw1Si38g90ukXFfL118rz7Ue7wEeubtakhnzgQQCTVJzTXcYFdW14mzEHbODIQ62GG4KfBo0fB2z/+cGPz286SNrDskjASWCKKApyWkKaY+OnQIDAQABo0cwRTBDBgNVHQEEPDA6gBANWQ4z4QADbxrnpmQP4FegoRQwEjEQMA4GA1UEAxMHdmluY2FsbIIQlUTJYUORc79ADucokjZfSjANBgkqhkiG9w0BAQsFAAOCAQEAfMLlnC/6LA2EuhqcbnSk+HfCrYRBB8iYoEVp4xjJWPBHgy/c0HWMJVNc/kcL0P3lsRpAKSnli7Wl7byGfQLChRMV3v1GyguMGyWDtLGdjs7iNj1+BBESuG/eTeZNDnzwjic4KYuyhGe+kVgQ/15EfBtuXP5C/xVm5tL/cWJq6i734ropz7qNskKKUFuUaWz80Er3GOrZs8hbRnXNB9tori8DCTYcoJ59bGIxvTieJ2ZpagItSpNdrFsdMQlpy2UC7NRNXe/OvTCzqJefWEJwjcbqF5VUQAINCv5ZEJ6I15YkBVE3JD0W8H2kevlRmV8cQeBKVpvKkygCdlEeLwzbjw==</X509Certificate></X509Data></KeyInfo><CipherData><CipherValue>BzYfOpxOzweG8koOmOwrobTQPBed9jkJ5q148gmUgcYWW8Rpq004V8GsqUAKbuYg9fyV58BnXSQMlWjijee0WmoX0aju8zn44iNHLCU8zD6+eBZSeHHCz46p7und1kl/3ZgyaQZhhDhAT4icB1MfD+ORXxt67Op3SrFt7vtX8U1BPUB900xQFytHyFLVVGRAVCNtvJ/FkbqhUlO4jVyQGXBcUkFkZL27EKq2s6uH7whyIl6oaZiRMuK9drw2ZOTuGizA7xmI5kItT1eB04fDMOfznQ0W9tiGxzVKI1yg9pvEVHTr4IMctGwNYfpuqJR0cPYo4ucLU9ILUdhRBymphg==</CipherValue></CipherData></EncryptedKey></KeyInfo><CipherData><CipherValue>RpcIwa4m1tee8CxXA/gwq3zN6ARxEufJIrUE2jjYQaewDQIv7eKugSPADHMytCicSpz21/yfIIV1GRz1GBYZ1wgWFRokGXgAFM+taith8FmR1loFpvpqPRU7QR4NtOboa4Kx3Q9nwHqLyrULd0zHlubC/z2W4RU5XVlOPATZcoySkb4LdMWiqzoyQfoVUMFmI1NQzScUHJ6PXj5ZOCD8HtQfyiTJZySkHuAkToyXWnmyaxRyXp04qLiFEz9ocZaymT52NWMVtGKNdWCqzis09DutnIIQxgpwry/lz1PxyM7TwJKlOhdicjxz8U9sY2K8tRI8w542d7h7F6HdupY6qa97zqTg4YT8/MmRFBpnB26NphTmLwpfaEtZ5PBrBChBcdMs5CkV76Bb4fWTaJ0fRw==</CipherValue></CipherData></EncryptedData></encryptedSecret></descriptor></descriptor></key>"
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
