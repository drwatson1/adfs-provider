namespace DrWatson.Adfs.OAuthProvider
{
    public static class AdfsAuthenticationDefaults
    {
        public static readonly string DisplayName = "ADFS";
        public static readonly string AuthenticationScheme = "adfs3.0-oauth2";
        public static readonly string AuthorizationEndpoint = "/adfs/oauth2/authorize";
        public static readonly string TokenEndpoint = "/adfs/oauth2/token";
        public static readonly string CallbackPath = "/signin-" + AuthenticationScheme;
    }
}