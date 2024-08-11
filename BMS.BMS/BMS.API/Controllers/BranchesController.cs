using BMS.Application.Branches;
using BMS.Application.Branches.Commands;
using BMS.Application.Branches.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BMS.API.Controllers;

[ApiController]
[Route("Branches")]
public class BranchesController(IMediator mediator) : ControllerBase
{
    [Authorize(Roles = "Admin")]
    [HttpPost("CreateNewBranch")]
    public async Task<Guid> CreateNewBranch([FromBody] CreateBranchCommand command)
    {
        var branchId = await mediator.Send(command);

        return branchId;
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("GetBranches")]
    public async Task<List<GetBranchVM>> GetBranches()
    {
        var query = new GetAllBranchesQuery();
        var branches = await mediator.Send(query);

        return branches;
    } 
}