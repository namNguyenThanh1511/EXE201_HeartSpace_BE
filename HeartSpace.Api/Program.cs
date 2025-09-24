/*
 top-level statements (câu lệnh cấp cao nhất), một tính năng của C# 6.0 trở lên. 
 Tính năng này cho phép viết code trực tiếp ở root của file mà không cần bọc trong namespace hay class Main.
 */

using HeartSpace.Api.Extensions;
using HeartSpace.Domain.Entities;
using Microsoft.AspNetCore.Identity;
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
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseExceptionHandler();

app.UseHttpsRedirection();


app.UseAuthentication();
app.UseAuthorization();
app.UseCors("AllowHeartSpace");

app.MapControllers();

app.Run();

