using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Cozinhe_Comigo_API.Models;

namespace Cozinhe_Comigo_API.DTOS
{
    public class UserDto {
        [Required]
        public string name { get; set; }
        [Required]
        public string email { get; set; }
        public string profirePictureUrl { get; set; }
        public string biography { get; set; }
        public string favoriteRecipesId { get; set; }
        [Required]
        public string passWord { get; set; }
    }
}
