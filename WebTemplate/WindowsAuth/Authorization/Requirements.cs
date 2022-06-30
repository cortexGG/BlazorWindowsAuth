using Microsoft.AspNetCore.Authorization;

namespace WindowsAuth.Authorization
{
    public static class Requirements
    {
        public class IsDeveloperRequirement : IAuthorizationRequirement
        {
            
        }
    }
}