using Microsoft.AspNetCore.Mvc;
using Receitas.Models;

namespace Receitas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeController : ControllerBase
    {
        // private readonly ReceitasContext _context;

        // public RecipeController(ReceitasContext context)
        // {
        //     _context = context;
        // }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Recipe>>> GetRecipes()
        {
            // ToDo: Implementar os filtros de busca
            // var recipes = await _context.Recipes.ToListAsync();
            var recipes = new List<Recipe>
            {
                new Recipe("Bolo de Chocolate", new List<string> { "Farinha", "Açúcar", "Cacau em pó", "Ovos", "Leite" }, new List<string> { "Misture os ingredientes secos.", "Adicione os ovos e o leite.", "Asse no forno." }),
                new Recipe("Salada de Frutas", new List<string> { "Maçã", "Banana", "Laranja", "Uva" }, new List<string> { "Corte as frutas em pedaços.", "Misture tudo em uma tigela.", "Sirva gelado." })
            };
            return recipes;
        }

        // ToDo: Implementar métodos para criar, editar e deletar receitas
    }
}