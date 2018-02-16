using DrWatson.Adfs.Metadata;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace DrWatson.Adfs.OAuthProvider
{
    public class AdfsAuthenticationHandler : OAuthHandler<AdfsAuthenticationOptions>
    {
        private IHostingEnvironment env;
        private AdfsMetadataService adfsMetadataService;

        public AdfsAuthenticationHandler(IOptionsMonitor<AdfsAuthenticationOptions> options, 
            ILoggerFactory loggerFactory, 
            UrlEncoder urlEncoder, 
            ISystemClock clock, 
            IHostingEnvironment env, 
            AdfsMetadataService adfsMetadataService)
            : base(options, loggerFactory, urlEncoder, clock)
        {
            this.env = env;
            this.adfsMetadataService = adfsMetadataService;
        }

        protected override async Task<AuthenticationTicket> CreateTicketAsync(ClaimsIdentity identity, 
            AuthenticationProperties properties, 
            OAuthTokenResponse tokens)
        {
            var jwtToken = await DecodeToken(tokens);

            var principal = new ClaimsPrincipal(identity);
            var payload = GetPayload(jwtToken);

            var context = new OAuthCreatingTicketContext(principal, properties, Context, Scheme, Options, Backchannel, tokens, payload);
            context.RunClaimActions();

            await Options.Events.CreatingTicket(context);
            return new AuthenticationTicket(context.Principal, context.Properties, Scheme.Name);
        }
        
        private async Task<ClaimsPrincipal> DecodeToken(OAuthTokenResponse tokens)
        {
            try
            {
                var adfs = await adfsMetadataService.Get();

                JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new X509SecurityKey(adfs.GetSigningCertificate()),
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidAudience = Options.Audience,
                    ValidIssuer = adfs.Identity,
                    RequireSignedTokens = true
                };

                return jwtSecurityTokenHandler.ValidateToken(tokens.AccessToken, validationParameters, out SecurityToken securityToken);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                throw;
            }
        }

        private JObject GetPayload(ClaimsPrincipal principal)
            => principal.Claims.Aggregate(new JObject(), (payload, claim) => { payload.TryAdd(claim.Type, new JValue(claim.Value)); return payload; });
    }
}