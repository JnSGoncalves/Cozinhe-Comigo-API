using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cozinhe_Comigo_API.Models {
    public class Recipe {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int UserID { get; set; } // ID do usuário que criou a receita
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        [Required]
        public List<string> Ingredients { get; set; }
        [Required]
        public string Instructions { get; set; }
        public string? ImageUrl { get; set; }
        public string? VideoUrl { get; set; }
        public int? PreparationTime { get; set; } // Em minutos
        public int? Portions { get; set; } // Porções por receita / Quantas pessoas a receita serve
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int AvaliationsCount { get; set; } = 0; // Número de avaliações recebidas
        public double AverageRating { get; set; } = 0;
        // ToDO: Ao criar uma avaliação, incrementar o campo AvaliationsCount e recalcular o campo AvarageRating
        public int FavoritesCount { get; set; } = 0;
        public bool IsPublic { get; set; } = true; // Indica se a receita é pública ou privada
        public List<string> Categories { get; set; } = new List<string>(); // Categoria da receita (ex: Sobremesa, Prato Principal, etc.)

        public Recipe() { }

        public Recipe(string title, List<string> ingredients, string instructions) {
            Title = title;
            Ingredients = ingredients;
            Instructions = instructions;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
