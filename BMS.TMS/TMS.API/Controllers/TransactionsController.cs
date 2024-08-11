using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TMS.Application.Transactions.Commands;
using TMS.Common.Constants;

namespace TMS.API.Controllers;

[ApiController]
[Route("Transactions")]
public class TransactionsController(IMediator mediator, IRecurringJobManager recurringJobManager) : ControllerBase
{
    [Authorize(Roles = "Customer")]
    [HttpPost("MakeTransaction")]
    public async Task<Guid> MakeTransaction([FromBody] CreateTransactionCommand command)
    {
        return await mediator.Send(command);
    }

    [Authorize(Policy = PolicyNames.BranchPolicy)] // custom policy
    [Authorize(Roles = "Employee")]
    [HttpPost("CreateRecurringTransaction")]
    public IActionResult CreateRecurringTransaction([FromHeader] string branchId ,[FromBody] CreateTransactionCommand command, 
        [FromHeader] string subject, 
        [FromHeader] string cronExpression)
    {
        var jobId = $"recurring-transaction-{subject}-{command.Amount}";

        // Schedule the recurring job using the wrapper method
        recurringJobManager.AddOrUpdate(jobId, 
            () => ExecuteTransaction(command), cronExpression);

        return Ok();
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task ExecuteTransaction(CreateTransactionCommand command)
    {
        await mediator.Send(command);
    }
}