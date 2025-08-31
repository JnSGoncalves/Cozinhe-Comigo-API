using Microsoft.AspNetCore.Mvc;
using Receitas.Models;

namespace Receitas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {

        [HttpPost]
        public async Task<ActionResult<IEnumerable<Avaliation>>> GetRecipeAvaliations(int recipeId)
        {
            // var comments = await _context.Comments.Where(c => c.RecipeId == recipeId).ToListAsync();
            var avaliation = new Avaliation(recipeId, 1, "Ótima receita! Muito fácil de fazer e deliciosa.");
            return new List<Avaliation> { avaliation };
        }

        // Todo: Implementar métodos para adicionar, editar e deletar avaliações
    }
}