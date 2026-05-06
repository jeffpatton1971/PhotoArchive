using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using PhotoArchive.Data;
using PhotoArchive.Core.Entities;
using PhotoArchive.Data.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PhotoDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("PhotoArchiveWebDev", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "PhotoArchive API",
        Version = "v1",
        Description = "API for browsing and querying the personal photo archive. " +
                      "Supports archive navigation by year/month/day, photo collection queries, " +
                      "gallery browsing, blog post association, and the 'On This Day' feature."
    });

    // Include XML comments from the API and Core assemblies.
    var apiXml = Path.Combine(AppContext.BaseDirectory, "PhotoArchive.Api.xml");
    if (File.Exists(apiXml))
        options.IncludeXmlComments(apiXml);

    var coreXml = Path.Combine(AppContext.BaseDirectory, "PhotoArchive.Core.xml");
    if (File.Exists(coreXml))
        options.IncludeXmlComments(coreXml);
});

builder.Services.AddScoped<PhotoService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("PhotoArchiveWebDev");

app.MapControllers();

app.Run();

/// <summary>Exposes the top-level Program class so that integration tests can reference it via WebApplicationFactory.</summary>
public partial class Program { }