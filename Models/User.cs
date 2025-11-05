using System.Runtime;
using System.Text.RegularExpressions;

namespace Cozinhe_Comigo_API.Models
{
    // Essa área será implementada em Python com FastAPI pelo Wallace
    public class User
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string email { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ProfirePictureUrl { get; set; }
        public string? Biography { get; set; }
        public string? FavoriteRecipesID { get; set; }
        public string passWord { get; set; }

        // Receitas são linkadas pelo UserId na classe Recipe
        // Avaliações são linkadas pelo UserId na classe Avaliation

        // Senha e autenticação serão tratadas em outro módulo/serviço
        // Não sendo armazenados diretamente aqui por questões de segurança
        // Armazenada no banco de dados e Recebida pelo Frontend criptografada e validada por outro serviço
        // sem passar pelo código
        // Poderia ser implemenado um token para manter a sessão do usuário


        public User(string name, string email)
        {
            Name = name;
            this.email = email;
            CreatedAt = DateTime.Now;
        }


        public bool validateEmail(string email)
        {
            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            Regex regex = new Regex(emailPattern);
            if (regex.IsMatch(email))
            {
                return true;
            }

            return false;
        }

        public bool validateName(string name)
        {
            if (name.Length < 2)
            {
                return false;
            }
            return true;
        }

        public bool validatePassWord(string passWord)
        {
            if (passWord.Length <= 5)
            {
                return false;
            }

            return true;
        }

    }
}