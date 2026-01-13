using Maraton_webAPI.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Controllers + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Connection string beolvasása appsettings.json-bõl
var cs = builder.Configuration.GetConnectionString("MaratonDb");

if (string.IsNullOrWhiteSpace(cs))
{
    throw new InvalidOperationException("Hiányzik a MaratonDb connection string az appsettings.json-bõl.");
}

// DbContext regisztrálása (MySql.EntityFrameworkCore provider)
builder.Services.AddDbContext<MaratonContext>(options =>
{
    options.UseMySQL(cs);
});

var app = builder.Build();

// Swagger csak fejlesztõi módban
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
