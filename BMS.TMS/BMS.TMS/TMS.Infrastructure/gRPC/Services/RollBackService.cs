using BMS.Infrastructure.Grpc.Services;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using TMS.Application.Transactions.Commands;

namespace TMS.Infrastructure.gRPC.Services;

public class RollBackService(IMediator mediator) : Rollback.RollbackBase
{
    public override async Task<RollbackResponse> RollbackTransactions(RollbackRequest rollbackRequest, ServerCallContext context)
    {
        var date = rollbackRequest.RollbackDate.ToDateTime();

        var command = new RollBackTransactionsCommand(DateOnly.FromDateTime(date));
       
        await mediator.Send(command);
        
        return new RollbackResponse();
    }
}