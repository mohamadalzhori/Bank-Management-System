using AMS.Application.Services;
using AMS.Domain.Accounts;
using AMS.Domain.Customers;
using AMS.Persistence.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AMS.Application.Accounts.Queries;

public record GetAllCustomerAccountsQuery(Guid CustomerId) : IRequest<List<Account>>;

public class GetAllCustomerAccountsQueryHandler(SharedDbContext sharedDbContext, DynamicDbContextFactory dynamicDbContextFactory) : IRequestHandler<GetAllCustomerAccountsQuery, List<Account>>
{
    public async Task<List<Account>> Handle(GetAllCustomerAccountsQuery request, CancellationToken cancellationToken)
    {
        var customerExists = sharedDbContext.Customers.Any(c => c.Id == request.CustomerId);
        
        if (!customerExists)
        {
            throw new CustomerNotFound(request.CustomerId);
        }
      
        var accounts = new List<Account>();
        
        // get all branches and then for each branch get a new dynamic db context and then get all accounts
       
        var branches = sharedDbContext.Branches.ToList();

        foreach (var branch in branches)
        {
           using var dynamicDbContext = await dynamicDbContextFactory.Create(branch.Id, cancellationToken); 
           
           var branchAccounts = await dynamicDbContext.Accounts.Where(x=>x.CustomerId == request.CustomerId).ToListAsync();
           
           // add the branch accounts to the accounts list
           accounts.AddRange(branchAccounts);
        }

        return accounts;

    }
}