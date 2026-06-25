using Lararium.MigrationService;
using Lararium.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = Host.CreateApplicationBuilder(args);

builder.Logging.AddConsole();
builder.AddNpgsqlDbContext<AppDbContext>("Lararium");
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();