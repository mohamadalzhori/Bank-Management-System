namespace AMS.Application.RabbitMQ.Account;

public interface IRabbitMqProducer
{
    void PublishAccountCreated(Guid branchId, Guid cusomterId); 
}