namespace TMS.Infrastructure.RabbitMQ.Branches;
public record BranchCreatedMessage(Guid BranchId, string BranchName);