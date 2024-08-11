using BMS.API.Configurations;
using BMS.API.Middlewares;
using BMS.Application;
using BMS.Application.Services;
using BMS.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Serilog
builder.ConfigureSerilog();

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Global Exception Handler
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Swagger
builder.Services.ConfigureSwagger();

// Application Layer
builder.Services.AddApplication(builder.Configuration);

// Keycloak
builder.Services.ConfigureKeycloak(builder.Configuration);

var app = builder.Build();

// Apply migrations at startup
using (var scope = app.Services.CreateScope())
{
    var migrationService = scope.ServiceProvider.GetRequiredService<MigrationService>();
    await migrationService.ApplyMigrationsAsync();
}

// Generating Encryption Key
// byte[] key = new byte[16];
// using(RandomNumberGenerator rng = RandomNumberGenerator.Create()) {
//     rng.GetBytes(key);
// }
// Print the key in Base64 format
// string base64Key = Convert.ToBase64String(key);
// Console.WriteLine($"Base64 Key: {base64Key}");


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.ConfigureSwaggerUi();
}

app.UseHttpsRedirection();
app.UseAuthorization();

// Exception Handling
app.UseExceptionHandler();

app.MapControllers();

app.Run();