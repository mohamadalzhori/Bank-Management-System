using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using TMS.Persistence.Dynamic;
using TMS.Persistence.Shared;

namespace TMS.Application.Services
{
    public class MigrationService(SharedDbContext _sharedDbContext, EncryptionHelper _encryptionHelper)
    {
        public async Task ApplyMigrationsAsync()
        {
            var tenantConnectionStrings = await _sharedDbContext.Branches
                .Select(x => x.ConnectionString)
                .ToListAsync();

            foreach (var encryptedConnectionString in tenantConnectionStrings)
            {
                var connectionString = _encryptionHelper.Decrypt(encryptedConnectionString);

                // Extract the schema from the connection string
                var schema = ExtractSchemaFromConnectionString(connectionString);

                // Create the schema if it doesn't exist
                var createSchemaSql = $"CREATE SCHEMA IF NOT EXISTS {schema};";
                await _sharedDbContext.Database.ExecuteSqlRawAsync(createSchemaSql);

                // Build the options with the correct schema for the migration history table
                var optionsBuilder = new DbContextOptionsBuilder<DynamicDbContext>();
                optionsBuilder.UseNpgsql(connectionString, npgsqlOptions =>
                {
                    npgsqlOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName, schema);
                });
                
                using (var dbContext = new DynamicDbContext(optionsBuilder.Options))
                {
                    await dbContext.Database.MigrateAsync();
                }
            }
        }

        private string ExtractSchemaFromConnectionString(string connectionString)
        {
            var searchPathKey = "Search Path=";
            var startIndex = connectionString.IndexOf(searchPathKey, StringComparison.OrdinalIgnoreCase) +
                             searchPathKey.Length;
            var endIndex = connectionString.IndexOf(';', startIndex);
            return endIndex == -1
                ? connectionString.Substring(startIndex)
                : connectionString.Substring(startIndex, endIndex - startIndex);
        }
    }
}