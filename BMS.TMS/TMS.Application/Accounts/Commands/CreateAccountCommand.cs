using MediatR;
using Microsoft.EntityFrameworkCore;
using TMS.Application.Services;
using TMS.Domain.Accounts;
using TMS.Domain.Branches;
using TMS.Persistence.Shared;

namespace TMS.Application.Accounts.Commands;

public record CreateAccountCommand(Guid BranchId, Guid CustomerId) : IRequest<Guid>;

public class CreateAccountCommandHandler(DynamicDbContextFactory dynamicDbContextFactory, SharedDbContext sharedDbContext) : IRequestHandler<CreateAccountCommand, Guid>
{
    public async Task<Guid> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        // var branchExists = await sharedDbContext.Branches.FirstOrDefaultAsync(x => x.Id == request.BranchId, cancellationToken);
        //
        // if (branchExists is null)
        // {
        //     throw new BranchNotFound(request.BranchId);
        // }
         
        using var dynamicDbContext = await dynamicDbContextFactory.Create(request.BranchId, cancellationToken);
        
        var account = new Account
        {
            CustomerId = request.CustomerId,
        };

        dynamicDbContext.Accounts.Add(account);
        await dynamicDbContext.SaveChangesAsync(cancellationToken);

        return account.Id;
    }
}