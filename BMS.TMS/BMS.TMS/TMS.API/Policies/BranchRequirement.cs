using Microsoft.AspNetCore.Authorization;

namespace TMS.API.Policies;

public class BranchRequirement : IAuthorizationRequirement 
{
}

public class BranchRequirementHandler : AuthorizationHandler<BranchRequirement>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public BranchRequirementHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, BranchRequirement requirement)
    {
        var branchIdInToken = context.User.Claims.FirstOrDefault(c => c.Type == "branchId")?.Value;

        if (branchIdInToken == null)
        {
            context.Fail();
            return Task.CompletedTask;
        }

        var branchIdInRequest = _httpContextAccessor.HttpContext.Request.Headers["branchId"].ToString();

        if (branchIdInToken != branchIdInRequest)
        {
            context.Fail();
            return Task.CompletedTask;
        }

        context.Succeed(requirement);
        return Task.CompletedTask;
    }
}
