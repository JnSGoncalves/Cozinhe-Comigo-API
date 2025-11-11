using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cozinhe_Comigo_API.Models
{
    public class Recipe
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Required]
        public int UserID { get; set; } // ID do usuário que criou a receita

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        public string Ingridients { get; set; }

        [Required]
        public string Instructions { get; set; }

        public string? ImageUrl { get; set; }

        public string? VideoUrl { get; set; }

        public int? PreparationTime { get; set; } // Em minutos

        public int? Portions { get; set; } // Quantas pessoas a receita serve

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int AvaliationsCount { get; set; } = 0; // Número de avaliações recebidas

        public double AverageRating { get; set; } = 0; // Corrigido o nome (era "AvarageRating")

        public bool IsPublic { get; set; } = true; // Indica se a receita é pública ou privada

        public string?  Categories { get; set; }

        // Construtor padrão exigido pelo Entity Framework
        public Recipe() { }

        // Construtor customizado (opcional)
        public Recipe(string title, string ingredients, string instructions)
        {
            Title = title;
            Ingridients = ingredients;
            Instructions = instructions;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
