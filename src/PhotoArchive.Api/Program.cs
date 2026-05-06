using Microsoft.EntityFrameworkCore;
using PhotoArchive.Data;
using PhotoArchive.Core.Entities;
using PhotoArchive.Data.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PhotoDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<PhotoService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();

/// <summary>Exposes the top-level Program class so that integration tests can reference it via WebApplicationFactory.</summary>
public partial class Program { }