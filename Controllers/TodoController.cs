using MeuTodo.Data;
using MeuTodo.Models;
using MeuTodo.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace MeuTodo.Controllers
{
    [ApiController]
    [Route(template: "v1")] //Versionamento de rota, esse controller é a versão 1 (v1)
    //A classe CONTROLLER vai receber a requisição, manipular ela e devolver para a tela
    public class TodoController : ControllerBase
    {
        [HttpGet]
        [Route(template: "todos")]
        //FromServices vai pegar tudo o que está dentro do Services na Startup
        public async Task<IActionResult> GetAsync(
            [FromServices] AppDbContext context)
        {
            var todos = await context
                .Todos
                .AsNoTracking()
                .ToListAsync();
            return Ok(todos);
        }

        [HttpGet]
        [Route(template: "todos/{id}")]
        public async Task<IActionResult> GetByIdAsync(
            [FromServices] AppDbContext context,
            [FromRoute] int id)
        {
            var todo = await context
                .Todos
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id); //Para selecionar um item só
            return todo == null ? NotFound() : Ok(todo); //Essa linha é um if (se for null retorna um notfound, senao retorna Ok)
        }

        //FromBody vai vir do corpo da requisição
        [HttpPost(template: "todos")]
        public async Task<IActionResult> PostAsync(
            [FromServices] AppDbContext context,
            [FromBody] CreateTodoViewModel model)
        {
            //ModelState faz a validação, se o title não estiver preenchido ele vai validar
            if (!ModelState.IsValid)
                return BadRequest();

            var todo = new Todo
            {
                Done = false,
                Title = model.Title
            };

            try
            {
                await context.Todos.AddAsync(todo);
                await context.SaveChangesAsync();
                return Created($"v1/todos/{todo.Id}", todo);
            }
            catch (System.Exception)
            {
                return BadRequest();
            }
        }


        [HttpPut(template: "todos/{id}")]
        public async Task<IActionResult> PutAsync(
        [FromServices] AppDbContext context,
        [FromBody] CreateTodoViewModel model,
        [FromRoute] int id)
        {
            //ModelState faz a validação, se o title não estiver preenchido ele vai validar
            if (!ModelState.IsValid)
                return BadRequest();

            //Abaixo tenho que recuperar do banco
            var todo = await context.Todos.FirstOrDefaultAsync(x => x.Id == id);

            if (todo == null)
                return NotFound();
            try
            {
                //Else fazer o update, abaixo:
                todo.Title = model.Title;

                context.Todos.Update(todo);
                await context.SaveChangesAsync();

                return Ok(todo);
            }
            catch (System.Exception)
            {
                return BadRequest();
            }
        }


        [HttpDelete("todos/{id}")]
        public async Task<IActionResult> DeleteAsync(
        [FromServices] AppDbContext context,
        [FromRoute] int id)
        {
            var todo = await context
                .Todos
                .FirstOrDefaultAsync(x => x.Id == id);

            try
            {
                context.Todos.Remove(todo);
                await context.SaveChangesAsync();
                return Ok();
            }
            catch (System.Exception)
            {
                return BadRequest();
            }
        }
    }
}
