using AMS.Application.Branches;
using AMS.Application.Branches.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AMS.API.Controllers;

[ApiController]
[Route("Branches")]
public class BranchesController(IMediator mediator) : ControllerBase
{
   [Authorize(Roles = "Employee")]
   [HttpGet("GetAllBranches")] 
   public async Task<List<GetBranchVM>> GetAllBranches()
   {
       var query = new GetAllBranchesQuery();

       return await mediator.Send(query);
   }
   
}