using MediatR;
using TMS.Application.Services;
using TMS.Persistence.Dynamic;
using TMS.Persistence.Shared;

namespace TMS.Application.Transactions.Commands;

public record RollBackTransactionsCommand(DateOnly RollBackDate) : IRequest;

public class RollBackTransactionsCommandHandler(SharedDbContext sharedDbContext, DynamicDbContextFactory dynamicDbContextFactory) : IRequestHandler<RollBackTransactionsCommand>
{
    public async Task Handle(RollBackTransactionsCommand request, CancellationToken cancellationToken)
    {
        var branches = sharedDbContext.Branches.ToList();

        foreach (var branch in branches)
        {
           using var dynamicDbContext = await dynamicDbContextFactory.Create(branch.Id, cancellationToken); 
           
           var accounts = dynamicDbContext.Accounts.ToList();

           foreach (var account in accounts)
           {
                var transactions = dynamicDbContext.Transactions
                     .Where(t => t.AccountId == account.Id && t.Date == request.RollBackDate)
                     .ToList();
  
                long totalAmount = 0;

                foreach (var transaction in transactions)
                {
                   totalAmount += transaction.Amount; 
                }
               
                account.AccountBalance -= totalAmount;
                
           }
       
           await dynamicDbContext.SaveChangesAsync(cancellationToken);
        }
      
    }
}