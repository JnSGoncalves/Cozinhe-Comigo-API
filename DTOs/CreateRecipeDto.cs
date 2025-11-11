using Cozinhe_Comigo_API.Models;

namespace Cozinhe_Comigo_API.DTOS
{
    public class CreateRecipeDto
    {
        public int id { get; set; }
        
        public  int userID { get; set; }
        public string Title { get; set; }
        public string Ingridientis { get; set; }

        public string? Instructions { get; set; }

        public string? ImageUrl { get; set; }
        public string? VideoUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsPublic { get; set; }
        public string Categories { get; set; }
        public int? PreparationTime { get; set; }
    }
}
