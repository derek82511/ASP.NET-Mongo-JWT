using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.OAuth;
using Owin;
using VieshowManager.Providers;
using VieshowManager.Models;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security;
using System.Configuration;
using System.Text;
using System.Security.Cryptography;
using VieshowManager.Services.Helpers;

namespace VieshowManager
{
    public partial class Startup
    {
        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        // 如需設定驗證的詳細資訊，請瀏覽 http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // 設定 OAuth 基礎流程的應用程式
            OAuthOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/Token"),
                Provider = new CustomOAuthProvider(),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(7),
                // 在生產模式中設定 AllowInsecureHttp = false
                AllowInsecureHttp = true,
                AccessTokenFormat = new CustomJwtFormat()
            };

            app.UseOAuthAuthorizationServer(OAuthOptions);

            // Api controllers with an [Authorize] attribute will be validated with JWT
            app.UseJwtBearerAuthentication(
                new JwtBearerAuthenticationOptions
                {
                    AuthenticationMode = AuthenticationMode.Active,
                    AllowedAudiences = new[] { JwtHelper.audience },
                    IssuerSecurityTokenProviders = new IIssuerSecurityTokenProvider[]
                    {
                        new SymmetricKeyIssuerSecurityTokenProvider(JwtHelper.issuer, JwtHelper.secret)
                    }
                });
        }
    }
}
