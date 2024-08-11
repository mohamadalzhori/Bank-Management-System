using AMS.Persistence.Shared;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AMS.Application.Customers.Queries;

public record GetAllCustomersQuery : IRequest<List<GetCustomerVM>>;

public class GetAllCustomersQueryHandler(SharedDbContext sharedDbContext, IMapper mapper) : IRequestHandler<GetAllCustomersQuery, List<GetCustomerVM>>
{
    public Task<List<GetCustomerVM>> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
    {
        return sharedDbContext.Customers
            .ProjectTo<GetCustomerVM>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}