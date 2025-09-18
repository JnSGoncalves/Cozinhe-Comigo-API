namespace Cozinhe_Comigo_API.Models {
    public class Category {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}

public enum PreferencesEnum
{
    Doces,
    Salgados,
    ZeroLactose,
    Fitnes,
    Vegano,
    Vegetariano,
    SemGluten,
    LowCarb,
    Proteico,
    Gourmet,
    Rápido,
    Tradicional,
    Internacional,
    Light,
    Orgânico,
    Picante,
    FrutosDoMar,
    Massa,
    Sopas,
    Lanches,
    CaféDaManhã,
    Sobremesa,
}