using HeartSpace.Api.Extensions;
using HeartSpace.Domain.Entities;
using HeartSpace.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);


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
if (app.Environment.IsEnvironment("LocalDocker"))
{
    //migrarte pending 
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<RepositoryContext>();
        //db.Database.EnsureCreated();
        if (db.Database.GetPendingMigrations().Any()) //only migrate if there are any new migrate file
        {
            db.Database.Migrate();
        }
    }
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseExceptionHandler();

app.UseHttpsRedirection();


app.UseAuthentication();
app.UseAuthorization();
app.UseCors("AllowHeartSpace");

app.MapControllers();

app.Run();

