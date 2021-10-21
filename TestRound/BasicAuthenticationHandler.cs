using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace TestRound
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public BasicAuthenticationHandler(
         IOptionsMonitor<AuthenticationSchemeOptions> options,
         ILoggerFactory logger,
         UrlEncoder encoder,
         ISystemClock clock
         )
         : base(options, logger, encoder, clock)
        {
        }

        private List<User> _validUsers = new List<User>
        {
            new User{ Name = "letsgo", Password ="roundthree" },
            new User{ Name = "fabiano",Password = "glasslewiss" },
            new User{ Name = "administrator",Password = "12345" }
        };

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var endpoint = Context.GetEndpoint();
            if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null)
                return Task.FromResult(AuthenticateResult.NoResult());

            if (!Request.Headers.ContainsKey("Authorization"))
                return Task.FromResult(AuthenticateResult.Fail("Missing Authorization Header"));
            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
                var credentials = Encoding.UTF8.GetString(credentialBytes).Split(new[] { ':' }, 2);
                string username = credentials[0];
                string password = credentials[1];
                if (!IsAuthorizedUser(username, password))
                {
                    return Task.FromResult(AuthenticateResult.Fail("Invalid Username or Password"));
                }
                else
                {
                    var authenticatedUser = new AuthenticatedUser("BasicAuthentication", true, "roundthecode");
                    var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(authenticatedUser));

                    return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, Scheme.Name)));
                }
            }
            catch
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Header"));
            }
        }

        public bool IsAuthorizedUser(string username, string password)
        {
            return _validUsers.FirstOrDefault(a => a.Name.ToLower().Equals(username) && a.Password.ToLower().Equals(password)) != null;
        }
    }

    public class User
    {
        public string Name { get; set; }
        public string Password { get; set; }

    }

    public class AuthenticatedUser : IIdentity
    {
        public AuthenticatedUser(string authenticationType, bool isAuthenticated, string name)
        {
            AuthenticationType = authenticationType;
            IsAuthenticated = isAuthenticated;
            Name = name;
        }

        public string AuthenticationType { get; }

        public bool IsAuthenticated { get; }

        public string Name { get; }
    }
}
