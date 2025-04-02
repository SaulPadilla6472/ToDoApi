using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Necesario para DbUpdateConcurrencyException, etc.
using TodoApi.Models;     // Namespace de tu TodoItem
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks; // Necesario para async/await
using TodoApi.Services.Interfaces;

namespace TodoApi.Controllers
{
    [Route("api/todos")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly ITodoService _todoService;
        // MODIFICAR ESTE CONSTRUCTOR:
        public TodoController(ITodoService todoService) // Inyecta la interfaz del servicio
        {
            _todoService = todoService; // Asigna al campo del servicio
        }

        // --- GET All ---
        // GET: api/todos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetAllTodos()
        {
            // Delega la obtención de datos al servicio
            var todos = await _todoService.GetAllAsync();
            // Devuelve el resultado obtenido del servicio
            return Ok(todos);
        }

        // --- GET by ID ---
        // GET: api/todos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetTodoById(long id)
        {
            // Pide el item al servicio
            var todo = await _todoService.GetByIdAsync(id);

            // Si el servicio devuelve null (no encontrado)...
            if (todo == null)
            {
                return NotFound(); // Devuelve 404 Not Found
            }

            // Si el servicio devuelve el item...
            return Ok(todo); // Devuelve 200 OK con el item
        }

        // --- POST (CREAR) --- <<<<< ESTE ES EL MÉTODO QUE NECESITAS ASEGURARTE DE TENER >>>>>
        // POST: api/todos
        [HttpPost]
        public async Task<ActionResult<TodoItem>> CreateTodo(TodoItem todoItem)
        {
            // Validación básica de entrada (podría ser más robusta)
            if (todoItem == null)
            {
                return BadRequest("El objeto TodoItem no puede ser nulo.");
            }

            // Llama al servicio para crear el item
            // El servicio maneja la lógica de negocio (ej. poner CreatedAt) y guardar en BD
            var createdTodo = await _todoService.CreateAsync(todoItem);

            // Devuelve la respuesta estándar 201 Created
            return CreatedAtAction(
                nameof(GetTodoById),
                new { id = createdTodo.Id },
                createdTodo);
        }

        // --- PUT (ACTUALIZAR) ---
        // PUT: api/todos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodo(long id, TodoItem updatedTodo)
        {
            // Validación básica
            if (updatedTodo == null || id != updatedTodo.Id)
            {
                return BadRequest("Datos inválidos para la actualización.");
            }

            // Llama al servicio para intentar actualizar
            var success = await _todoService.UpdateAsync(id, updatedTodo);

            // Si el servicio devuelve false (no encontrado o fallo)...
            if (!success)
            {
                // Asumimos que si falla es porque no se encontró (simplificación)
                return NotFound();
            }

            // Si el servicio devuelve true (actualizado con éxito)...
            return NoContent(); // Devuelve 204 No Content
        }

        // --- DELETE ---
        // DELETE: api/todos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodo(long id)
        {
            // Llama al servicio para intentar eliminar
            var success = await _todoService.DeleteAsync(id);

            // Si el servicio devuelve false (no encontrado)...
            if (!success)
            {
                return NotFound();
            }

            // Si el servicio devuelve true (eliminado con éxito)...
            return NoContent(); // Devuelve 204 No Content
        }
    }
}