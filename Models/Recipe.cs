namespace Cozinhe_Comigo_API.Models {
    public class Recipe
    {
        // Todo: Rever a nulicidade de alguns campos
        // Todo: Verificar a necessidade de validações de dados
        public int Id { get; set; }
        public int UserId { get; set; } // ID do usuário que criou a receita
        public string Title { get; set; }
        public string? Description { get; set; }
        public List<string> Ingredients { get; set; }
        public List<string> Instructions { get; set; }
        public string? ImageUrl { get; set; }
        public string? VideoUrl { get; set; }
        public int? PreparationTime { get; set; } // Em minutes
        public int? Portions { get; set; } // Porções por receita / Quantas pessoas a receita serve
        public DateTime CreatedAt { get; set; }
        public int AvaliationsCount { get; set; } = 0; // Número de avaliações recebidas
        public double AvarageRating { get; set; } = 0;
        // ToDO: Ao criar uma avaliação, incrementar o campo AvaliationsCount e recalcular o campo AvarageRating
        public int FavoritesCount { get; set; } = 0;
        public bool IsPublic { get; set; } = true; // Indica se a receita é pública ou privada
        public List<Category>? Categories { get; set; } // Categoria da receita (ex: Sobremesa, Prato Principal, etc.)

        public Recipe(int id, string title, List<string> ingredients, List<string> instructions)
        {
            Id = id;
            Title = title;
            Ingredients = ingredients;
            Instructions = instructions;
        }
    }
}
