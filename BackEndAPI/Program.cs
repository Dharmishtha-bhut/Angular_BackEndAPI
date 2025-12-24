using Backend.Core.Interfaces;
using Backend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<StoreContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IProductRepository, ProductRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.MapControllers();


// Apply migrations and seed the database.
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<StoreContext>();
    try
    {
        await context.Database.MigrateAsync();
        await StoreContextSeed.SeedAsync(context);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred during migration or seeding: {ex.Message}");
    }
}

app.Run();
