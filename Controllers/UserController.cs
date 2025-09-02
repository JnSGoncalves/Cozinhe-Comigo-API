using Microsoft.AspNetCore.Mvc;
using Cozinhe_Comigo_API.Models;

namespace Cozinhe_Comigo_API.Controllers
{
    [Route("CozinheComigoAPI/[controller]")]

    // diz que essa classe vai ser usada para controlar requisições HTTP (GET, POST, PUT, DELETE)
    [ApiController]
    public class UserController : ControllerBase
    {
       
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Users>>> GetUsers()
        {
            //TODO: implementar lógica para busca de todos os usuários no banco de dados.
            var users = new List<Users>
            {
                new Users("Wallace", "wallace@gmail.com"),
                new Users("Jonatas" , "Jonatas@gmail.com"),
            };
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Users>>> GetUser(long id)
        {

            // TODO: implementar lógica de busca por ID no banco de dados.
            var user = new Users("Wallace", "Wallaceizidoro.1@gmail.com");

            return Ok(user);
        }

        [HttpPost]
        public ActionResult<Users> InsertUser([FromBody] Users user)
        {
            if (!user.validateEmail(user.Email))
            {
                return ValidationProblem("E-mail inválido.");
            }

            if (!user.validateName(user.Name))
            {
                return ValidationProblem("O Nome deve ter no mínimo 2 caracteres.");
            }

            // TODO: Implementar lógica para salvar no banco de dados.

            return Ok(user.Name + " Add to data base.");
        }

        [HttpPut("{id}")]
        public ActionResult<Users> UpdateUser(long id, [FromBody] Users user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            //TODO: implementar lógica do banco de dados.
            return Ok(user);
        }

        [HttpDelete("{id}")]
        public ActionResult<Users> DeleteUser(long id)
        {
            //TODO: Implementar lógica do banco de dados.
            return NoContent();
        }

    }
}