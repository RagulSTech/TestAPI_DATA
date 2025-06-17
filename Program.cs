using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// ✅ Try to build connection string from environment variables first
var host = Environment.GetEnvironmentVariable("DB_HOST");
var port = Environment.GetEnvironmentVariable("DB_PORT") ?? "5432";
var dbName = Environment.GetEnvironmentVariable("DB_NAME");
var user = Environment.GetEnvironmentVariable("DB_USER");
var password = Environment.GetEnvironmentVariable("DB_PASSWORD");

string? connectionString = null;

// ✅ If all required env vars are present, build from them
if (!string.IsNullOrEmpty(host) && !string.IsNullOrEmpty(dbName) && !string.IsNullOrEmpty(user) && !string.IsNullOrEmpty(password))
{
    connectionString = $"Host={host};Port={port};Database={dbName};Username={user};Password={password}";
}
else
{
    // ✅ Fallback to appsettings.json
    connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new Exception("Connection string not found in appsettings.json or environment variables.");
}

Console.WriteLine($"✅ DB connection: {connectionString}");

// ✅ Register configuration to access connection string in controllers
builder.Services.AddSingleton(connectionString); // Add as a singleton string
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ✅ Configure Kestrel to listen on the correct port for Render
var portEnv = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(int.Parse(portEnv));
});

var app = builder.Build();

// ✅ Swagger only in development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHttpsRedirection();
}

app.UseAuthorization();
app.MapGet("/", () => "🚀 API is running");
app.MapControllers();
app.Run();
