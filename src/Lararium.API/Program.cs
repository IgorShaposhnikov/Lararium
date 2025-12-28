using Lararium.API.DataProviders.Authorization;
using Lararium.API.Extensions;
using Lararium.Authorization.Jwt;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
// Add services to the container.
builder.Services.AddControllers().AddApplicationPart(typeof(ModuleInitializer).Assembly);
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Register all app options.
builder.Services.RegisterOptions(builder.Configuration);
// Add Garnet/Redis
builder.Services.AddStackExchangeRedisCache(options => 
{
    var redisConfig = builder.Configuration.GetSection("Redis");

    options.Configuration = redisConfig["Configuration"];
    options.InstanceName = redisConfig["InstanceName"];
});

// Api versioning
builder.Services.EnableApiVersioning();

// Jwt Authorization
builder.Services.AddJwtAuthorization(builder.Configuration);
// Add EFCore DbContext
builder.Services.AddDbContext(builder.Configuration.GetConnectionString("DefaultConnection")!);

builder.Services.AddScoped<IJwtAuthorizationProvider, JwtAuthorizationProvider>();
// Add JwtAuthorization with endpoint and another logic
builder.Services.AddJwtAuthModule(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        foreach (var description in app.DescribeApiVersions())
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName);
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
