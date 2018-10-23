using System;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using AspNetCore.Sample.DesktopUtils;
using Microsoft.AspNetCore.Authentication.JwtBearer;


namespace Microsoft.AspNetCore.Authentication
{
    public static class AzureAdAuthenticationBuilderExtensions
    {
        public static AuthenticationBuilder AddAzureAd(this AuthenticationBuilder builder)
            => builder.AddAzureAd(_ => { });

        public static AuthenticationBuilder AddAzureAd(this AuthenticationBuilder builder, Action<AzureAdOptions> configureOptions)
        {
            builder.Services.Configure(configureOptions);
            builder.Services.AddSingleton<IConfigureOptions<OpenIdConnectOptions>, ConfigureAzureOptions>();
            builder.AddOpenIdConnect();
            return builder;
        }

        public static AuthenticationBuilder AddAzureAdBearer(this AuthenticationBuilder builder)
            => builder.AddAzureAdBearer(_ => { });

        public static AuthenticationBuilder AddAzureAdBearer(this AuthenticationBuilder builder, Action<AzureAdOptions> configureOptions)
        {
            builder.Services.Configure(configureOptions);
            builder.Services.AddSingleton<IConfigureOptions<JwtBearerOptions>, ConfigureAzureOptionsJwt>();
            builder.AddJwtBearer();
            return builder;
        }

        private class ConfigureAzureOptionsJwt : IConfigureNamedOptions<JwtBearerOptions>
        {
            private readonly AzureAdOptions _azureOptions;

            public ConfigureAzureOptionsJwt(IOptions<AzureAdOptions> azureOptions)
            {
                _azureOptions = azureOptions.Value;
            }

            public void Configure(string name, JwtBearerOptions options)
            {
                options.Audience = _azureOptions.ClientId;
                options.Authority = _azureOptions.Authority;
            }

            public void Configure(JwtBearerOptions options)
            {
                Configure(Options.DefaultName, options);
            }
        }


        private class ConfigureAzureOptions : IConfigureNamedOptions<OpenIdConnectOptions>
        {
            private readonly AzureAdOptions _azureOptions;

            public ConfigureAzureOptions(IOptions<AzureAdOptions> azureOptions)
            {
                _azureOptions = azureOptions.Value;
            }

            public void Configure(string name, OpenIdConnectOptions options)
            {
                options.ClientId = _azureOptions.ClientId;
                options.Authority = _azureOptions.Authority;
                options.UseTokenLifetime = true;
                options.CallbackPath = _azureOptions.CallbackPath;
                options.RequireHttpsMetadata = false;
                options.ClientSecret = _azureOptions.ClientSecret;
                options.ResponseType = "id_token code";
                // options.Resource = _azureOptions.Resource;
                options.SaveTokens = true;
                options.GetClaimsFromUserInfoEndpoint = true;
                options.Events.OnAuthorizationCodeReceived = OnAuthorizationCodeReceived;
            }

            public void Configure(OpenIdConnectOptions options)
            {
                Configure(Options.DefaultName, options);
            }

            /// <summary>
            /// Redeems the authorization code by calling AcquireTokenByAuthorizationCodeAsync in order to ensure
            /// that the cache has a token for the signed-in user, which will then enable the controllers (like the
            /// TodoController, to call AcquireTokenSilentAsync successfully.
            /// </summary>
            private async Task OnAuthorizationCodeReceived(AuthorizationCodeReceivedContext context)
            {
                // Acquire a Token for the Graph API and cache it using ADAL. In the TodoListController, we'll use the cache to acquire a token for the Todo List API
                string userObjectId = (context.Principal.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier"))?.Value;
                var authContext = new AuthenticationContext(context.Options.Authority, new NaiveSessionCache(userObjectId, context.HttpContext.Session));
                var credential = new ClientCredential(context.Options.ClientId, context.Options.ClientSecret);

                var authResult = await authContext.AcquireTokenByAuthorizationCodeAsync(context.TokenEndpointRequest.Code,
                    new Uri(context.TokenEndpointRequest.RedirectUri, UriKind.RelativeOrAbsolute), credential);

                // Notify the OIDC middleware that we already took care of code redemption.
                context.HandleCodeRedemption(authResult.AccessToken, context.ProtocolMessage.IdToken);
            }

        }
    }
}
