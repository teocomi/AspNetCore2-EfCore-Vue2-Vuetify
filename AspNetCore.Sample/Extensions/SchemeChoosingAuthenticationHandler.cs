using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Text.Encodings.Web;


using Microsoft.Extensions.Logging;

namespace Microsoft.AspNetCore.Authentication
{
    public class SchemeChoosingAuthenticationHandler : AuthenticationHandler<SchemeChoosingAuthenticationOptions>
    {
        public SchemeChoosingAuthenticationHandler(IOptionsMonitor<SchemeChoosingAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock) { }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var chosenScheme = Options.SchemeChooser.Invoke(Context);
            var chosenChallengeScheme = Options.ChallengeSchemeChooser.Invoke(Context, chosenScheme);

            var result = await Context.AuthenticateAsync(chosenScheme);

            return result;
        }

        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            var request = Context.Request;

            var chosenScheme = Options.SchemeChooser.Invoke(Context);
            var chosenChallengeScheme = Options.ChallengeSchemeChooser.Invoke(Context, chosenScheme);

            await Context.ChallengeAsync(chosenChallengeScheme, properties);
        }
    }

    public static class SchemeChoosingAuthenticationExtensions
    {
        public static AuthenticationBuilder AddSchemeChoosingAuthentication(this AuthenticationBuilder builder, string authenticationScheme, string displayName, Action<SchemeChoosingAuthenticationOptions> configureOptions)
        {
            return builder.AddScheme<SchemeChoosingAuthenticationOptions, SchemeChoosingAuthenticationHandler>(authenticationScheme, displayName, configureOptions);
        }
    }

    public class SchemeChoosingAuthenticationOptions : AuthenticationSchemeOptions
    {
        public Func<HttpContext, string> SchemeChooser { get; set; }
        public Func<HttpContext, string, string> ChallengeSchemeChooser { get; set; }
    }
}
