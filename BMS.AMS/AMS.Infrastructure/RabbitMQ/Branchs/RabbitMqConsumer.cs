using System.Text;
using System.Text.Json;
using AMS.Application.Branches.Commands;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace AMS.Infrastructure.RabbitMQ.Branchs;

public class RabbitMqConsumer : BackgroundService
{
    private IConnection _connection;
    private IModel _channel;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IConfiguration _configuration;
    private string _queueName;
    
    public RabbitMqConsumer(IServiceScopeFactory serviceScopeFactory, IConfiguration configuration)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _configuration = configuration;
        InitializeRabbitMq();
    }

    private void InitializeRabbitMq()
    {
        var hostName = _configuration["RabbitMq:Host"]; 
        _queueName = _configuration["RabbitMq:BranchQueue"]!;
        var exchangeName = _configuration["RabbitMq:Exchange"]!; 
        
        var factory = new ConnectionFactory { HostName = hostName };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        // Declare a queue
        _channel.QueueDeclare(queue: _queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

        // Bind the queue to the exchange
        _channel.QueueBind(queue: _queueName, exchange: exchangeName, routingKey: "");
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += async (model, ea) =>
        {
            Console.WriteLine($"Message received from the queue: {_queueName}");
            
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var branchCreatedMessage = JsonSerializer.Deserialize<BranchCreatedMessage>(message);

            if (branchCreatedMessage != null)
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                    var command = new CreateBranchCommand(branchCreatedMessage.BranchId, branchCreatedMessage.BranchName);
                    
                   await mediator.Send(command);
                }

            }
        };

        _channel.BasicConsume(queue: _queueName,
            autoAck: true,
            consumer: consumer);

        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        _channel?.Close();
        _connection?.Close();
        base.Dispose();
    }
}