using Microsoft.AspNetCore.Authorization.Infrastructure;
using System.Security.Principal;

namespace Betacomio.Authentication
{
    public class AuthenticatedUser : IIdentity
    {

        public AuthenticatedUser(string authType, bool isAuthenticated, string name, IEnumerable<string> roles)
        {
            AuthenticationType = authType;
            IsAuthenticated = isAuthenticated;
            Name = name;
            Roles = roles;
        }

        public string? AuthenticationType { get; set; }

        public bool IsAuthenticated { get; set; }

        public string? Name { get; set; }

        public IEnumerable<string> Roles { get; set; }
    }
}
