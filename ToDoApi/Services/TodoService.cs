using TodoApi.Models;     // Para TodoItem
using TodoApi.Services.Interfaces; // Para ITodoService
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using TodoApi.Repositories;
using System.Linq; // Para Any() en Update/Delete (opcional)

namespace TodoApi.Services // O el namespace que corresponda a tu carpeta Services
{
    public class TodoService : ITodoService // Implementa la interfaz
    {
        private readonly ITodoRepository _todoRepository; // Dependencia del DbContext

        // MODIFICAR ESTE CONSTRUCTOR:
        public TodoService(ITodoRepository todoRepository) // Inyecta la interfaz del repositorio
        {
            _todoRepository = todoRepository; // Asigna al campo del repositorio
        }

        // Ahora implementaremos los métodos de la interfaz ITodoService
        // Si Visual Studio te marca error en "ITodoService", puedes usar la
        // bombilla de acciones rápidas para "Implementar interfaz" y te creará
        // los métodos vacíos. Luego los llenas como se muestra abajo.

        public async Task<IEnumerable<TodoItem>> GetAllAsync()
        {
            // Delega totalmente al repositorio
            return await _todoRepository.GetAllAsync();
        }

        public async Task<TodoItem?> GetByIdAsync(long id)
        {
            // Delega totalmente al repositorio
            return await _todoRepository.GetByIdAsync(id);
        }

        public async Task<TodoItem> CreateAsync(TodoItem newTodoItem)
        {
            // Lógica de negocio/aplicación (ej: establecer valores por defecto)
            newTodoItem.CreatedAt = DateTime.UtcNow;

            // Delega la adición y el guardado al repositorio
            // Asumimos que AddAsync del repo devuelve el item con el Id poblado
            return await _todoRepository.AddAsync(newTodoItem);
        }

        public async Task<bool> UpdateAsync(long id, TodoItem updatedTodoItem)
        {
            // Validación o lógica de negocio ANTES de llamar al repo (si la hubiera)
            // Por ejemplo, asegurar consistencia de ID si el modelo lo trae
            if (updatedTodoItem == null || id != updatedTodoItem.Id)
            {
                // O manejar esto de otra forma, quizás lanzar excepción
                // Depende de las reglas de negocio. Por ahora, devolvemos false.
                return false;
            }

            // Delega la actualización (que incluye buscar y guardar) al repositorio
            return await _todoRepository.UpdateAsync(updatedTodoItem);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            // Delega la eliminación (que incluye buscar y guardar) al repositorio
            return await _todoRepository.DeleteAsync(id);
        }
    }
}