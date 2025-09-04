using Cozinhe_Comigo_API.Models;

namespace Cozinhe_Comigo_API.DTOS
{
    public class ReadRecipeDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public double AverageRating { get; set; } = 0;
        public List<string> Categories { get; set; } = new List<string>();
        public int? PreparationTime { get; set; }
    }
}
