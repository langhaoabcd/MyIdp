using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;
using System.Security.Claims;

namespace MyIdp
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResources.Address(),
                new IdentityResources.Phone(),
            };
        // v4新增
        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("test1Api", "My test1Api"),
            };
        public static IEnumerable<ApiResource> ApiResources =>
         new ApiResource[]
         {
             //new ApiResource("test1Api", "test1 API")
         };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientId = "console client",
                    ClientName = "Client Credentials Client",

                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },

                    AllowedScopes = {
                        "test1Api"
                    }
                },
                new Client
                {
                    ClientId = "winform client",
                    ClientName = "winform Client",

                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets = { new Secret("winform secret".Sha256()) },

                    AllowedScopes = {
                        "test1Api",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                    }
                },
                new Client
                {
                    ClientId = "phone_number_auth_client",
                    AllowedGrantTypes = new List<string>{ "phone_number"},//手机号登录授权类型
                    ClientSecrets = { new Secret("phone_number_aut_secret".Sha256()) },

                    AllowedScopes = {
                        "test1Api",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Phone,
                    }
                },
                new Client
                {
                    ClientId = "mvc client",
                    ClientName = "mvc Client",
                    //RequireConsent = true,
                    AllowedGrantTypes = GrantTypes.Code,
                    ClientSecrets = { new Secret("mvc secret".Sha256()) },
                    RedirectUris = { "http://localhost:5002/signin-oidc" },
                    FrontChannelLogoutUri = "http://localhost:5002/signout-oidc",
                    PostLogoutRedirectUris = { "http://localhost:5002/signout-callback-oidc" },
                    AllowedScopes = {
                        "test1Api",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.Address,
                        IdentityServerConstants.StandardScopes.Phone,
                    },
                   AlwaysIncludeUserClaimsInIdToken = true,// 总是允许返回的Idtoken中包含UserClamins
                   AllowOfflineAccess = true, //总是包含refresh_token
                   AccessTokenLifetime = 60 * 60,//单位s 默认的token有效期是1小时?
                },
                new Client
                {
                    ClientId = "angular-client",
                    ClientName = "Angular SPA Client",
                    ClientUri = "http://localhost:4200",

                    AllowedGrantTypes = GrantTypes.Implicit,
                    //允许token 是通过浏览器发送给客户端的，这里必须启用
                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = true,
                    AccessTokenLifetime = 60 * 5,
                    //注销重定向的url
                    PostLogoutRedirectUris = { "http://localhost:4200/dashboard" },
                    RedirectUris =
                    {
                        "http://localhost:4200/signin-oidc",// 登录uri
                        //"http://localhost:4200/redirect-silentrenew"// 刷新token
                    },
                    AllowedCorsOrigins =
                    {
                        "http://localhost:4200" //允许js客户端跨域
                    },
                    AllowedScopes =
                    {
                         "test1Api",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.Address,
                        IdentityServerConstants.StandardScopes.Phone,
                    }
                }
            };
    }
}