using Microsoft.AspNetCore.Mvc;
using Cozinhe_Comigo_API.Models;
using Cozinhe_Comigo_API.Data;
using Microsoft.EntityFrameworkCore;

namespace Cozinhe_Comigo_API.Controllers
{
    [Route("CozinheComigoAPI/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var users = await _context.User.ToListAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(long id)
        {
            var user = await _context.User.FindAsync(id);
            if (user == null)
                return NotFound();
            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<User>> InsertUser([FromBody] User user)
        {
            if (!user.validateEmail(user.email))
                return ValidationProblem("E-mail inválido.");

            if (!user.validateName(user.Name))
                return ValidationProblem("O nome deve ter no mínimo 2 caracteres.");

            _context.User.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { id = user.id }, user);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateUser(long id, [FromBody] User user)
        {
            if (id != user.id)
                return BadRequest();

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(user);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(long id)
        {
            var user = await _context.User.FindAsync(id);
            if (user == null)
                return NotFound();

            _context.User.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
