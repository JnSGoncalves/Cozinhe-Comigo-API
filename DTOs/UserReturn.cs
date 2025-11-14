using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Cozinhe_Comigo_API.DTOs {
    public class UserReturn {
        public int? id { get; set; }
        public string? Name { get; set; }
        public string? email { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? ProfirePictureUrl { get; set; }
        public string? Biography { get; set; }
        public string? FavoriteRecipesID { get; set; }
    }
}
