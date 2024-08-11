
using MediatR;
using Microsoft.EntityFrameworkCore;
using TMS.Application.Services;
using TMS.Domain.Accounts;
using TMS.Domain.Transactions;

namespace TMS.Application.Transactions.Commands;

public record CreateTransactionCommand(Guid BranchId, Guid AccountId, long Amount) : IRequest<Guid>;

public class CreateTransactionCommandHandler(DynamicDbContextFactory dynamicDbContextFactory) : IRequestHandler<CreateTransactionCommand, Guid>
{
    public async Task<Guid> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        using var dynamicDbContext = await dynamicDbContextFactory.Create(request.BranchId, cancellationToken);

        var account = await dynamicDbContext.Accounts.FirstOrDefaultAsync(x => x.Id == request.AccountId);

        if (account is null)
        {
            throw new AccountNotFound(request.AccountId);
        }
       
        var transaction = new Transaction
        {
            AccountId = request.AccountId,
            Amount = request.Amount,
            Date = DateOnly.FromDateTime(DateTime.UtcNow)
        };
        
        account.ApplyTransaction(transaction);

        dynamicDbContext.Transactions.Add(transaction);
        
        await dynamicDbContext.SaveChangesAsync(cancellationToken);

        return transaction.Id;
    }
}