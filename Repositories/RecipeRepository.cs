using Cozinhe_Comigo_API.Data;

namespace Cozinhe_Comigo_API.Repositories {
    /// <summary>
    /// Classe responsável por realizar as queries no banco de dados
    /// </summary>
    public interface IRecipeRepository { 
        
    }

    public class RecipeRepository : IRecipeRepository {
        private readonly AppDbContext _context;
        public RecipeRepository(AppDbContext context) {
            _context = context;
        }
    }
}
