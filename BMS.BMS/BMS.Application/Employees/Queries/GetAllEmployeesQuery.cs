using BMS.Application.Services;
using BMS.Domain.Employees;
using BMS.Persistence.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BMS.Application.Employees.Queries;

public record GetAllEmployeesQuery(Guid BranchId) : IRequest<List<Employee>>;

public class GetAllEmployeesQueryHandler(DynamicDbContextFactory dynamicDbContextFactory) : IRequestHandler<GetAllEmployeesQuery, List<Employee>>
{
    public async Task<List<Employee>> Handle(GetAllEmployeesQuery request, CancellationToken cancellationToken)
    {
       using var dynamicDbContext = await dynamicDbContextFactory.Create(request.BranchId, cancellationToken);
       
       var employees = await dynamicDbContext.Employees.ToListAsync(cancellationToken);

       return employees;
    }
}