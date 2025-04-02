using Microsoft.EntityFrameworkCore; // Necesitamos EF Core
using TodoApi.Data;       // Para TodoContext
using TodoApi.Models;     // Para TodoItem
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoApi.Repositories // O el namespace que corresponda
{
    public class TodoRepository : ITodoRepository // Implementa la interfaz
    {
        private readonly TodoContext _context; // Dependencia del DbContext

        // Inyectamos DbContext
        public TodoRepository(TodoContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TodoItem>> GetAllAsync()
        {
            // Usa el DbContext para obtener todos los items
            return await _context.TodoItems.ToListAsync();
        }

        public async Task<TodoItem?> GetByIdAsync(long id)
        {
            // Usa el DbContext para buscar por ID
            return await _context.TodoItems.FindAsync(id);
        }

        public async Task<TodoItem> AddAsync(TodoItem todoItem)
        {
            // Añade al DbContext y guarda cambios
            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();
            return todoItem; // Devuelve el item (ahora con Id si era nuevo)
        }

        public async Task<bool> UpdateAsync(TodoItem todoItem)
        {
            // Marca la entidad como modificada
            // Nota: FindAsync no es ideal aquí si ya tenemos la entidad.
            // Asumimos que la entidad 'todoItem' ya fue obtenida y modificada
            // por la capa de servicio ANTES de llamar a este UpdateAsync.
            // Si la entidad viene directamente del request, primero hay que buscarla.
            // --> Vamos a ajustar esto para que sea más robusto <--

            // Buscamos primero si existe
            var existingTodo = await _context.TodoItems.FindAsync(todoItem.Id);
            if (existingTodo == null)
            {
                return false; // No encontrado, no se puede actualizar
            }

            // Actualizamos las propiedades del encontrado con los valores del que llegó
            // (Esto evita problemas si 'todoItem' no era una entidad rastreada)
            _context.Entry(existingTodo).CurrentValues.SetValues(todoItem);
            _context.Entry(existingTodo).State = EntityState.Modified; // Asegurar el estado

            try
            {
                await _context.SaveChangesAsync();
                return true; // Actualización exitosa
            }
            catch (DbUpdateConcurrencyException)
            {
                // Podría ser que alguien más lo borró o modificó
                // Volver a verificar si existe podría ser una opción
                if (!await _context.TodoItems.AnyAsync(e => e.Id == todoItem.Id))
                {
                    return false; // Ya no existe
                }
                else
                {
                    throw; // Otro tipo de error de concurrencia
                }
            }
        }

        public async Task<bool> DeleteAsync(long id)
        {
            // Busca el item a eliminar
            var todoToDelete = await _context.TodoItems.FindAsync(id);

            // Si no existe, devuelve false
            if (todoToDelete == null)
            {
                return false;
            }

            // Marca para eliminar y guarda cambios
            _context.TodoItems.Remove(todoToDelete);
            await _context.SaveChangesAsync();

            return true; // Eliminación exitosa
        }
    }
}