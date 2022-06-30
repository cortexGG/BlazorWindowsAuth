using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace WindowsAuth.Authorization
{
    public static class Handlers
    {
        public class IsDeveloperHandler : AuthorizationHandler<Requirements.IsDeveloperRequirement>
        {
            protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, Requirements.IsDeveloperRequirement requirement)
            {
                context.Succeed(requirement);

                return Task.CompletedTask;
            }
        }
    }
}