using System.Text;
using System.Text.Json;
using AMS.Application.RabbitMQ.Account;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace AMS.Infrastructure.RabbitMQ.Accounts;

public class RabbitMqProducer(IConfiguration configuration) : IRabbitMqProducer
{
    public void PublishAccountCreated(Guid branchId, Guid customerId)
    {
        var message = new AccountCreatedMessage(branchId, customerId);
        
        var hostname = configuration["RabbitMq:Host"]; 
        var queueName = configuration["RabbitMq:AccountQueue"]; 
        
        var factory = new ConnectionFactory { HostName = hostname };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

        channel.BasicPublish(exchange: string.Empty,
            routingKey: queueName,
            basicProperties: null,
            body: body);
    } 
}