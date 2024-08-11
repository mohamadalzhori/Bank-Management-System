namespace BMS.Infrastructure.RabbitMQ;
public record BranchCreatedMessage(Guid BranchId, string BranchName);