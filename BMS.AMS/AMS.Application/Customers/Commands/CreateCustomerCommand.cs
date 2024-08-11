using AMS.Domain.Customers;
using AMS.Persistence.Shared;
using MediatR;

namespace AMS.Application.Customers.Commands;

public record CreateCustomerCommand(string Name, Guid UserId) : IRequest<Guid>;

public class CreateCustomerCommandHandler(SharedDbContext sharedDbContext) : IRequestHandler<CreateCustomerCommand, Guid>
{
    public async Task<Guid> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var newCustomer = new Customer
        {
            Name = request.Name,
            UserId = request.UserId
        };

        sharedDbContext.Customers.Add(newCustomer);

        await sharedDbContext.SaveChangesAsync(cancellationToken);

        return newCustomer.Id;
    }
}