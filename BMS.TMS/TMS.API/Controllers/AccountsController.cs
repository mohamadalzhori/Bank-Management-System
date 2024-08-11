using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TMS.Application.Accounts;
using TMS.Application.Accounts.Queries;
using TMS.Domain.Accounts;

namespace TMS.API.Controllers;

[ApiController]
[Route("Accounts")]
public class AccountsController(IMediator mediator) : ControllerBase
{
    [Authorize(Roles = "Customer")]
    [HttpGet("GetCustomerAccounts")]
    public async Task<List<GetAccountVM>> GetCustomerAccounts([FromQuery] GetAllCustomerAccountsQuery query)
    {
        return await mediator.Send(query);
    }
}