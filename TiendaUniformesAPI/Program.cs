using Microsoft.EntityFrameworkCore;
using TiendaUniformesAPI.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string conn = builder.Configuration.GetConnectionString("TiendaUniformesContext")!;

builder.Services.AddControllers();
builder.Services.AddDbContext<TiendaUniformesContext>(db => db.UseSqlServer(conn));

// Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Configurar el uso de CORS
app.UseCors("AllowSpecificOrigin");

app.UseAuthorization();

app.MapControllers();

app.Run();
