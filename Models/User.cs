using System.Runtime;
using System.Text.RegularExpressions;

namespace Cozinhe_Comigo_API.Models {
    // Essa área será implementada em Python com FastAPI pelo Wallace
    public class Users
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public string? Bio { get; set; }
        public List<int> FavoriteRecipesIds { get; set; } = new List<int>();

        public List<PreferencesEnum> Preferences { get; set; } = new List<PreferencesEnum>();

        // Receitas são linkadas pelo UserId na classe Recipe
        // Avaliações são linkadas pelo UserId na classe Avaliation

        // Senha e autenticação serão tratadas em outro módulo/serviço
        // Não sendo armazenados diretamente aqui por questões de segurança
        // Armazenada no banco de dados e Recebida pelo Frontend criptografada e validada por outro serviço
        // sem passar pelo código
        // Poderia ser implemenado um token para manter a sessão do usuário


        public Users(string name, string email)
        {
            Name = name;
            Email = email;
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

    }
}