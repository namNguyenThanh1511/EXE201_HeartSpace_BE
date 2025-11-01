using HeartSpace.Api.Extensions;
using HeartSpace.Domain.Entities;
using HeartSpace.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();


// ✅ Log môi trường hiện tại
Console.WriteLine($"🔥 Hosting Environment: {builder.Environment.EnvironmentName}");

// ✅ Log connection string (ẩn mật khẩu để bảo mật)
var connStr = builder.Configuration.GetConnectionString("HeartSpaceDb");
if (!string.IsNullOrEmpty(connStr))
{
    try
    {
        var builderSql = new SqlConnectionStringBuilder(connStr);
        Console.WriteLine("✅ Using SQL Connection:");
        Console.WriteLine($"   Server   : {builderSql.DataSource}");
        Console.WriteLine($"   Database : {builderSql.InitialCatalog}");
        Console.WriteLine($"   User ID  : {builderSql.UserID}");
        Console.WriteLine($"   Encrypt  : {builderSql.Encrypt}");
        Console.WriteLine($"   TrustServerCertificate : {builderSql.TrustServerCertificate}");


    }
    catch (Exception ex)
    {
        Console.WriteLine($"⚠️  Invalid connection string: {ex.Message}");
        Console.WriteLine($"   Raw value: {connStr}");
    }
}
else
{
    Console.WriteLine("❌ Connection string 'HeartSpaceDb' not found in configuration.");
}
// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Global settings: use the defaults, but serialize enums as strings
        // (because it really should be the default)
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter(JsonNamingPolicy.CamelCase, false));
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwaggerForAuthentication();
builder.Services.ConfigureDatabase(builder.Configuration);
builder.Services.AddHttpContextAccessor();
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.ConfigureGlobalException();
builder.Services.ConfigureJWT(builder.Configuration);
builder.Services.ConfigureCors();


var app = builder.Build();

// Configure the HTTP request pipeline. 
if (app.Environment.IsEnvironment("LocalDocker") || app.Environment.IsEnvironment("Production"))
{
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<RepositoryContext>();

        try
        {
            if (db.Database.GetPendingMigrations().Any())
            {
                db.Database.Migrate();
            }
        }
        catch (SqlException ex) when (ex.Number == 2714 || ex.Number == 1801)  // Duplicate object/DB exists
        {
            // Log & skip (table/DB đã có)
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            logger.LogWarning("Migration skipped: Object already exists. {Error}", ex.Message);
        }
    }
}
app.Seed();

app.UseSwagger();
app.UseSwaggerUI();

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseCors("AllowHeartSpace");
app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();

