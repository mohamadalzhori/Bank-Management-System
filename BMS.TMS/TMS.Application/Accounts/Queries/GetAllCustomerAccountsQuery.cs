using MediatR;
using Microsoft.EntityFrameworkCore;
using TMS.Application.Services;
using TMS.Domain.Accounts;
using TMS.Persistence.Shared;

namespace TMS.Application.Accounts.Queries;

public record GetAllCustomerAccountsQuery(Guid CustomerId) : IRequest<List<GetAccountVM>>;

public class GetAllCustomerAccountsQueryHandler(
    SharedDbContext sharedDbContext,
    DynamicDbContextFactory dynamicDbContextFactory) : IRequestHandler<GetAllCustomerAccountsQuery, List<GetAccountVM>>
{
    public async Task<List<GetAccountVM>> Handle(GetAllCustomerAccountsQuery request, CancellationToken cancellationToken)
    {
        var accounts = new List<GetAccountVM>();

        // get all branches and then for each branch get a new dynamic db context and then get all accounts

        var branches = sharedDbContext.Branches.ToList();

        foreach (var branch in branches)
        {
            using var dynamicDbContext = await dynamicDbContextFactory.Create(branch.Id, cancellationToken);

            var branchAccounts = await dynamicDbContext.Accounts
                .Where(x => x.CustomerId == request.CustomerId)
                .Select(x => new GetAccountVM
                {
                    Id = x.Id,
                    BranchId = branch.Id,
                    Balance = x.AccountBalance
                })
                .ToListAsync();

            // add the branch accounts to the accounts list
            accounts.AddRange(branchAccounts);
        }

        return accounts;
    }
}