using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Cozinhe_Comigo_API.Models;

namespace Cozinhe_Comigo_API.DTOS
{
    public class CreateRecipeDto {
        [Required]
        public int UserID { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public List<string> Ingredients { get; set; }
        [Required]
        public string Instructions { get; set; }
        public string? ImageUrl { get; set; }
        public string? VideoUrl { get; set; }
        public int? PreparationTime { get; set; }
        public int? Portions { get; set; }
        public bool IsPublic { get; set; } = true;
        [Required]
        public List<string> Categories { get; set; }
    }
}
