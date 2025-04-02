using TodoApi.Models; // Necesitamos TodoItem
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TodoApi.Repositories // O el namespace que corresponda
{
    public interface ITodoRepository
    {
        // Métodos asíncronos para operaciones de datos
        Task<IEnumerable<TodoItem>> GetAllAsync();
        Task<TodoItem?> GetByIdAsync(long id);
        Task<TodoItem> AddAsync(TodoItem todoItem); // Devuelve el item añadido (con Id)
        Task<bool> UpdateAsync(TodoItem todoItem); // Devuelve true si se actualizó, false si no
        Task<bool> DeleteAsync(long id);        // Devuelve true si se eliminó, false si no
    }
}