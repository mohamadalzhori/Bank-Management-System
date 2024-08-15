using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;

namespace BMS.Infrastructure.Grpc.Services;

public class RollBackService
{
    private readonly Rollback.RollbackClient _client;

    public RollBackService(IConfiguration configuration)
    {
        var address = configuration["GrpcServices:RollbackService"];
        // Create a gRPC channel
        var channel = GrpcChannel.ForAddress(address);

        // Create a client using the channel
        _client = new Rollback.RollbackClient(channel);
    }

    public async Task RollbackTransactions(DateOnly date)
    {
        // Convert DateOnly to DateTime in UTC
        var dateTime = date.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc); 
        
        // Convert DateTime to Google Protobuf Timestamp
        var timestamp = new Timestamp
        {
            Seconds = new DateTimeOffset(dateTime).ToUnixTimeSeconds()
        };

        // Create the request
        var request = new RollbackRequest
        {
            RollbackDate = timestamp
        };
        
        // Call the service method
        var response = await _client.RollbackTransactionsAsync(request);

        // Handle the response (no response content in this case)
        Console.WriteLine("Rollback completed successfully.");
    }
}