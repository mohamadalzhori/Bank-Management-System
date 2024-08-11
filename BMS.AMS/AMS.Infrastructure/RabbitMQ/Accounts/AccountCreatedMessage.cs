namespace AMS.Infrastructure.RabbitMQ.Accounts;
public record AccountCreatedMessage(Guid BranchId, Guid CustomerId);