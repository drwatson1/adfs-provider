# OAuth2 authentication provider for ADFS 3.0
[![NuGet Status](https://img.shields.io/nuget/v/DrWatson.Adfs.OAuthProvider.svg?style=flat)](https://www.nuget.org/packages/DrWatson.Adfs.OAuthProvider)

This is the ASP.Net Core 2.0 authentication provider. It can be used to authenticate users against the on-premise ADFS 3.0 which is part of 
Microsoft Windows Server 2012 R2 via its OAuth endpoint.

## Installing

```powershell
Install-Package DrWatson.Adfs.OAuthProvider
```

## Usage

This provider can be the most useful in conjunction with [Identity Server 4](https://github.com/IdentityServer/IdentityServer4) to create the "Federation Gateway" to allow the users be authenticated against the local Active Directory via Active Directory Fedefation Services. ADFS 3.0 has limited implementation of OAuth2 protocol. With the Identity Server we can create the token provider service with support of OpenId Connect 1.0 protocol.

```csharp
services.AddAuthentication()
  .AddAdfs(options =>
  {
    options.AdfsUrl = "https://fs.example.com";
    options.AdfsAudience = "idsrv-aud";
    options.ClientId = "idsrv";
    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
  });
```
