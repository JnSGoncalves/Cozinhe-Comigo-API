using System.Runtime;

namespace Receitas.Models {
    // Essa área será implementada em Python com FastAPI pelo Wallace
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public string? Bio { get; set; }
        public List<int> FavoriteRecipesIds { get; set; } = new List<int>();

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
            Email = email;
            CreatedAt = DateTime.Now;
        }
    }
}