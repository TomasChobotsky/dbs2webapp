using Api.Extensions;
using dbs2webapp.Api.Extensions;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

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