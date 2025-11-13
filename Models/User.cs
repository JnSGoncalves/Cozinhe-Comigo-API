using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace Cozinhe_Comigo_API.Models
{
    [Table("users")]
    public class User {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int id { get; set; }
        [Required]
        [Column("name")]
        public string Name { get; set; }
        [Required]
        [Column("email")]
        public string email { get; set; }
        [Column("createdat")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [Column("profirepictureurl")]
        public string? ProfirePictureUrl { get; set; }
        [Column("biography")]
        public string? Biography { get; set; }
        [Column("favoriterecipesid")]
        public string? FavoriteRecipesID { get; set; }
        [Required]
        [Column("password")]
        public string passWord { get; set; }
    
        // Receitas são linkadas pelo UserId na classe Recipe
        // Avaliações são linkadas pelo UserId na classe Avaliation

        // Senha e autenticação serão tratadas em outro módulo/serviço
        // Não sendo armazenados diretamente aqui por questões de segurança
        // Armazenada no banco de dados e Recebida pelo Frontend criptografada e validada por outro serviço
        // sem passar pelo código
        // Poderia ser implemenado um token para manter a sessão do usuário
        public User(){}

        public User(string name, string email)
        {
            Name = name;
            this.email = email;
            CreatedAt = DateTime.UtcNow;

        }

        public static string GenerateLoginToken(int size = 64) {
            // Gera um array de bytes aleatórios
            byte[] tokenBytes = RandomNumberGenerator.GetBytes(size);

            // Converte para string em Base64Url
            string token = Convert.ToBase64String(tokenBytes)
                .Replace("+", "-")
                .Replace("/", "_")
                .Replace("=", "");

            return token;
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