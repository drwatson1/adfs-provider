using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace DrWatson.Adfs.OAuthProvider
{
    public class AdfsAuthenticationOptions : OAuthOptions
    {
        private string authority;

        public AdfsAuthenticationOptions()
        {
            CallbackPath = new PathString(AdfsAuthenticationDefaults.CallbackPath);
            ClientSecret = "not used";

            Events = new OAuthEvents
            {
                OnRedirectToAuthorizationEndpoint = context =>
                {
                    context.Response.Redirect($"{context.RedirectUri}&resource={Audience}");
                    return Task.FromResult(0);
                }
            };
        }

        public string Audience { get; set; }
        public string Authority
        {
            get => authority;
            set
            {
                authority = value;
                AuthorizationEndpoint = CombineUrl(authority, AdfsAuthenticationDefaults.AuthorizationEndpoint);
                TokenEndpoint = authority + AdfsAuthenticationDefaults.TokenEndpoint;
            }
        }

        private static string CombineUrl(string part1, string part2)
        {
            part1 = part1.TrimEnd('/', ' ');
            part2 = part2.TrimStart('/', ' ');

            return $"{part1}/{part2}";
        }
    }
}