using Microsoft.EntityFrameworkCore; // Para ToListAsync, FindAsync, SaveChangesAsync, etc.
using TodoApi.Data;       // Para TodoContext
using TodoApi.Models;     // Para TodoItem
using TodoApi.Services.Interfaces; // Para ITodoService
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq; // Para Any() en Update/Delete (opcional)

namespace TodoApi.Services // O el namespace que corresponda a tu carpeta Services
{
    public class TodoService : ITodoService // Implementa la interfaz
    {
        private readonly TodoContext _context; // Dependencia del DbContext

        // Constructor para inyectar el DbContext
        public TodoService(TodoContext context)
        {
            _context = context;
        }

        // Ahora implementaremos los métodos de la interfaz ITodoService
        // Si Visual Studio te marca error en "ITodoService", puedes usar la
        // bombilla de acciones rápidas para "Implementar interfaz" y te creará
        // los métodos vacíos. Luego los llenas como se muestra abajo.

        public async Task<IEnumerable<TodoItem>> GetAllAsync()
        {
            // Lógica movida desde el controlador
            return await _context.TodoItems.ToListAsync();
        }

        public async Task<TodoItem?> GetByIdAsync(long id)
        {
            // Lógica movida desde el controlador
            return await _context.TodoItems.FindAsync(id);
            // Devuelve null automáticamente si FindAsync no encuentra nada
        }

        public async Task<TodoItem> CreateAsync(TodoItem newTodoItem)
        {
            // Lógica movida desde el controlador
            newTodoItem.CreatedAt = DateTime.UtcNow;
            _context.TodoItems.Add(newTodoItem);
            await _context.SaveChangesAsync();
            return newTodoItem; // Devuelve el item con su nuevo Id
        }

        public async Task<bool> UpdateAsync(long id, TodoItem updatedTodoItem)
        {
            // Busca el item existente
            var existingTodo = await _context.TodoItems.FindAsync(id);

            // Si no existe, no se puede actualizar -> devuelve false
            if (existingTodo == null)
            {
                return false;
            }

            // Actualiza las propiedades
            existingTodo.Title = updatedTodoItem.Title;
            existingTodo.IsCompleted = updatedTodoItem.IsCompleted;
            existingTodo.DueDate = updatedTodoItem.DueDate;

            try
            {
                // Intenta guardar los cambios
                await _context.SaveChangesAsync();
                return true; // Éxito al guardar
            }
            catch (DbUpdateConcurrencyException)
            {
                // Manejo básico de concurrencia o simplemente indicar fallo
                // Podrías verificar aquí si aún existe con _context.TodoItems.Any(e => e.Id == id)
                return false; // Falló la actualización (podría ser concurrencia u otro error)
            }
        }

        public async Task<bool> DeleteAsync(long id)
        {
            // Busca el item
            var todoToDelete = await _context.TodoItems.FindAsync(id);

            // Si no existe, no se puede borrar -> devuelve false
            if (todoToDelete == null)
            {
                return false;
            }

            // Marca para eliminar y guarda cambios
            _context.TodoItems.Remove(todoToDelete);
            await _context.SaveChangesAsync();

            return true; // Éxito al eliminar
        }
    }
}