using AMS.Application.Accounts.Commands;
using AMS.Application.Accounts.Queries;
using AMS.Common.Constants;
using AMS.Domain.Accounts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AMS.API.Controllers;

[ApiController]
[Route("Accounts")]
public class AccountsController(IMediator mediator) : ControllerBase
{
    [Authorize(Roles = "Employee")]
    [HttpGet("GetBranchAccounts")]
    public async Task<List<Account>> GetBranchAccounts([FromQuery] GetAllBranchAccountsQuery query)
    {
        return await mediator.Send(query);
    }
    
    [Authorize(Policy = PolicyNames.BranchPolicy)]
    [Authorize(Roles = "Employee")]
    [HttpPost("CreateAccount")]
    public async Task<Guid> CreateAccount([FromBody] CreateAccountCommand command, [FromHeader] string branchId)
    {
        return await mediator.Send(command);
    }
   
    [Authorize(Roles = "Employee")]
    [HttpGet("GetCustomerAccounts")]
    public async Task<List<Account>> GetCustomerAccounts([FromQuery] GetAllCustomerAccountsQuery query)
    {
        return await mediator.Send(query);
    }

}