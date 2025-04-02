using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Models; // ¡Asegúrate de incluir tu namespace de Modelos!
using System.Collections.Generic;
using System.Linq;
using System;


namespace TodoApi.Controllers
{
    [Route("api/todos")] // Cambiado a plural
    [ApiController] // Aplica convenciones y comportamientos estándar de API
    public class TodoController : ControllerBase // Hereda de ControllerBase para funcionalidades de API
    {
        // Almacenamiento temporal en memoria (simula una base de datos)
        private static List<TodoItem> _todos = new List<TodoItem>
        {
            new TodoItem { Id = 1, Title = "Aprender ASP.NET Core", IsCompleted = false, CreatedAt = DateTime.UtcNow },
            new TodoItem { Id = 2, Title = "Crear API To-Do List", IsCompleted = false, CreatedAt = DateTime.UtcNow },
            new TodoItem { Id = 3, Title = "Probar con Postman", IsCompleted = true, CreatedAt = DateTime.UtcNow }
        };

        // Aquí añadiremos nuestras acciones (métodos)
        [HttpGet] // Este atributo mapea peticiones HTTP GET a este método
        public ActionResult<IEnumerable<TodoItem>> GetAllTodos()
        {
            // Devuelve la lista completa de tareas
            // Ok() genera una respuesta HTTP 200 OK con la lista en el cuerpo
            return Ok(_todos);
        }

        [HttpPost] // Este atributo mapea peticiones HTTP POST a este método
        public ActionResult<TodoItem> CreateTodo(TodoItem todoItem) // Recibe el TodoItem del cuerpo de la petición
        {
            // Asignar un nuevo ID (simple para el ejemplo en memoria)
            // Usamos Max() + 1 para evitar colisiones simples si se borran items.
            // Si la lista está vacía, empezamos en 1.
            long nextId = _todos.Any() ? _todos.Max(t => t.Id) + 1 : 1;
            todoItem.Id = nextId;

            // Establecer la fecha de creación
            todoItem.CreatedAt = DateTime.UtcNow;

            // Añadir a la lista en memoria
            _todos.Add(todoItem);

            // Devolver una respuesta HTTP 201 Created
            // CreatedAtAction genera:
            // - Código 201 Created
            // - Cabecera 'Location' apuntando a la URL del nuevo recurso (necesita un GET por ID)
            // - El objeto recién creado en el cuerpo de la respuesta
            // ¡Necesitaremos crear el método GetTodoById para que esto funcione 100%!
            return CreatedAtAction(nameof(GetTodoById), new { id = todoItem.Id }, todoItem);
        }

        // --- Necesitaremos añadir GetTodoById(long id) aquí más tarde ---
        // Placeholder temporal para que compile CreatedAtAction:
        [HttpGet("{id}")] // Ruta será /api/todo/{id}
        public ActionResult<TodoItem> GetTodoById(long id)
        {
            // Lógica para buscar por ID irá aquí más tarde
            var todo = _todos.FirstOrDefault(t => t.Id == id);
            if (todo == null)
            {
                return NotFound(); // HTTP 404
            }
            return Ok(todo); // HTTP 200
        }
        // PUT: api/todos/5
        [HttpPut("{id}")] // Especifica que este método maneja PUT con un parámetro 'id'
        public IActionResult UpdateTodo(long id, TodoItem updatedTodo) // Recibe el id de la ruta y el objeto actualizado del cuerpo
        {
            // Validación 1: Asegurar que el ID en la ruta coincida con el ID en el cuerpo (si existe)
            if (id != updatedTodo.Id)
            {
                // Si no coinciden, es una mala petición
                return BadRequest("El ID de la ruta no coincide con el ID del cuerpo.");
            }

            // Buscar la tarea existente en la lista
            var existingTodo = _todos.FirstOrDefault(t => t.Id == id);

            // Validación 2: Si no se encuentra la tarea...
            if (existingTodo == null)
            {
                return NotFound(); // Devuelve 404 Not Found
            }

            // Actualizar las propiedades del objeto existente con los valores del objeto recibido
            // ¡No actualizamos el Id ni CreatedAt!
            existingTodo.Title = updatedTodo.Title;
            existingTodo.IsCompleted = updatedTodo.IsCompleted;
            existingTodo.DueDate = updatedTodo.DueDate;

            // Devolver una respuesta HTTP 204 No Content, indicando éxito sin devolver contenido.
            // Es la respuesta estándar para un PUT exitoso.
            return NoContent();
        }
        // DELETE: api/todos/5
        [HttpDelete("{id}")] // Especifica que este método maneja DELETE con un parámetro 'id'
        public IActionResult DeleteTodo(long id)
        {
            // Buscar la tarea a eliminar
            var todoToDelete = _todos.FirstOrDefault(t => t.Id == id);

            // Si no se encuentra la tarea...
            if (todoToDelete == null)
            {
                return NotFound(); // Devuelve 404 Not Found
            }

            // Eliminar la tarea de la lista
            _todos.Remove(todoToDelete);

            // Devolver una respuesta HTTP 204 No Content, indicando éxito.
            // Es la respuesta estándar para un DELETE exitoso.
            return NoContent();
        }












    }
}