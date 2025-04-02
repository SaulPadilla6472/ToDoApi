using Microsoft.EntityFrameworkCore;
using TodoApi.Data; // El namespace donde está tu TodoContext
using TodoApi.Services;
using TodoApi.Services.Interfaces;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle


// VVV AÑADE ESTO VVV
// Leer la cadena de conexión desde appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Registrar TodoContext con el contenedor de Inyección de Dependencias (DI)
builder.Services.AddDbContext<TodoContext>(options =>
    options.UseNpgsql(connectionString)); // Configurar para usar Npgsql (PostgreSQL)
// Registrar nuestros servicios personalizados
builder.Services.AddScoped<ITodoService, TodoService>();
// --- Más servicios existentes como builder.Services.AddEndpointsApiExplorer(); ---

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
