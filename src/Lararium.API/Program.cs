using Lararium.API.Extensions;
using Microsoft.AspNetCore.HttpOverrides;
using Lararium.Core.Extensions;
using Lararium.Core.AspNetCore.Extensions;

var builder = WebApplication.CreateBuilder(args);

// --- 1. .NET Aspire Service Defaults & Infrastructure ---

builder.AddServiceDefaults();

builder.AddRedisClientBuilder("cache")
    .WithOutputCache()
    .WithDistributedCache();

builder.AddDbContext("Lararium");
builder.Services.RegisterOptions(builder.Configuration);

// --- 2. Register base services ---

builder.Services.AddProblemDetails();

builder.Services.AddControllers(options =>
{
    options.AddModuleFilters();
})
.AddModuleControllers();

builder.Services.AddOpenApi();
builder.Services.EnableApiVersioning();
builder.Services.AddSwaggerJwtAuthorization();

// --- 3. Modular System ---

builder.Services.AddModuleServices(builder.Configuration);

var app = builder.Build();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        foreach (var description in app.DescribeApiVersions())
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName);
    });

    app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
}

// HTTPS redirection is handled by the reverse proxy (nginx), not the app.
// The API runs behind nginx which terminates TLS/HTTP.
// app.UseHttpsRedirection();
app.UseRouting();

app.UseOutputCache();

app.UseAuthentication();
app.UseAuthorization();

app.MapDefaultEndpoints();

app.UseModuleMiddlewares();
app.UseModuleLifecycle();

app.MapControllers();
app.MapModuleEndpoints();

await app.SeedModuleDataAsync();

app.Run();