using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cozinhe_Comigo_API.Models {
    [Table("recipes")]
    public class Recipe {
        // Todo: Adicionar uma coluna de quantidade de chamadas por id, para ser usado no order by
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }
        [Required]
        [Column("userid")]
        public int UserID { get; set; } // ID do usuário que criou a receita
        [Required]
        [MaxLength(100)]
        [Column("title")]
        public string Title { get; set; }
        [Required]
        [Column("ingredients", TypeName = "text[]")]
        public List<string> Ingredients { get; set; }
        [Required]
        [Column("instructions")]
        public string Instructions { get; set; }
        [Column("imageurl")]
        public string? ImageUrl { get; set; }
        [Column("videourl")]
        public string? VideoUrl { get; set; }
        [Column("preparationtime")]
        public int? PreparationTime { get; set; } // Em minutos
        [Column("portions")]
        public int? Portions { get; set; } // Quantas pessoas a receita serve
        [Column("createdat")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [Column("avaliationscount")]
        public int AvaliationsCount { get; set; } = 0; // Número de avaliações recebidas
        [Column("averagerating")]
        public double AverageRating { get; set; } = 0;
        [Column("favoritescount")]
        public int FavoritesCount { get; set; } = 0;
        [Column("ispublic")]
        public bool IsPublic { get; set; } = true; // Receita pública ou privada
        [Column("categories", TypeName = "text[]")]
        public List<string> Categories { get; set; } = new List<string>();
        public Recipe() { }

        public Recipe(string title, List<string> ingredients, string instructions) {
            Title = title;
            Ingredients = ingredients;
            Instructions = instructions;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
