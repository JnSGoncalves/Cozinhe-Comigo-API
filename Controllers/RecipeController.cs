using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cozinhe_Comigo_API.Models;
using Cozinhe_Comigo_API.Filters;
using Cozinhe_Comigo_API.DTOS;
using Cozinhe_Comigo_API.Data;


    //TODO: anexar as receitas as suas determinadas categorias.
    //TODO: Endpoint para filto por categoria.
    //TODO: Endpoint(get) para buscar receitas por id.
    //TODO: Endpoint(get) para bucas n receitas.
namespace Cozinhe_Comigo_API.Controllers
{
    [Route("CozinheComigoAPI/[controller]")]
    [ApiController]
    public class RecipeController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RecipeController(AppDbContext context)
        {
            _context = context;
        }

        // POST: api/recipe
        [HttpPost]
        public async Task<ActionResult<ReadRecipeDto>> InsertRecipe([FromBody] CreateRecipeDto recipeDto)
        {
            var recipe = new Recipe
            {
                UserID = recipeDto.userID,
                Title = recipeDto.Title,
                Ingridients = recipeDto.Ingridientis,
                Instructions = recipeDto.Instructions,
                ImageUrl = recipeDto.ImageUrl,
                VideoUrl = recipeDto.VideoUrl,
                CreatedAt = DateTime.UtcNow,
                AvaliationsCount = 0,
                IsPublic = recipeDto.IsPublic,
                AverageRating = 0,
                Categories = recipeDto.Categories,
                PreparationTime = recipeDto.PreparationTime
            };

            _context.Recipe.Add(recipe);
            await _context.SaveChangesAsync();

            var readDto = new CreateRecipeDto
            {
                id = recipe.id,
                userID = recipe.UserID,
                Title = recipe.Title,
                Ingridientis = recipe.Ingridients,
                Instructions = recipe.Instructions,
                ImageUrl = recipe.ImageUrl,
                VideoUrl = recipe.VideoUrl,
                CreatedAt = recipe.CreatedAt,
                IsPublic = recipe.IsPublic,
                Categories = recipe.Categories,
                PreparationTime = recipe.PreparationTime
            };

            return Ok(new
            {
                message = "Receita criada com sucessos",
                receita = readDto
            });
        }
    }
}
