using Cozinhe_Comigo_API.Repositories;

namespace Cozinhe_Comigo_API.Services {
    public interface IRecipeService {

    }

    public class RecipeService : IRecipeService {
        private readonly IRecipeRepository _repo;

        public RecipeService(IRecipeRepository repo) {
            _repo = repo;
        }


    }
}