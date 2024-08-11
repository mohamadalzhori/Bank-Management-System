using Hangfire;
using TMS.API.Configurations;
using TMS.API.Middlewares;
using TMS.Application;
using TMS.Application.Services;
using TMS.Infrastructure;

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

// Infrastructure Layer
builder.Services.AddInfrastructure(builder.Configuration);

// Keycloak
builder.Services.ConfigureKeycloak(builder.Configuration);

// Hangfire
builder.Services.ConfigureHangfire(builder);

// add IHttpContextAccessor service
builder.Services.AddHttpContextAccessor();

var app = builder.Build();


// Apply migrations at startup
using (var scope = app.Services.CreateScope())
{
    var migrationService = scope.ServiceProvider.GetRequiredService<MigrationService>();
    await migrationService.ApplyMigrationsAsync();
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.ConfigureSwaggerUi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// Exception Handling
app.UseExceptionHandler();

// Hangfire
app.UseHangfireDashboard("/jobs");

// Grpc
app.UseInfrastructure();

app.MapControllers();

app.Run();