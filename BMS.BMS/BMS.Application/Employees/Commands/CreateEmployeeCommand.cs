using BMS.Application.Services;
using BMS.Domain.Employees;
using BMS.Persistence.Shared;
using MediatR;

namespace BMS.Application.Employees.Commands;

public record CreateEmployeeCommand(string EmployeeName, Guid UserId, Guid BranchId) : IRequest<Guid>;

public class CreateEmployeeCommandHandler(DynamicDbContextFactory dynamicDbContextFactory) : IRequestHandler<CreateEmployeeCommand, Guid>
{
    public async Task<Guid> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var newEmployee = new Employee
        {
            Name = request.EmployeeName,
            UserId = request.UserId
        };

        using var dynamicDbContext = await dynamicDbContextFactory.Create(request.BranchId, cancellationToken);
        
        dynamicDbContext.Employees.Add(newEmployee);
        
        await dynamicDbContext.SaveChangesAsync(cancellationToken);
        
        return newEmployee.Id;
    }
}