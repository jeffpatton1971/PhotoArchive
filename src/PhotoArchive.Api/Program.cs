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

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PhotoDbContext>();

    if (!db.Photos.Any())
    {
        db.Photos.Add(new Photo
        {
            Id = Guid.NewGuid(),
            Slug = "test-photo",
            Title = "My First Photo",
            TakenAt = DateTimeOffset.UtcNow,
            Year = DateTime.UtcNow.Year,
            Month = DateTime.UtcNow.Month,
            Day = DateTime.UtcNow.Day,
            Source = "manual",
            OriginalUrl = "https://example.com/photo.jpg",
            ThumbUrl = "https://example.com/thumb.jpg"
        });

        db.SaveChanges();
    }
}

app.Run();