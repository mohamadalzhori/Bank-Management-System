namespace BMS.Infrastructure.RabbitMQ;

public interface IRabbitMqProducer
{
    void PublishBranchCreated(BranchCreatedMessage message); 
}