using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace PNMS.Security.Handlers
{
    public class AuthenticationHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var validToken = false;
            var response = new HttpResponseMessage();

            if (TokenExists(request, out string token))
            {
                try
                { validToken = await ValidateTokenAsync(token); } // This section can be greatly expanded to give more custom error messages.
                catch (SecurityTokenExpiredException) { /* Handle */ }
                catch (SecurityTokenValidationException) { /* Handle */ }
                catch (SecurityTokenException) { /* Handle */ }
                catch (Exception) { /* Handle */ }

                if (validToken) // Happy Path For Valid JWT
                { response = await base.SendAsync(request, cancellationToken); }
                else
                { response.StatusCode = HttpStatusCode.Unauthorized; }
            }
            else // Optional Public access as Anonymous
            {
                response.StatusCode = HttpStatusCode.Unauthorized;
                response = await base.SendAsync(request, cancellationToken);
            }

            return response;
        }

        private bool TokenExists(HttpRequestMessage request, out string token)
        {
            var tokenFound = false;
            token = null;

            if (request.Headers.TryGetValues("Authorization", out IEnumerable<string> authHeaders) && authHeaders.Any())
            {
                var bearerToken = authHeaders.ElementAt(0);

                // Authorization Header Flexibility
                // Authorization value start with "Bearer " then trim it off, else treat value as Token.
                token = bearerToken.StartsWith("Bearer ") ? bearerToken.Substring(7) : bearerToken;
                tokenFound = true;
            }

            return tokenFound;
        }

        private async Task<bool> ValidateTokenAsync(string token)
        {
            var userIsValid = true; // assumed user is good (but could be false)
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            var tvp = GetTokenValidationParameters();

            //Extract and assigns the PrincipalId from JWT User/Claims.
            HttpContext.Current.User = Thread.CurrentPrincipal = jwtSecurityTokenHandler.ValidateToken(token, tvp, out SecurityToken securityToken);

            // TODO: Extra Validate the UserId, check for user still exists, user isn't banned, user registered email etc.
            //userIsValid = await _userService.ApiUserGetValidationAsync(HttpContext.Current.User.GetUserId());
            // GetUserId() is an extension method I wrote so you will need to write something like
            //this for yourself.

            if (!userIsValid) throw new SecurityTokenValidationException();

            return await Task.FromResult(userIsValid);
        }

        private TokenValidationParameters GetTokenValidationParameters()
        {
            // Cleanup
            return new TokenValidationParameters
            {
                ValidAudience = "https://houseofcat.io", // Remember to copy your own.
                ValidIssuer = "https://houseofcat.io", // Also don't leave them as magic strings, load them from the app.config.
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                LifetimeValidator = this.LifetimeValidator,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.Default // Seriously don't copy this string. It's bad for you. Stop. Go here: https://passwordsgenerator.net/
                    .GetBytes("J6k2eVCTXDp5b97u6gNH5GaaqHDxCmzz2wv3PRPFRsuW2UavK8LGPRauC4VSeaetKTMtVmVzAC8fh8Psvp8PFybEvpYnULHfRpM8TA2an7GFehrLLvawVJdSRqh2unCnWehhh2SJMMg5bktRRapA8EGSgQUV8TCafqdSEHNWnGXTjjsMEjUpaxcADDNZLSYPMyPSfp6qe5LMcd5S9bXH97KeeMGyZTS2U8gp3LGk2kH4J4F3fsytfpe9H9qKwgjb"))
            };
        }

        private bool LifetimeValidator(
            DateTime? notBefore,
            DateTime? expires,
            SecurityToken securityToken,
            TokenValidationParameters validationParameters)
        {
            var valid = false;

            // Additional checks can be performed on the SecurityToken or the validationParameters.
            if ((expires.HasValue && DateTime.UtcNow < expires)
             && (notBefore.HasValue && DateTime.UtcNow > notBefore))
            { valid = true; }

            return valid;
        }
    }
}
