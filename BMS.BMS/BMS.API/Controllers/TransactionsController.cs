using BMS.Infrastructure.Grpc.Services;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BMS.API.Controllers;

[ApiController]
[Route("Transactions")]
public class TransactionsController(RollBackService rollBackService) : ControllerBase
{
   // [Authorize(Roles = "Admin")]
   [HttpPost("RollBackTransactions")]
   public async Task<IActionResult> RollBackTransactions([FromQuery] DateTime date)
   {
      var dateOnly = DateOnly.FromDateTime(date);
      await rollBackService.RollbackTransactions(dateOnly);
      return Ok();
   } 
}