using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DrWatson.Adfs.OAuthProvider
{
    /// <summary>
    /// Extension methods to add Asana authentication capabilities to an HTTP application pipeline.
    /// </summary>
    public static class AdfsAuthenticationExtensions
    {
        /// <summary>
        /// Adds <see cref="AdfsAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Asana authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static AuthenticationBuilder AddAdfs(this AuthenticationBuilder builder)
        {
            return builder.AddAdfs(AdfsAuthenticationDefaults.AuthenticationScheme, options => { });
        }

        /// <summary>
        /// Adds <see cref="AdfsAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Asana authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="configuration">The delegate used to configure the OpenID 2.0 options.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static AuthenticationBuilder AddAdfs(
            this AuthenticationBuilder builder,
            Action<AdfsAuthenticationOptions> configuration)
        {
            return builder.AddAdfs(AdfsAuthenticationDefaults.AuthenticationScheme, configuration);
        }

        /// <summary>
        /// Adds <see cref="AdfsAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Asana authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the Asana options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddAdfs(
            this AuthenticationBuilder builder, string scheme,
            Action<AdfsAuthenticationOptions> configuration)
        {
            return builder.AddAdfs(scheme, AdfsAuthenticationDefaults.DisplayName, configuration);
        }

        /// <summary>
        /// Adds <see cref="AdfsAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Asana authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="caption">The optional display name associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the Asana options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddAdfs(
            this AuthenticationBuilder builder,
            string scheme, string caption,
            Action<AdfsAuthenticationOptions> configuration)
        {
            return builder.AddOAuth<AdfsAuthenticationOptions, AdfsAuthenticationHandler>(scheme, caption, configuration);
        }
    }
}