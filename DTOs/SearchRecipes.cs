namespace Cozinhe_Comigo_API.DTOs {
    public class SearchRecipes {
        public int? Id { get; set; }
        public string? Title { get; set; } = string.Empty;
        public int? AverageRating { get; set; } = 0;
        public List<string> Categories { get; set; } = new List<string>();
        public int? PreparationTime { get; set; }
    }
}
