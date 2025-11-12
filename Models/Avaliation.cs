using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Receitas.Models
{
    public class Avaliation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        [Required]
        public int Id { get; set; }
        [Required]
        [Column("recipeid")]
        public int RecipeId { get; set; } // ID da receita associada ao comentário
        [Column("rating")]
        public int Rating { get; set; } // Avaliação associada ao comentário (1 a 5 estrelas)
        [Required]
        [Column("userid")]
        public int UserId { get; set; } // ID do usuário que fez o comentário
        [Column("content")]
        public string Content { get; set; } // Conteúdo do comentário
        [Column("createdat")]
        public DateTime CreatedAt { get; set; } // Data e hora em que o comentário foi criado
       
        public Avaliation(int recipeId,int rating, int userId, string content)
        {
            RecipeId = recipeId;
            UserId = userId;
            Rating = rating;
            Content = content;
            CreatedAt = DateTime.UtcNow;
        }
    }
}