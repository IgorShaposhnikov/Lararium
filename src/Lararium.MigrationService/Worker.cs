using Lararium.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Lararium.MigrationService;

internal sealed class Worker(
    IServiceProvider serviceProvider,
    IHostApplicationLifetime hostApplicationLifetime,
    ILogger<Worker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Starting migration service...");

        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        try
        {
            logger.LogInformation("Applying EF Core migrations...");
            await dbContext.Database.MigrateAsync(stoppingToken);
            logger.LogInformation("Migrations applied successfully!");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while executing database migrations.");
            Environment.ExitCode = 1;
        }
        finally
        {
            hostApplicationLifetime.StopApplication();
        }
    }
}
