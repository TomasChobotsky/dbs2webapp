using Api.Extensions;
using dbs2webapp.Api.Extensions;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Text.RegularExpressions;

var builder = WebApplication.CreateBuilder(args);
// Services
builder.Services.AddControllers();
builder.Services.AddAppCors(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAppServices(builder.Configuration);
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddSwaggerDocs();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();

    // Read the SQL script
    var sqlPath = Path.Combine(AppContext.BaseDirectory, "dbs2webapp.Infrastructure", "Data", "Seed", "CustomDbObjects.sql");
    if (!File.Exists(sqlPath))
    {
        Console.WriteLine($"⚠️ Seed file not found at: {sqlPath}");
    }
    else
    {
        var sqlContent = File.ReadAllText(sqlPath);

        // 👇 Split by "GO" (batch delimiter in SQL Server)
        var statements = Regex.Split(sqlContent, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase);

        foreach (var stmt in statements)
        {
            var trimmed = stmt.Trim();
            if (string.IsNullOrWhiteSpace(trimmed)) continue;

            try
            {
                db.Database.ExecuteSqlRaw(trimmed);
                Console.WriteLine($"✅ Executed SQL block:\n{trimmed[..Math.Min(trimmed.Length, 100)]}...");
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Error executing SQL:\n" + trimmed[..Math.Min(trimmed.Length, 200)]);
                Console.WriteLine(ex.Message);
            }
        }
    }
}

// Middleware
if (app.Environment.IsDevelopment())
    app.UseAppSwagger();

app.UseHttpsRedirection();
app.UseAppCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Seed Identity
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await IdentitySeeder.SeedAsync(services);
}

app.Run();