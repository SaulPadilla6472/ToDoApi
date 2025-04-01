namespace TodoApi.Models // Asegúrate que el namespace coincida con tu estructura de carpetas
{
    public class TodoItem
    {
        public long Id { get; set; } // Identificador único de la tarea (usamos long por simplicidad)
        public string? Title { get; set; } // Título o descripción de la tarea. El '?' indica que puede ser null.
        public bool IsCompleted { get; set; } // Indica si la tarea está completada o no (por defecto será false)
        public DateTime CreatedAt { get; set; } // Fecha y hora de creación de la tarea
        public DateTime? DueDate { get; set; } // Fecha límite opcional para la tarea. El '?' indica que puede ser null.
    }
}
