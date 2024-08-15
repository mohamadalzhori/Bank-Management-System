using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace BMS.Infrastructure.RabbitMQ;

public class RabbitMqProducer(IConfiguration configuration) : IRabbitMqProducer
{
    public void PublishBranchCreated(BranchCreatedMessage message)
    {
        var hostname = configuration["RabbitMq:Host"]; 
        var exchangeName = configuration["RabbitMq:Exchange"]; 
        
        var factory = new ConnectionFactory { HostName = hostname };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        // Declare a fanout exchange
        channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Fanout, durable: true, autoDelete: false, arguments: null);
        
        // Serialize the message
        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
       
        // Publish to the exchange
        channel.BasicPublish(exchange: exchangeName, routingKey: "", basicProperties: null, body: body);
        
        Console.WriteLine("Message sent to exchange."); 
    } 
}