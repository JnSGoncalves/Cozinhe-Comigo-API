using Microsoft.AspNetCore.Mvc;
using Receitas.Models;
using Cozinhe_Comigo_API.Data;
using Microsoft.EntityFrameworkCore;
using Cozinhe_Comigo_API.Models;
using Cozinhe_Comigo_API.DTOs;
using Cozinhe_Comigo_API.DTOS;
using Cozinhe_Comigo_API.Filters;
using System.Linq;

namespace Receitas.Controllers
{
    [Route("CozinheComigoAPI/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CommentsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<ReturnDto<Avaliation>>> Post(
            [FromBody] CreateAvaliationDto avaliationDto,
            [FromHeader] string requesterUserToken)
        {
            try
            {
                var userIdToken = await _context.Tokens
                    .Where(u => u.TokenCode == requesterUserToken)
                    .Select(u => u.UserId)
                    .FirstOrDefaultAsync();

                if (userIdToken == 0)
                {
                    return BadRequest(new ReturnDto<Recipe>(
                        EInternStatusCode.BAD_REQUEST,
                        "You need to be authenticated to create a new comments.",
                        null
                    ));
                }

                if (avaliationDto.userId != userIdToken)
                {
                    return BadRequest(new ReturnDto<Recipe>(
                        EInternStatusCode.BAD_REQUEST,
                        @"You need to be authenticated with the same user for
                         whom you are trying to create a new comments.",
                        null
                    ));
                }

                bool recipeExists = await _context.Recipes
                    .AnyAsync(r => r.Id == avaliationDto.recipeId);
                if (!recipeExists)
                {
                    return NotFound(new ReturnDto<Recipe>(
                        EInternStatusCode.BAD_REQUEST,
                        $"Recipe with ID {avaliationDto.recipeId} not found.",
                        null
                    ));
                }

                var avaliation = new Avaliation(
                    avaliationDto.recipeId,
                    avaliationDto.rating,
                    avaliationDto.userId,
                    avaliationDto.content
                );

                _context.avaliations.Add(avaliation);
                int result = await _context.SaveChangesAsync();

                if (result == 0)
                {
                    return StatusCode(500, new
                    {
                        StatusCode = EInternStatusCode.DB_ERROR,
                        ReturnMessage = "Failed to save Avaliation. No rows affected."
                    });
                }

                return Ok(new ReturnDto<Avaliation>(
                    EInternStatusCode.OK,
                    "Successfully created",
                    avaliation
                ));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Internal Error");
                Console.WriteLine(ex.Message);

                return StatusCode(500, new
                {
                    StatusCode = EInternStatusCode.INTERNAL_ERROR,
                    ReturnMessage = "Internal server error while saving recipe.",
                });
            }
        }
        
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] AvaliationFilter filter)
        {
            if (filter.PageSize <= 0)
            {
                return BadRequest(new ReturnDto<List<Avaliation>>(
                    EInternStatusCode.BAD_REQUEST,
                    "Invalid page size.",
                    null
                ));
            }

            if (filter.PageNumber <= 0)
            {
                return BadRequest(new ReturnDto<List<Avaliation>>(
                    EInternStatusCode.BAD_REQUEST,
                    "Invalid page number.",
                    null
                ));
            }

            if (filter.PageSize > 100)
            {
                return BadRequest(new ReturnDto<List<Avaliation>>(
                    EInternStatusCode.BAD_REQUEST,
                    "Page size too large. Maximum is 100.",
                    null
                ));
            }

            if (filter.RecipeId <= 0)
            {
                return BadRequest(new ReturnDto<List<Avaliation>>(
                    EInternStatusCode.BAD_REQUEST,
                    "RecipeId is required.",
                    null
                ));
            }

            var query = _context.avaliations
                .AsNoTracking()
                .Where(a => a.RecipeId == filter.RecipeId)
                .AsQueryable();

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)filter.PageSize);

            var paginated = await query
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            return Ok(new ReturnDto<List<Avaliation>>(
                EInternStatusCode.OK,
                "Query executed successfully",
                paginated
            )
            {
                TotalItems = totalItems,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
                TotalPages = totalPages
            });
        }
    }
}