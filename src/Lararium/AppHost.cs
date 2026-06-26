var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache", port: 50406);

var postgres = builder.AddPostgres("postgres")
    .WithHostPort(65123)
    .WithDataVolume()
    .AddDatabase("Lararium");

var server = builder.AddProject<Projects.Lararium_API>("server");

var migrationService = builder.AddProject<Projects.Lararium_MigrationService>("migrationservice")
    .WithReference(postgres)
    .WaitFor(postgres);

server.WithReference(postgres)
    .WaitFor(postgres)
    .WithReference(cache)
    .WaitFor(cache)
    .WithHttpHealthCheck("/health")
    .WithExternalHttpEndpoints();

var webfrontend = builder.AddViteApp("webfrontend", "../Lararium.UI.Svelte")
    .WithReference(server)
    .WaitFor(server);

server.PublishWithContainerFiles(webfrontend, "wwwroot");

builder.Build().Run();