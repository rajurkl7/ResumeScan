using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text.Encodings.Web;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace ResumeScanApi.Authentication;

public sealed class BasicAuthenticationOptions : AuthenticationSchemeOptions
{
    public const string SectionName = "BasicAuthentication";
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public sealed class BasicAuthenticationHandler(
    IOptionsMonitor<BasicAuthenticationOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder) : AuthenticationHandler<BasicAuthenticationOptions>(options, logger, encoder)
{
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue("Authorization", out StringValues authorizationHeader))
            return Task.FromResult(AuthenticateResult.NoResult());

        try
        {
            var credentials = AuthenticationHeaderValue.Parse(authorizationHeader.ToString());
            if (!credentials.Scheme.Equals("Basic", StringComparison.OrdinalIgnoreCase) || credentials.Parameter is null)
                return Task.FromResult(AuthenticateResult.Fail("Invalid Authorization scheme."));

            var decoded = Encoding.UTF8.GetString(Convert.FromBase64String(credentials.Parameter));
            var separator = decoded.IndexOf(':');
            if (separator <= 0)
                return Task.FromResult(AuthenticateResult.Fail("Invalid Basic Authentication credentials."));

            var username = decoded[..separator];
            var password = decoded[(separator + 1)..];
            var configured = Options;
            if (configured is null || !CryptographicOperations.FixedTimeEquals(Encoding.UTF8.GetBytes(username), Encoding.UTF8.GetBytes(configured.Username)) || !CryptographicOperations.FixedTimeEquals(Encoding.UTF8.GetBytes(password), Encoding.UTF8.GetBytes(configured.Password)))
                return Task.FromResult(AuthenticateResult.Fail("Invalid username or password."));

            var claims = new[] { new Claim(ClaimTypes.Name, username) };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(new ClaimsPrincipal(identity), Scheme.Name)));
        }
        catch (FormatException)
        {
            return Task.FromResult(AuthenticateResult.Fail("Invalid Basic Authentication credentials."));
        }
    }

    protected override Task HandleChallengeAsync(AuthenticationProperties properties)
    {
        Response.Headers.WWWAuthenticate = "Basic realm=ResumeScanApi";
        return base.HandleChallengeAsync(properties);
    }
}
