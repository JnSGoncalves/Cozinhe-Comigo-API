using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Cozinhe_Comigo_API.Models;

namespace Cozinhe_Comigo_API.DTOS
{
    public class CreateAvaliationDto {
        [Required]
        public int recipeId { get; set; }
        [Required]
        public int rating { get; set; }
        [Required]
        public int userId { get; set; }
        public string content { get; set; }
    }
}
