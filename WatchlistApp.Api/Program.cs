using Microsoft.EntityFrameworkCore;
using WatchlistApp.Api.Controllers;
using WatchlistApp.Api.Data;
using WatchlistApp.Api.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<MovieDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost",
        policy =>
        {
            policy.WithOrigins("https://localhost:7151") // .Web csproj
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

SeedDatabase(app);

app.UseStaticFiles();

app.UseCors("AllowLocalhost");

app.Run();
void SeedDatabase(IHost app)
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<MovieDbContext>();

        if (!context.Movies.Any())
        {
            context.Movies.AddRange(new List<Movie>
            {
                new Movie { Title = "The Lion King", Watched = false, Genre = Genre.Horror.ToString() },
                new Movie { Title = "Godzilla", Watched = true, Genre = Genre.Action.ToString() },
                new Movie { Title = "The Hangover", Watched = false, Genre = Genre.Comedy.ToString() }
            });

            context.SaveChanges();
        }
    }
}