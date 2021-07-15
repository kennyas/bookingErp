using IdentityModel;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Tornado.Shared.Helpers;

namespace Tornado.Shared.Identity
{
    public class UserPrincipal : ClaimsPrincipal
    {
        public UserPrincipal(ClaimsPrincipal principal) : base(principal)
        {
        }

        private string GetClaimValue(string key)
        {
            var identity = Identity as ClaimsIdentity;
            if (identity == null)
                return null;

            var claim = identity.Claims.FirstOrDefault(c => c.Type == key);
            return claim?.Value;
        }

        public string UserName
        {
            get
            {
                if (FindFirst(JwtClaimTypes.Name) == null)
                    return string.Empty;

                return GetClaimValue(JwtClaimTypes.Name);
            }
        }

        public string PhoneNumber
        {
            get
            {
                if (FindFirst(JwtClaimTypes.PhoneNumber) == null)
                    return string.Empty;

                return GetClaimValue(JwtClaimTypes.PhoneNumber);
            }
        }

        public string Email
        {
            get
            {
                if (FindFirst(JwtClaimTypes.Email) == null)
                    return string.Empty;

                return GetClaimValue(JwtClaimTypes.Email);
            }
        }

        public Guid UserId
        {
            get
            {
                if (FindFirst(JwtRegisteredClaimNames.Sub) == null)
                    return Guid.Empty;

                return Guid.Parse(GetClaimValue(JwtRegisteredClaimNames.Sub));
            }
        }

        public string FirstName
        {
            get
            {
                var usernameClaim = FindFirst(ClaimTypesHelper.FirstName);

                if (usernameClaim == null)
                    return string.Empty;

                return usernameClaim.Value;
            }
        }

        public string Picture
        {
            get
            {
                var claim = FindFirst(ClaimTypesHelper.Picture);

                if (claim == null)
                    return string.Empty;

                return claim.Value;
            }
        }

        public string LastName
        {
            get
            {
                var usernameClaim = FindFirst(ClaimTypesHelper.LastName);

                if (usernameClaim == null)
                    return string.Empty;

                return usernameClaim.Value;
            }
        }

        public string DialingCode
        {
            get
            {
                var dialingCodeClaim = FindFirst(ClaimTypesHelper.DialingCode);

                if (dialingCodeClaim == null)
                    return string.Empty;

                return dialingCodeClaim.Value;
            }
        }
    }
}