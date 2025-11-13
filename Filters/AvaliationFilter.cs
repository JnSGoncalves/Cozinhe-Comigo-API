namespace Cozinhe_Comigo_API.Filters
{
    public class AvaliationFilter
    {
        public int RecipeId { get; set; }  // obrigatório
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
