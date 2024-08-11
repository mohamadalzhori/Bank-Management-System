namespace AMS.Infrastructure.RabbitMQ.Branchs;
public record BranchCreatedMessage(Guid BranchId, string BranchName);