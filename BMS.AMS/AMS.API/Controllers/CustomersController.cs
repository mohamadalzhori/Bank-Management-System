using AMS.Application.Customers.Commands;
using AMS.Application.Customers.Queries;
using AMS.Common.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AMS.API.Controllers;

[ApiController]
[Route("Customers")]
public class CustomersController(IMediator mediator) : ControllerBase
{
    [Authorize(Roles = "Employee")]
    [HttpGet("GetAllCustomers")]
    public async Task<List<GetCustomerVM>> GetAllCustomers()
    {
        var query = new GetAllCustomersQuery();

        return await mediator.Send(query);
    }
    
    [Authorize(Roles = "Employee")]
    [HttpPost("CreateCustomer")]
    public async Task<Guid> CreateCustomer([FromBody] CreateCustomerCommand command)
    {
        return await mediator.Send(command);
    }
}