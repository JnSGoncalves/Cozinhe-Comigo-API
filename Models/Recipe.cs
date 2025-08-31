
namespace Receitas.Models
{
    public class Recipe
    {
        public int Id { get; set; }
        public int UserId { get; set; } // ID do usuário que criou a receita
        public string Title { get; set; }
        public List<string> Ingredients { get; set; }
        public List<string> Instructions { get; set; }
        public string? ImageUrl { get; set; }
        public string? VideoUrl { get; set; }
        public int? PreparationTime { get; set; } // Em minutes
        public int? Portions { get; set; } // Porções por receita / Quantas pessoas a receita serve
        public DateTime CreatedAt { get; set; }
        public int AvaliationsCount { get; set; } = 0;  // Número de avaliações recebidas
        public int AvarageRating { get; set; } = 0;
        // ToDo: Ao criar uma avaliação, incrementar o campo AvaliationsCount e recalcular o campo AvarageRating
        public int FavoritesCount { get; set; } = 0;
        public bool IsPublic { get; set; } = true; // Indica se a receita é pública ou privada
        public List<CategoryEnum>? Categories { get; set; } // Categoria da receita (ex: Sobremesa, Prato Principal, etc.)

        public Recipe(string title, List<string> ingredients, List<string> instructions)
        {
            Title = title;
            Ingredients = ingredients;
            Instructions = instructions;
        }
    }
}