using Microsoft.AspNetCore.Mvc;
using Cozinhe_Comigo_API.Models;
using Cozinhe_Comigo_API.Data;
using Cozinhe_Comigo_API.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;


//TODO: Implementar melhor resposta para confirmação de cadastro.
//TODO: implementar hash para melhor tratamento da senha.
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

            if (!user.validatePassWord(user.passWord))
            {
                return ValidationProblem("A senha deve ter 6 ou mais caracteres!");
            }

            var existingUser = await _context.User.FirstOrDefaultAsync
            (u => u.email == user.email);
            if (existingUser != null)
                return Conflict(new { message = "Este e-mail já está cadastrado." });

            var passwordHasher = new PasswordHasher<User>();
            user.passWord = passwordHasher.HashPassword(user, user.passWord);

            _context.User.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { id = user.id }, new
            {
                message = "Cadastro realizado com sucesso.",
                user = new
                {
                    user.id,
                    user.Name,
                    user.email,
                    user.CreatedAt,
                    user.ProfirePictureUrl,
                    user.Biography,
                    user.FavoriteRecipesID
                }
            });
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginRequest login)
        {
            if (string.IsNullOrEmpty(login.Email) || string.IsNullOrEmpty(login.PassWord))
                return BadRequest(new { message = "E-mail e senha são obrigatórios." });

            var user = await _context.User.FirstOrDefaultAsync(u => u.email == login.Email);
            if (user == null)
                return Unauthorized(new { message = "Usuário não encontrado." });

            var passwordHasher = new PasswordHasher<User>();
            var result = passwordHasher.VerifyHashedPassword(user, user.passWord, login.PassWord);

            if (result == PasswordVerificationResult.Failed)
                return Unauthorized(new { 
                    message = "Seu email ou sua senha está incorreta." 
                    });

            return Ok(new
            {
                message = "Login efetuado com sucesso.",
                user = new { user.id, user.Name, user.email }
            });
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
