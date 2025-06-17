using Microsoft.Extensions.DependencyInjection;
using MyApi.Data;

var builder = WebApplication.CreateBuilder(args);

// Build connection string from environment variables with null checks
var host = Environment.GetEnvironmentVariable("DB_HOST") ?? throw new Exception("DB_HOST not set");
var port = Environment.GetEnvironmentVariable("DB_PORT") ?? throw new Exception("DB_PORT not set");
var dbName = Environment.GetEnvironmentVariable("DB_NAME") ?? throw new Exception("DB_NAME not set");
var user = Environment.GetEnvironmentVariable("DB_USER") ?? throw new Exception("DB_USER not set");
var pass = Environment.GetEnvironmentVariable("DB_PASS") ?? throw new Exception("DB_PASS not set");

var connectionString = $"Host={host};Port={port};Database={dbName};Username={user};Password={pass};SSL Mode=Require;Trust Server Certificate=true";

// Register UserRepository with connection string injected
builder.Services.AddSingleton<UserRepository>(sp => new UserRepository(connectionString));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// THIS IS THE IMPORTANT PART:
var portEnv = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(int.Parse(portEnv));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapGet("/", () => "🚀 API is running");

app.MapControllers();

app.Run();
