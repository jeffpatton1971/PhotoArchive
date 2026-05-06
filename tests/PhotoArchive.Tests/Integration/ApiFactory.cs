using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PhotoArchive.Data;

namespace PhotoArchive.Tests.Integration;

/// <summary>
/// A <see cref="WebApplicationFactory{TEntryPoint}"/> that replaces the PostgreSQL
/// database with an in-memory EF Core database seeded with deterministic test data.
/// Each factory instance creates a uniquely named in-memory database so that test
/// classes do not share state.
/// </summary>
public sealed class ApiFactory : WebApplicationFactory<Program>
{
    private readonly string _databaseName = $"PhotoArchiveTests_{Guid.NewGuid()}";

    /// <inheritdoc/>
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove the existing DbContextOptions<PhotoDbContext> that was registered with Npgsql.
            var optionsDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<PhotoDbContext>));
            if (optionsDescriptor is not null)
                services.Remove(optionsDescriptor);

            // Remove the PhotoDbContext registration so we can re-register it.
            var ctxDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(PhotoDbContext));
            if (ctxDescriptor is not null)
                services.Remove(ctxDescriptor);

            // Build InMemory options directly (without AddDbContext) to avoid registering the
            // InMemory IDatabaseProvider alongside Npgsql's IDatabaseProvider in the service
            // collection, which would cause a "multiple database providers" conflict.
            var inMemoryOptions = new DbContextOptionsBuilder<PhotoDbContext>()
                .UseInMemoryDatabase(_databaseName)
                .Options;

            services.AddSingleton(inMemoryOptions);
            services.AddScoped<PhotoDbContext>();
        });
    }

    /// <inheritdoc/>
    protected override IHost CreateHost(IHostBuilder builder)
    {
        var host = base.CreateHost(builder);

        // Seed the in-memory database after the host is fully built.
        using var scope = host.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<PhotoDbContext>();
        db.Database.EnsureCreated();
        SeedData.Seed(db);

        return host;
    }
}
