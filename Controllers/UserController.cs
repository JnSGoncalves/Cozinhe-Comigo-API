using Microsoft.AspNetCore.Mvc;
using Cozinhe_Comigo_API.Models;
using Cozinhe_Comigo_API.Data;
using Cozinhe_Comigo_API.DTOs;
using Cozinhe_Comigo_API.DTOS;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

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
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();
            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<User>> InsertUser([FromBody] UserDto userDto)
        {
            try
            {
                var user = new User
                {
                    Name = userDto.name,
                    email = userDto.email,
                    passWord = userDto.passWord,
                    ProfirePictureUrl = userDto.profirePictureUrl,
                    Biography = userDto.biography,
                    FavoriteRecipesID = userDto.favoriteRecipesId
                };



                if (!user.validateEmail(user.email))
                {
                    return BadRequest(new ReturnDto<User>(
                        EInternStatusCode.BAD_REQUEST,
                        @"You need to be use a valid e-mail",
                        null
                    ));
                }
                ;

                if (!user.validateName(user.Name))
                {
                    return BadRequest(new ReturnDto<User>(
                        EInternStatusCode.BAD_REQUEST,
                        @"The name must contain at least 2 characters.",
                        null
                    ));;
                }

                if (!user.validatePassWord(user.passWord))
                {
                    return BadRequest(new ReturnDto<User>(
                        EInternStatusCode.BAD_REQUEST,
                        @"The password must contain at least 6 characters.",
                        null
                    ));;
                }

                var existingUser = await _context.Users.FirstOrDefaultAsync
                (u => u.email == user.email);
                if (existingUser != null)
                {
                    return BadRequest(new ReturnDto<User>(
                        EInternStatusCode.BAD_REQUEST,
                        "The email is already registered.",
                        null
                    ));;
                }

                var passwordHasher = new PasswordHasher<User>();
                user.passWord = passwordHasher.HashPassword(user, user.passWord);

                _context.Users.Add(user);
                int result = await _context.SaveChangesAsync();

                if(result == 0)
                {
                    return StatusCode(500, new
                    {
                        StatusCode = EInternStatusCode.DB_ERROR,
                        ReturnMessage = "Failed to save user. No rows affected."
                    });
                }

                return CreatedAtAction(nameof(GetUser), new { id = user.id }, new
                {
                    InternStatusCode = 0,
                    message = "Registration successful",
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
                
            } catch (Exception ex)
            {
                Console.WriteLine("Internal error");
                Console.WriteLine(ex.Message);

                return StatusCode(500, new
                {
                    StatusCode = EInternStatusCode.INTERNAL_ERROR,
                    ReturnMessage = "Internal server error while saving user.",
                });
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginRequest login)
        {
            if (string.IsNullOrEmpty(login.Email) || string.IsNullOrEmpty(login.PassWord))
            {
                return BadRequest(new ReturnDto<User>(
                        EInternStatusCode.BAD_REQUEST,
                        "Email and password are required.",
                        null
                    ));;
            }
            ;

            var user = await _context.Users.FirstOrDefaultAsync(u => u.email == login.Email);
            if (user == null)
            {
                return BadRequest(new ReturnDto<User>(
                        EInternStatusCode.BAD_REQUEST,
                        "User not found",
                        null
                    ));
            }

            var passwordHasher = new PasswordHasher<User>();
            var result = passwordHasher.VerifyHashedPassword(user, 
            user.passWord, login.PassWord);

            if (result == PasswordVerificationResult.Failed)
            {
                return BadRequest(new ReturnDto<User>(
                    EInternStatusCode.BAD_REQUEST,
                    "Incorrect email or password",
                    null
                ));
            }

            var lastToken = await _context.Tokens.Where(t => t.UserId == user.id).FirstOrDefaultAsync();
            Token token;

            if (lastToken != null)
            {
                if (lastToken.ExpiredAt > DateTime.UtcNow)
                {
                    lastToken.LastLoginAt = DateTime.UtcNow;
                    _context.Tokens.Update(lastToken);
                    token = lastToken;
                }
                else
                {
                    _context.Tokens.Remove(lastToken);

                    string tokenCode = Models.User.GenerateLoginToken();
                    token = new Token(user.id, tokenCode);
                    _context.Tokens.Add(token);
                }
            }
            else
            {
                string tokenCode = Models.User.GenerateLoginToken();
                token = new Token(user.id, tokenCode);
                _context.Tokens.Add(token);
            }

            await _context.SaveChangesAsync();

            return Ok(new
            {
                InternStatusCode = 0,
                message = "Login successful.",
                user = new { user.id, user.Name, user.email },
                token = token.TokenCode
            });
        }

    }
}
