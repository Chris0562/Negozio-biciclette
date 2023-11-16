using Microsoft.AspNetCore.Authorization;

namespace Betacomio.Authentication
{
    public class BasicAutorizationAttribute : AuthorizeAttribute
    {
        public BasicAutorizationAttribute()
        {
            Policy = "BasicAuthentication";
        }
    }
}
