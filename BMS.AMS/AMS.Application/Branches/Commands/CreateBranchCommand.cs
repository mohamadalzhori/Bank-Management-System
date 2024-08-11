using AMS.Application.Services;
using AMS.Common.Constants;
using AMS.Domain.Branches;
using AMS.Persistence.Shared;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace AMS.Application.Branches.Commands;

public record CreateBranchCommand(Guid BranchId, string BranchName) : IRequest;

public class CreateBranchCommandHandler(IConfiguration configuration, EncryptionHelper encryptionHelper, SharedDbContext sharedDbContext, MigrationService migrationService) : IRequestHandler<CreateBranchCommand>
{
    public async Task Handle(CreateBranchCommand request, CancellationToken cancellationToken)
    {

        var publicString = configuration.GetConnectionString(Schemas.Shared);

        var branchConnectionString = publicString!.Replace(Schemas.Shared, request.BranchName);

        var newBranch = new Branch
        {
            Id = request.BranchId,
            Name = request.BranchName,
            ConnectionString = encryptionHelper.Encrypt(branchConnectionString)
        };

        sharedDbContext.Branches.Add(newBranch);

        await sharedDbContext.SaveChangesAsync(cancellationToken);

        // Apply Migrations to the new branch
        await migrationService.ApplyMigrationsAsync();

    }
}