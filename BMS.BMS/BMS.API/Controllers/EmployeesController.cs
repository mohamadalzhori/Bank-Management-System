using BMS.Application.Employees.Commands;
using BMS.Application.Employees.Queries;
using BMS.Domain.Employees;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BMS.API.Controllers;

[ApiController]
[Route("Employees")]
public class EmployeesController(IMediator mediator) : ControllerBase
{
    [Authorize(Roles = "Admin")]
    [HttpPost("CreateNewEmployee")]
    public async Task<Guid> CreateNewEmployee([FromBody] CreateEmployeeCommand command)
    {
        var employeeId = await mediator.Send(command);

        return employeeId;
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("GetEmployees")]
    public async Task<List<Employee>> GetEmployees([FromQuery] GetAllEmployeesQuery query)
    {
        var employees = await mediator.Send(query);

        return employees;
    }    
}