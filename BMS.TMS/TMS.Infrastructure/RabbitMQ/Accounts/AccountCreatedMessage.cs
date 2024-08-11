namespace TMS.Infrastructure.RabbitMQ.Accounts;
public record AccountCreatedMessage(Guid BranchId, Guid CustomerId);