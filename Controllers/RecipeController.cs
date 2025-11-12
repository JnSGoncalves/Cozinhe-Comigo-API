using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cozinhe_Comigo_API.Models;
using Cozinhe_Comigo_API.Filters;
using Cozinhe_Comigo_API.Data;
using Cozinhe_Comigo_API.DTOs;
using Cozinhe_Comigo_API.DTOS;
using System.Linq;


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
        public async Task<ActionResult<ReturnDto<Recipe>>> Post(
            [FromBody] CreateRecipeDto recipeDto, 
            [FromHeader] string? requesterUserToken = null)
        {
            try {
                var userIdRecipe = _context.User.Where(u => u.id == recipeDto.UserID).Select(u => u.id).FirstOrDefault();
                var userIdToken = _context.Token
                    .Where(u => u.TokenCode.Equals(requesterUserToken))
                    .Select(u => u.id)
                    .FirstOrDefault();

                if (userIdRecipe != userIdToken) { 
                    return BadRequest(new ReturnDto<Recipe>(
                        EInternStatusCode.BAD_REQUEST,
                        "You need to be authenticated with the same user for whom you are trying to create a new recipe.",
                        null
                    ));
                }

                var recipe = new Recipe
                {
                    UserID = recipeDto.UserID,
                    Title = recipeDto.Title,
                    Ingredients = recipeDto.Ingredients,
                    Instructions = recipeDto.Instructions,
                    ImageUrl = recipeDto.ImageUrl,
                    VideoUrl = recipeDto.VideoUrl,
                    CreatedAt = DateTime.UtcNow,
                    AvaliationsCount = 0,
                    IsPublic = recipeDto.IsPublic,
                    AverageRating = 0,
                    Categories = recipeDto.Categories,
                    Portions = recipeDto.Portions,
                    PreparationTime = recipeDto.PreparationTime
                };

                _context.Recipe.Add(recipe);
                int result = await _context.SaveChangesAsync();

                if (result == 0) {
                    return StatusCode(500, new {
                        StatusCode = EInternStatusCode.DB_ERROR,
                        ReturnMessage = "Failed to save recipe. No rows affected."
                    });
                }

                return Ok(new ReturnDto<Recipe>(
                    EInternStatusCode.OK,
                    "Successfully created",
                    recipe
                ));
            } catch (Exception ex) {
                Console.WriteLine("Internal Error");
                Console.WriteLine(ex.Message);

                return StatusCode(500, new {
                    StatusCode = EInternStatusCode.INTERNAL_ERROR,
                    ReturnMessage = "Internal server error while saving recipe.",
                });
            }
        }

        [HttpGet]
        public async Task<ActionResult<ReturnDto<List<Recipe>>>> Get(
            [FromQuery] RecipeFilter filter, 
            [FromHeader] string? requesterUserToken = null
        ) {
            if(requesterUserToken == null && filter.IsPublic == false){
                return BadRequest(new ReturnDto<ReadRecipeDto>(
                    EInternStatusCode.BAD_REQUEST,
                    "To view a private recipe, you must authenticate yourself as the recipe's creator.",
                    null
                ));
            }

            var query = _context.Recipe.AsQueryable();

            if (!string.IsNullOrEmpty(filter.TitleSearch))
                query = query.Where(r => r.Title.ToLower().Contains(filter.TitleSearch.ToLower()));

            if (filter.MinPreparationTime.HasValue)
                query = query.Where(r => r.PreparationTime >= filter.MinPreparationTime);
            if (filter.MaxPreparationTime.HasValue)
                query = query.Where(r => r.PreparationTime <= filter.MaxPreparationTime);

            if (filter.MinRating.HasValue)
                query = query.Where(r => r.AverageRating >= filter.MinRating);
            if (filter.MaxRating.HasValue)
                query = query.Where(r => r.AverageRating <= filter.MaxRating);

            if (filter.MinPortions.HasValue)
                query = query.Where(r => r.Portions >= filter.MinPortions);
            if (filter.MaxPortions.HasValue)
                query = query.Where(r => r.Portions <= filter.MaxPortions);

            if (filter.UserId.HasValue)
                query = query.Where(r => r.UserID == filter.UserId);

            query = query.Where(r => r.IsPublic == filter.IsPublic);

            if (filter.Categories != null && filter.Categories.Count > 0) {
                query = query.Where(r =>
                    r.Categories.Any(c =>
                        filter.Categories.Contains(c))
                );
            }

            if (filter.SortBy.HasValue) {
                query = filter.SortBy switch {
                    SortByEnum.Title => filter.SortDescending
                        ? query.OrderByDescending(r => r.Title)
                        : query.OrderBy(r => r.Title),

                    SortByEnum.PreparationTime => filter.SortDescending
                        ? query.OrderByDescending(r => r.PreparationTime)
                        : query.OrderBy(r => r.PreparationTime),

                    SortByEnum.Portions => filter.SortDescending
                        ? query.OrderByDescending(r => r.Portions)
                        : query.OrderBy(r => r.Portions),

                    SortByEnum.CreatedAt => filter.SortDescending
                        ? query.OrderByDescending(r => r.CreatedAt)
                        : query.OrderBy(r => r.CreatedAt),

                    SortByEnum.AvaliationsCount => filter.SortDescending
                        ? query.OrderByDescending(r => r.AvaliationsCount)
                        : query.OrderBy(r => r.AvaliationsCount),

                    SortByEnum.AverageRating => filter.SortDescending
                        ? query.OrderByDescending(r => r.AverageRating)
                        : query.OrderBy(r => r.AverageRating),

                    _ => query
                };
            }

            if (filter.FullResult) {
                var recipes = await query.ToListAsync();

                return Ok(new ReturnDto<List<Recipe>>(
                    EInternStatusCode.OK,
                    "Query executed",
                    recipes
                ));
            } else {
                var recipes = await query
                .Select(r => new ReadRecipeDto {
                    Id = r.Id,
                    Title = r.Title,
                    AverageRating = r.AverageRating,
                    PreparationTime = r.PreparationTime,
                    Categories = r.Categories,
                    ImageUrl = r.ImageUrl
                })
                .ToListAsync();

                return Ok(new ReturnDto<List<ReadRecipeDto>>(
                    EInternStatusCode.OK,
                    "Query executed",
                    recipes
                ));
            }
        }
    }
}
