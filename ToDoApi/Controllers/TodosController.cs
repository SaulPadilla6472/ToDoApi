using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Necesario para DbUpdateConcurrencyException, etc.
using TodoApi.Data;       // Namespace de tu TodoContext
using TodoApi.Models;     // Namespace de tu TodoItem
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks; // Necesario para async/await

namespace TodoApi.Controllers
{
    [Route("api/todos")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly TodoContext _context;

        public TodoController(TodoContext context)
        {
            _context = context;
        }

        // --- GET All ---
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetAllTodos()
        {
            return Ok(await _context.TodoItems.ToListAsync());
        }

        // --- GET by ID ---
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetTodoById(long id)
        {
            var todo = await _context.TodoItems.FindAsync(id);
            if (todo == null)
            {
                return NotFound();
            }
            return Ok(todo);
        }

        // --- POST (CREAR) --- <<<<< ESTE ES EL MÉTODO QUE NECESITAS ASEGURARTE DE TENER >>>>>
        [HttpPost] // Atributo que mapea el verbo POST a este método
        public async Task<ActionResult<TodoItem>> CreateTodo(TodoItem todoItem)
        {
            // Establecemos valores generados por el servidor
            todoItem.CreatedAt = DateTime.UtcNow;

            // Añadimos la nueva entidad al contexto de EF Core
            _context.TodoItems.Add(todoItem); // Marca la entidad como 'Added'

            // Guardamos los cambios en la base de datos (ejecuta el INSERT)
            await _context.SaveChangesAsync(); // Persiste los cambios

            // Devolvemos una respuesta 201 Created.
            // Para este punto, 'todoItem.Id' ya ha sido actualizado por EF Core
            // con el valor generado por la base de datos.
            return CreatedAtAction(
                nameof(GetTodoById),       // Nombre del método GET para obtener el recurso creado
                new { id = todoItem.Id },  // Parámetros de ruta para ese método GET
                todoItem);                 // El objeto creado (con Id) en el cuerpo de la respuesta
        }

        // --- PUT (ACTUALIZAR) ---
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodo(long id, TodoItem updatedTodo)
        {
            if (id != updatedTodo.Id)
            {
                return BadRequest("El ID de la ruta no coincide con el ID del cuerpo.");
            }

            var existingTodo = await _context.TodoItems.FindAsync(id);
            if (existingTodo == null)
            {
                return NotFound();
            }

            existingTodo.Title = updatedTodo.Title;
            existingTodo.IsCompleted = updatedTodo.IsCompleted;
            existingTodo.DueDate = updatedTodo.DueDate;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Manejo básico (podría ser más robusto verificando si aún existe)
                if (!_context.TodoItems.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw; // Relanzar si no sabemos qué hacer
                }
            }

            return NoContent();
        }

        // --- DELETE ---
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodo(long id)
        {
            var todoToDelete = await _context.TodoItems.FindAsync(id);
            if (todoToDelete == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(todoToDelete); // Marca para eliminar
            await _context.SaveChangesAsync();      // Ejecuta DELETE en la BD

            return NoContent();
        }
    }
}