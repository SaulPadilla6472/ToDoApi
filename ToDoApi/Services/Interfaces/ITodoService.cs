using TodoApi.Models; // Necesitamos TodoItem
using System.Collections.Generic;
using System.Threading.Tasks; // Necesitamos Task

namespace TodoApi.Services.Interfaces // Asegúrate que el namespace sea correcto
{
    public interface ITodoService
    {
        // Nota: Los tipos de retorno aquí pueden variar. A veces devuelven
        // directamente las entidades, otras veces tipos específicos del servicio
        // o indicadores de éxito/error. Empecemos devolviendo las entidades.

        Task<IEnumerable<TodoItem>> GetAllAsync();
        Task<TodoItem?> GetByIdAsync(long id); // Usamos '?' para indicar que puede ser null si no se encuentra
        Task<TodoItem> CreateAsync(TodoItem newTodoItem);
        Task<bool> UpdateAsync(long id, TodoItem updatedTodoItem); // Devuelve true si se actualizó, false si no se encontró
        Task<bool> DeleteAsync(long id); // Devuelve true si se eliminó, false si no se encontró
    }
}