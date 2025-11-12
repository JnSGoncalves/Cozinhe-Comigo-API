using Cozinhe_Comigo_API.Models;

namespace Cozinhe_Comigo_API.Filters
{
    public class RecipeFilter
    {
        public string? RecipeId { get; set; }
        public string? TitleSearch { get; set; }
        public int? MinPreparationTime { get; set; }
        public int? MaxPreparationTime { get; set; }
        public int? MinRating { get; set; }
        public int? MaxRating { get; set; }
        public int? MinPortions { get; set; }
        public int? MaxPortions { get; set; }
        // Todo: Talvez seja util adicionar filtros por ingredientes quando fazer a integração com o
        // modelo de IA
        // public List<string>? Ingredients { get; set; }
        public List<string>? Categories { get; set; }
        public int? UserId { get; set; } // Filtrar receitas por usuário específico
        // Todo: Caso seja falso, só retorna receitas privadas do usuário que fez a requisição
        public bool? IsPublic { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public bool FullResult { get; set; } = false;
        public SortByEnum? SortBy { get; set; } = SortByEnum.AverageRating;
        public bool SortDescending { get; set; } = false;
    }

    // Todo: Adicionar mais opções de ordenação se necessário
    // Todo: Verificar se faz sentido manter o enum aqui ou mover para Models
    public enum SortByEnum
    {
        Title,
        PreparationTime,
        Portions,
        CreatedAt,
        AvaliationsCount,
        AverageRating,
    }
}
