namespace Receitas.Models
{
    public class Avaliation
    {
        public int Id { get; set; }
        public int RecipeId { get; set; } // ID da receita associada ao comentário
        public int Rating { get; set; } // Avaliação associada ao comentário (1 a 5 estrelas)
        public int UserId { get; set; } // ID do usuário que fez o comentário
        public string Content { get; set; } // Conteúdo do comentário
        public DateTime CreatedAt { get; set; } // Data e hora em que o comentário foi criado
       
        public Avaliation(int recipeId, int userId, string content)
        {
            RecipeId = recipeId;
            UserId = userId;
            Content = content;
            CreatedAt = DateTime.Now;
        }
    }
}