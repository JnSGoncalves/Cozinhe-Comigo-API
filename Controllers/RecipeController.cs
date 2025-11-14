using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cozinhe_Comigo_API.Models;
using Cozinhe_Comigo_API.Filters;
using Cozinhe_Comigo_API.Data;
using Cozinhe_Comigo_API.DTOs;
using Cozinhe_Comigo_API.DTOS;
using System.Linq;

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
            [FromHeader] string requesterUserToken)
        {
            try {
                var userIdToken = await _context.Tokens
                    .Where(u => u.TokenCode == requesterUserToken)
                    .Select(u => u.UserId)
                    .FirstOrDefaultAsync();

                if (userIdToken == 0) {
                    return BadRequest(new ReturnDto<Recipe>(
                        EInternStatusCode.BAD_REQUEST,
                        "You need to be authenticated to create a new recipe.",
                        null
                    ));
                }

                if (recipeDto.UserID != userIdToken) { 
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

                _context.Recipes.Add(recipe);
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

        // GET: api/recipe
        [HttpGet]
        public async Task<ActionResult> Get(
            int? id,
            [FromQuery] RecipeFilter filter,
            [FromHeader] string? requesterUserToken = null
        ) {
            Token? token = null;

            // Se token foi enviado → validar
            if (!string.IsNullOrEmpty(requesterUserToken)) {
                token = await _context.Tokens.FirstOrDefaultAsync(t => t.TokenCode == requesterUserToken);

                if (token == null || token.ExpiredAt < DateTime.UtcNow) {
                    return BadRequest(new ReturnDto<List<Recipe>>(
                        EInternStatusCode.BAD_REQUEST,
                        "Invalid or expired authentication token.",
                        null
                    ));
                }
            }

            if (id.HasValue) {
                var recipe = await _context.Recipes.FirstOrDefaultAsync(r => r.Id == id.Value);

                if (recipe == null) {
                    return Ok(new ReturnDto<Recipe>(
                        EInternStatusCode.NOT_FOUND,
                        "Recipe not found",
                        null
                    ));
                }

                // Se receita é privada → somente dono pode ver
                if (!recipe.IsPublic) {
                    if (token == null || token.UserId != recipe.UserID) {
                        return BadRequest(new ReturnDto<Recipe>(
                            EInternStatusCode.UNAUTHORIZED,
                            "This recipe is private and can only be viewed by the creator.",
                            null
                        ));
                    }
                }

                return Ok(new ReturnDto<Recipe>(
                    EInternStatusCode.OK,
                    "Query executed successfully",
                    recipe
                ));
            }

            if (!string.IsNullOrEmpty(requesterUserToken)) {
                token = await _context.Tokens.FirstOrDefaultAsync(t => t.TokenCode == requesterUserToken);

                if (token == null || token.ExpiredAt < DateTime.UtcNow) {
                    return BadRequest(new ReturnDto<List<Recipe>>(
                        EInternStatusCode.BAD_REQUEST,
                        "Invalid or expired authentication token.",
                        null
                    ));
                }
            }

            var query = _context.Recipes.AsNoTracking().AsQueryable();

            if (token == null) {
                query = query.Where(r => r.IsPublic);
            } else {
                query = query.Where(r =>
                    r.IsPublic ||
                    (r.UserID == token.UserId)
                );
            }

            if (!string.IsNullOrEmpty(filter.TitleSearch))
                query = query.Where(r => EF.Functions.ILike(r.Title, $"%{filter.TitleSearch}%"));

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

            if (filter.Categories != null && filter.Categories.Count > 0) {
                query = query.Where(r =>
                    r.Categories.Any(c =>
                        filter.Categories.Contains(c))
                );
            }

            if (filter.SortBy.HasValue) {
                query = filter.SortBy switch {
                    SortByEnum.Title => filter.SortDescending ? query.OrderByDescending(r => r.Title) : query.OrderBy(r => r.Title),
                    SortByEnum.PreparationTime => filter.SortDescending ? query.OrderByDescending(r => r.PreparationTime) : query.OrderBy(r => r.PreparationTime),
                    SortByEnum.Portions => filter.SortDescending ? query.OrderByDescending(r => r.Portions) : query.OrderBy(r => r.Portions),
                    SortByEnum.CreatedAt => filter.SortDescending ? query.OrderByDescending(r => r.CreatedAt) : query.OrderBy(r => r.CreatedAt),
                    SortByEnum.AvaliationsCount => filter.SortDescending ? query.OrderByDescending(r => r.AvaliationsCount) : query.OrderBy(r => r.AvaliationsCount),
                    SortByEnum.AverageRating => filter.SortDescending ? query.OrderByDescending(r => r.AverageRating) : query.OrderBy(r => r.AverageRating),
                    _ => query
                };
            }

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)filter.PageSize);

            query = query
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize);

            if (filter.FullResult) {
                var recipes = await query.ToListAsync();

                return Ok(new ReturnDto<List<Recipe>>(
                    EInternStatusCode.OK,
                    "Query executed successfully",
                    recipes
                ) {
                    TotalItems = totalItems,
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    TotalPages = totalPages
                });
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
                    "Query executed successfully",
                    recipes
                ) {
                    TotalItems = totalItems,
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    TotalPages = totalPages
                });
            }
        }
    }
}
