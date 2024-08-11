using AMS.Application.RabbitMQ.Account;
using AMS.Application.Services;
using AMS.Domain.Accounts;
using AMS.Domain.Branches;
using AMS.Domain.Customers;
using AMS.Persistence.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AMS.Application.Accounts.Commands;

public record CreateAccountCommand(Guid BranchId ,Guid CustomerId) : IRequest<Guid>;

public class CreateAccountCommandHandler(SharedDbContext sharedDbContext, DynamicDbContextFactory dynamicDbContextFactory, IRabbitMqProducer rabbitMqProducer) : IRequestHandler<CreateAccountCommand, Guid>
{
    public async Task<Guid> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        var customerExists = await sharedDbContext.Customers.AnyAsync(c => c.Id == request.CustomerId);

        if (!customerExists)
        {
            throw new CustomerNotFound(request.CustomerId);
        }
      
        var branchExists = await sharedDbContext.Branches.AnyAsync(b => b.Id == request.BranchId);

        if (!branchExists)
        {
            throw new BranchNotFound(request.BranchId);
        }
        
        using var dynamicDbContext = await dynamicDbContextFactory.Create(request.BranchId, cancellationToken); 
      
        var account = new Account
        {
            CustomerId = request.CustomerId
        };

        dynamicDbContext.Accounts.Add(account);

        await dynamicDbContext.SaveChangesAsync(cancellationToken);

        // Increment Customer Accounts Count
        var customer = sharedDbContext.Customers.FirstOrDefault(c => c.Id == request.CustomerId); 
        customer!.IncrementAccountCount(); 
        
        await sharedDbContext.SaveChangesAsync(cancellationToken);
        
        // Publish an event to notify the other services that a new account has been created 
        rabbitMqProducer.PublishAccountCreated(request.BranchId, request.CustomerId);
        
        return account.Id;
    }
}