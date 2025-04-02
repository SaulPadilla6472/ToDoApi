using Microsoft.EntityFrameworkCore; // Necesario para DbContext y DbSet
using TodoApi.Models; // Necesario para poder usar TodoItem

namespace TodoApi.Data // Asegúrate que el namespace sea correcto
{
    public class TodoContext : DbContext // Hereda de DbContext
    {
        // Constructor necesario para la configuración y la Inyección de Dependencias
        public TodoContext(DbContextOptions<TodoContext> options)
            : base(options)
        {
        }

        // DbSet representa la colección de entidades (la tabla en la BD)
        // El nombre de la propiedad ("TodoItems") será el nombre de la tabla por defecto
        public DbSet<TodoItem> TodoItems { get; set; }
    }
}