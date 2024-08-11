using BMS.Application.Services;
using BMS.Common.Constants;
using BMS.Domain.Branches;
using BMS.Infrastructure.RabbitMQ;
using BMS.Persistence.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BMS.Application.Branches.Commands;

public record CreateBranchCommand(string BranchName) : IRequest<Guid>;

public class CreateBranchCommandHandler(
    SharedDbContext sharedDbContext,
    IConfiguration configuration,
    EncryptionHelper encryptionHelper,
    MigrationService migrationService,
    IRabbitMqProducer rabbitMqProducer) : IRequestHandler<CreateBranchCommand, Guid>
{
    public async Task<Guid> Handle(CreateBranchCommand request, CancellationToken cancellationToken)
    {
        var duplicateBranch = await sharedDbContext.Branches.AnyAsync(x => x.Name == request.BranchName,
            cancellationToken: cancellationToken);

        if (duplicateBranch)
        {
            throw new DuplicateBranchNameException(request.BranchName);
        }

        var publicString = configuration.GetConnectionString(Schemas.Shared);

        var branchConnectionString = publicString!.Replace(Schemas.Shared, request.BranchName);

        var newBranch = new Branch
        {
            Name = request.BranchName,
            ConnectionString = encryptionHelper.Encrypt(branchConnectionString)
        };

        sharedDbContext.Branches.Add(newBranch);

        await sharedDbContext.SaveChangesAsync(cancellationToken);

        // Apply Migrations to the new branch
        await migrationService.ApplyMigrationsAsync();

        // Produce an event to notify the other services that a new branch has been created
        var message = new BranchCreatedMessage(newBranch.Id, newBranch.Name);
        rabbitMqProducer.PublishBranchCreated(message);

        return newBranch.Id;
    }
}