using AMS.Application.Services;
using AMS.Domain.Accounts;
using AMS.Domain.Branches;
using AMS.Persistence.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AMS.Application.Accounts.Queries;

public record GetAllBranchAccountsQuery(Guid BranchId) : IRequest<List<Account>>;

public class GetAllBranchAccountsQueryHandler(SharedDbContext sharedDbContext, DynamicDbContextFactory dynamicDbContextFactory) : IRequestHandler<GetAllBranchAccountsQuery, List<Account>>
{
    public async Task<List<Account>> Handle(GetAllBranchAccountsQuery request, CancellationToken cancellationToken)
    {
        var branhExists = sharedDbContext.Branches.Any(b => b.Id == request.BranchId);
        
        if (!branhExists)
        {
            throw new BranchNotFound(request.BranchId);
        }
        
        using var dynamicDbContext = await dynamicDbContextFactory.Create(request.BranchId, cancellationToken);

        return await dynamicDbContext.Accounts.ToListAsync();
    }
}