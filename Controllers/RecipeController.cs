using Microsoft.AspNetCore.Mvc;
using Cozinhe_Comigo_API.Models;
using Cozinhe_Comigo_API.Filters;
using Cozinhe_Comigo_API.DTOS;
using Cozinhe_Comigo_API.Services;

namespace Cozinhe_Comigo_API.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeController : ControllerBase {
        private readonly IRecipeService _service;

        public RecipeController(IRecipeService service) {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReadRecipeDto>>> Get([FromQuery] RecipeFilter filter) {
            // ToDo: Implementar os filtros de busca
            // var recipes = await _context.Recipes.ToListAsync();
            
            return Ok(new List<ReadRecipeDto>());
        }





        // ToDo: Implementar métodos para criar, editar e deletar receitas
    }
}